using Spectre.Console.Cli;
using System.ComponentModel;

namespace FlpBackup.Commands;

/// <summary>
/// Settings of <see cref="PackageCommand"/>.
/// </summary>
public sealed class PackageCommandSettings : CommandSettings
{
    #region Properties

    /// <summary>
    /// Determines whether to create a folder with the same name as the project file's parent folder when storing package files.
    /// </summary>
    [Description("Create project parent folder.")]
    [CommandOption("-p")]
    public bool? CreateParentFolder { get; init; }

    /// <summary>
    /// Gets the FL Studio installation folder used to resolve factory samples.
    /// </summary>
    [Description("Path of the FL Studio folder.")]
    [CommandArgument(2, "<flstudio-folder>")]
    public string FLStudioFolder { get; init; } = null!;

    /// <summary>
    /// Gets the folder used to store package files.
    /// </summary>
    [Description("Path of the output folder. If not specified packages will be created in the folder of their project files.")]
    [CommandOption("-o|--output-folder")]
    public string? OutputFolder { get; init; }

    /// <summary>
    /// Gets the folder containing the project files. The folder is searched recursively.
    /// </summary>
    [Description("Path of the folder containing the project files.")]
    [CommandArgument(0, "<project-folder>")]
    public string ProjectFolder { get; init; } = null!;

    /// <summary>
    /// Determines whether to display a summary after completion.
    /// </summary>
    [Description("Show summary.")]
    [CommandOption("-s")]
    public bool? ShowSummary { get; init; }

    #endregion Properties
}