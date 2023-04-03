using FlpBackup.Parser;

namespace FlpBackup.Services;

/// <summary>
/// Contract of a service that packages a project into a zip file.
/// </summary>
public interface IProjectPackager
{
    #region Methods

    #region Public Methods

    /// <summary>
    /// Creates a package of the specified <paramref name="project"/>.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="flstudioFolder">The FL Studio installation folder.</param>
    /// <param name="outputFolder">The output folder of the package.</param>
    /// <returns></returns>
    ProjectPackagerResult Package(Project project, string flstudioFolder, string outputFolder);

    #endregion Public Methods

    #endregion Methods
}