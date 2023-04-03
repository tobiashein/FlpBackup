using System.Text;

namespace FlpBackup.Parser;

/// <summary>
/// Implementation of a service that parses FL Studio project files.
/// </summary>
public class ProjectParser : IProjectParser
{
    #region Constants

    private const string FLDT_CHUNK = "FLdt";
    private const string FLP_HEADER = "FLhd";

    #endregion Constants

    #region Methods

    #region Private Static Methods

    private static List<string> GetSamples(BinaryReader binaryReader, Version version)
    {
        var samples = new List<string>();

        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
        {
            var @event = (Event)binaryReader.ReadByte();

            if (@event is not Event.SampleFileName)
            {
                SkipEvent(binaryReader, @event);
                continue;
            }

            var sampleFilename = ParseSampleFileName(binaryReader, version);

            samples.Add(sampleFilename);
        }

        return samples;
    }

    private static void ParseDataChunk(BinaryReader binaryReader)
    {
        if (Encoding.ASCII.GetString(binaryReader.ReadBytes(4)) is not FLDT_CHUNK)
            throw new ProjectParserException("Invalid file format.");

        binaryReader.ReadInt32();
    }

    private static void ParseHeaderChunk(BinaryReader binaryReader)
    {
        if (Encoding.ASCII.GetString(binaryReader.ReadBytes(4)) is not FLP_HEADER)
            throw new ProjectParserException("Invalid file format.");

        binaryReader.ReadBytes(10);
    }

    private static string ParseSampleFileName(BinaryReader binaryReader, Version version)
    {
        var byteData = binaryReader.ReadBytes(binaryReader.Read7BitEncodedInt());
        var stringData = version > new Version(12, 0)
            ? Encoding.Unicode.GetString(byteData)
            : Encoding.UTF8.GetString(byteData);
        var sampleFilename = Environment.ExpandEnvironmentVariables(stringData);

        return sampleFilename;
    }

    private static Version ParseVersion(BinaryReader binaryReader)
    {
        if ((Event)binaryReader.ReadByte() is not Event.Version)
            throw new ProjectParserException("Version not found.");

        var byteData = binaryReader.ReadBytes(binaryReader.Read7BitEncodedInt());
        var stringData = Encoding.ASCII.GetString(byteData);
        var version = new Version(stringData);

        return version;
    }

    private static void SkipEvent(BinaryReader binaryReader, Event @event)
    {
        switch (@event)
        {
            case < Event.Word:
                binaryReader.ReadByte();
                break;

            case < Event.Int:
                binaryReader.ReadInt16();
                break;

            case < Event.Text:
                binaryReader.ReadInt32();
                break;

            case < Event.Data:
                binaryReader.ReadBytes(binaryReader.Read7BitEncodedInt());
                break;

            default:
                binaryReader.ReadBytes(binaryReader.Read7BitEncodedInt());
                break;
        }
    }

    #endregion Private Static Methods

    #region Public Methods

    /// <inheritdoc/>
    public Project Parse(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            throw new ProjectParserException("File not found.");

        using var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using var binaryReader = new BinaryReader(fileStream);

        try
        {
            ParseHeaderChunk(binaryReader);
            ParseDataChunk(binaryReader);

            var version = ParseVersion(binaryReader);
            var samples = GetSamples(binaryReader, version);

            return new Project(filename, version, samples);
        }
        catch (Exception ex)
        {
            throw new ProjectParserException("Could not load project.", ex);
        }
    }

    #endregion Public Methods

    #endregion Methods
}