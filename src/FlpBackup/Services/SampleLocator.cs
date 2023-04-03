namespace FlpBackup.Services;

/// <summary>
/// Implementation of a service to locate a sample.
/// </summary>
public class SampleLocator : ISampleLocator
{
    #region Constants

    private const string Data = "Data";
    private const string FLStudioData = "%FLStudioData%";
    private const string FLStudioFactoryData = "%FLStudioFactoryData%";

    #endregion Constants

    #region Methods

    #region Public Methods

    /// <inheritdoc/>
    public string? Locate(string filename, string projectFolder, string flstudioFolder)
    {
        _ = filename ?? throw new ArgumentNullException(nameof(filename));
        _ = projectFolder ?? throw new ArgumentNullException(nameof(projectFolder));
        _ = flstudioFolder ?? throw new ArgumentNullException(nameof(flstudioFolder));

        if (filename.StartsWith(FLStudioData, StringComparison.OrdinalIgnoreCase))
            filename = filename.Replace(FLStudioData, Path.Combine(flstudioFolder, Data));
        else if (filename.StartsWith(FLStudioFactoryData, StringComparison.OrdinalIgnoreCase))
            filename = filename.Replace(FLStudioFactoryData, flstudioFolder);
        else if (!Path.IsPathFullyQualified(filename))
            filename = Path.Combine(flstudioFolder, Data, Path.GetRelativePath(Path.DirectorySeparatorChar.ToString(), filename));

        if (File.Exists(filename))
            return filename;

        var localFilename = Path.Combine(projectFolder, Path.GetFileName(filename));

        if (File.Exists(localFilename))
            return localFilename;

        return null;
    }

    #endregion Public Methods

    #endregion Methods
}