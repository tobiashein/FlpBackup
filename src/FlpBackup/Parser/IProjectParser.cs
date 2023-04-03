namespace FlpBackup.Parser;

/// <summary>
/// Contract of a service that parses FL Studio project files.
/// </summary>
public interface IProjectParser
{
    #region Methods

    #region Public Methods

    /// <summary>
    /// Parses the specified project file.
    /// </summary>
    /// <param name="filename">The project file name.</param>
    Project Parse(string filename);

    #endregion Public Methods

    #endregion Methods
}