namespace FlpBackup.Parser;

/// <summary>
/// A project file.
/// </summary>
public class Project
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="Project"/>.
    /// </summary>
    /// <param name="filename">The project file name</param>
    /// <param name="version">The FL Studio version the project was created with.</param>
    /// <param name="samples">The collection of samples used by the project.</param>
    public Project(string filename, Version version, IEnumerable<string> samples)
    {
        Filename = filename;
        Version = version;
        Samples = samples.ToList();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the file name of the project.
    /// </summary>
    public string Filename { get; init; }

    /// <summary>
    /// Gets the samples used by the project.
    /// </summary>
    public List<string> Samples { get; init; }

    /// <summary>
    /// Gets the FL Studio version the project was created with.
    /// </summary>
    public Version Version { get; init; }

    #endregion Properties
}