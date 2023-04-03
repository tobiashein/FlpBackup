using FlpBackup.Parser;
using Serilog;
using System.IO.Compression;

namespace FlpBackup.Services;

/// <summary>
/// Implementation of a service that packages a project into a zip file.
/// </summary>
public class ProjectPackager : IProjectPackager
{
    #region Constants

    private const string PackageFileExtension = ".zip";

    #endregion Constants

    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectPackager"/>.
    /// </summary>
    /// <param name="sampleLocator">The sample locator.</param>
    /// <param name="logger">The logger.</param>
    public ProjectPackager(ISampleLocator sampleLocator, ILogger logger)
    {
        _sampleLocator = sampleLocator;
        _logger = logger;
    }

    #endregion Constructors

    #region Fields

    private readonly ILogger _logger;
    private readonly ISampleLocator _sampleLocator;

    #endregion Fields

    #region Methods

    #region Public Methods

    /// <inheritdoc/>
    public ProjectPackagerResult Package(Project project, string flstudioFolder, string outputFolder)
    {
        _ = project ?? throw new ArgumentNullException(nameof(project));
        _ = flstudioFolder ?? throw new ArgumentNullException(nameof(flstudioFolder));
        _ = outputFolder ?? throw new ArgumentNullException(nameof(outputFolder));

        try
        {
            Directory.CreateDirectory(outputFolder);

            var projectFolder = Path.GetDirectoryName(project.Filename);
            var outputFilename = Path.Combine(outputFolder, Path.ChangeExtension(Path.GetFileName(project.Filename), PackageFileExtension));

            if (File.Exists(outputFilename))
            {
                _logger.Error("Package already exists: {Details}", new { Project = project.Filename, Package = outputFilename, });

                return new ProjectPackagerResult
                {
                    IsError = true,
                    ErrorMessage = "Package already exists.",
                    ProjectFilename = project.Filename,
                    ProjectVersion = project.Version,
                    PackageFilename = outputFilename,
                };
            }

            using var zipFile = ZipFile.Open(outputFilename, ZipArchiveMode.Create);

            zipFile.CreateEntryFromFile(project.Filename, Path.GetFileName(project.Filename));

            var foundSamples = new HashSet<string>();
            var missingSamples = new HashSet<string>();

            foreach (var sample in project.Samples)
            {
                var sampleFilename = _sampleLocator.Locate(sample, projectFolder!, flstudioFolder);

                if (sampleFilename is null)
                {
                    missingSamples.Add(sample);

                    _logger.Warning("Sample not found: {Details}", new { Project = project.Filename, Sample = sample, });
                }
                else if (!foundSamples.Contains(sampleFilename))
                {
                    foundSamples.Add(sampleFilename);

                    zipFile.CreateEntryFromFile(sampleFilename, Path.GetFileName(sampleFilename));
                }
            }

            return new ProjectPackagerResult
            {
                IsError = false,
                ProjectFilename = project.Filename,
                ProjectVersion = project.Version,
                PackageFilename = outputFilename,
                FoundSamples = foundSamples,
                MissingSamples = missingSamples
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Package could not be created");

            return new ProjectPackagerResult
            {
                IsError = true,
                ErrorMessage = $"Package could not be created: {ex.Message}",
                ProjectFilename = project.Filename,
                ProjectVersion = project.Version
            };
        }
    }

    #endregion Public Methods

    #endregion Methods
}