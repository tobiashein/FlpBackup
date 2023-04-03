namespace FlpBackup.Services;

/// <summary>
/// Contract of a service to locate a sample.
/// </summary>
public interface ISampleLocator
{
    #region Methods

    #region Public Methods

    /// <summary>
    /// Gets the path of a sample with the specified <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The file name of the sample.</param>
    /// <param name="projectFolder">The folder of the project referencing the sample.</param>
    /// <param name="flstudioFolder">The FL Studio installation folder, used to locate factory samples.</param>
    /// <returns></returns>
    string? Locate(string filename, string projectFolder, string flstudioFolder);

    #endregion Public Methods

    #endregion Methods
}