namespace FlpBackup.Services;

/// <summary>
/// A result returned by the <see cref="ProjectPackager"/>.
/// </summary>
public class ProjectPackagerResult
{
    #region Properties

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets the collection of successfully located samples.
    /// </summary>
    public IReadOnlyCollection<string> FoundSamples { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets a value indicating whether the result contains any errors.
    /// </summary>
    public bool IsError { get; init; }

    /// <summary>
    /// Gets the collection of unsuccessfully located samples.
    /// </summary>
    public IReadOnlyCollection<string> MissingSamples { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets the package file name.
    /// </summary>
    public string PackageFilename { get; init; } = null!;

    /// <summary>
    /// Gets the project file name.
    /// </summary>
    public string ProjectFilename { get; init; } = null!;

    /// <summary>
    /// Gets the FL Studio version used to create the project.
    /// </summary>
    public Version ProjectVersion { get; init; } = null!;

    #endregion Properties
}