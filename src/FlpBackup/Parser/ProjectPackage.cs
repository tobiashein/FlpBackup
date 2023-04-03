using System.IO.Compression;

namespace FlpBackup.Parser;

/// <summary>
/// A project package.
/// </summary>
public static class ProjectPackage
{
    #region Methods

    #region Public Static Methods

    /// <summary>
    /// Creates a package of the specified <paramref name="project"/>.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="outputFolder">The output folder of the created package.</param>
    public static string Create(Project project, string? outputFolder)
    {
        var sourceFolder = Path.GetDirectoryName(project.Filename)!.Split(Path.DirectorySeparatorChar).Last();

        if (!string.IsNullOrWhiteSpace(outputFolder))
        {
            outputFolder = Path.Combine(outputFolder, sourceFolder);

            Directory.CreateDirectory(outputFolder);
        }

        var outputFilename = Path.Combine(outputFolder ?? Path.GetDirectoryName(project.Filename)!, Path.GetFileName(Path.ChangeExtension(project.Filename, ".zip")));

        using var zipFile = ZipFile.Open(outputFilename, ZipArchiveMode.Create);

        zipFile.CreateEntryFromFile(project.Filename, Path.GetFileName(project.Filename));

        foreach (var sample in project.Samples.Where(File.Exists))
            zipFile.CreateEntryFromFile(sample, Path.GetFileName(sample));

        return outputFilename;
    }

    #endregion Public Static Methods

    #endregion Methods
}