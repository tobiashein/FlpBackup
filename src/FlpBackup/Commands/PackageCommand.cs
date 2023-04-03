using FlpBackup.Parser;
using FlpBackup.Services;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FlpBackup.Commands;

/// <summary>
/// A command to package project files.
/// </summary>
public sealed class PackageCommand : Command<PackageCommandSettings>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="PackageCommand"/>.
    /// </summary>
    /// <param name="projectParser">The project parser.</param>
    /// <param name="projectPackager">The project packager.</param>
    /// <param name="logger">The logger.</param>
    public PackageCommand(IProjectParser projectParser, IProjectPackager projectPackager, ILogger logger)
    {
        _projectParser = projectParser;
        _projectPackager = projectPackager;
        _logger = logger;
    }

    #endregion Constructors

    #region Fields

    private readonly ILogger _logger;
    private readonly IProjectPackager _projectPackager;
    private readonly IProjectParser _projectParser;

    #endregion Fields

    #region Methods

    #region Private Static Methods

    private static string GetPackageOutputFolder(Project project, PackageCommandSettings settings)
    {
        if (settings.OutputFolder is null)
            return Path.GetDirectoryName(project.Filename)!;

        if (settings.CreateParentFolder is true)
            return Path.Combine(settings.OutputFolder, GetParentFolderFromFilename(project.Filename));

        return settings.OutputFolder;
    }

    private static string GetParentFolderFromFilename(string filename)
    {
        var parentFolder = Path.GetDirectoryName(filename);
        var parentFolderName = Path.GetFileName(parentFolder);

        return parentFolderName!;
    }

    private static void ShowSummary(ConcurrentBag<ProjectPackagerResult> projectPackagerResults)
    {
        foreach (var projectPackagerResult in projectPackagerResults)
        {
            var projectNode = new Tree($"Project [blue]{projectPackagerResult.ProjectFilename}[/]");
            projectNode.AddNode($"Version: [teal]{projectPackagerResult.ProjectVersion}[/]");
            projectNode.AddNode($"Package: [teal]{projectPackagerResult.PackageFilename}[/]");

            if (projectPackagerResult.IsError)
            {
                projectNode.AddNode($"[red]{projectPackagerResult.ErrorMessage}[/]");
            }
            else
            {
                var foundSamplesNode = projectNode.AddNode($"Found samples: [teal]{projectPackagerResult.FoundSamples.Count}[/]");
                foreach (var sample in projectPackagerResult.FoundSamples)
                    foundSamplesNode.AddNode($"[green]{sample}[/]");

                var missingSamplesNode = projectNode.AddNode($"Missing samples: [teal]{projectPackagerResult.MissingSamples.Count}[/]");
                foreach (var missingSample in projectPackagerResult.MissingSamples)
                    missingSamplesNode.AddNode($"[yellow]{missingSample}[/]");
            }

            AnsiConsole.Write(new Padder(projectNode));
        }
    }

    #endregion Private Static Methods

    #region Public Methods

    /// <inheritdoc/>
    public override int Execute([NotNull] CommandContext context, [NotNull] PackageCommandSettings settings)
    {
        var sw = Stopwatch.StartNew();

        var projectFiles = new DirectoryInfo(settings.ProjectFolder)
            .GetFiles("*.flp", SearchOption.AllDirectories);
        var projectPackagerResults = new ConcurrentBag<ProjectPackagerResult>();

        AnsiConsole.Progress()
            .HideCompleted(true)
            .AutoClear(true)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
            .Start(ctx =>
            {
                var task = ctx.AddTask("Creating packages", new ProgressTaskSettings
                {
                    MaxValue = projectFiles.Length,
                });

                while (!ctx.IsFinished)
                {
                    Parallel.ForEach(projectFiles, projectFile =>
                    {
                        var project = _projectParser.Parse(projectFile.FullName);
                        var packageOutputFolder = GetPackageOutputFolder(project, settings);
                        var projectPackagerResult = _projectPackager.Package(project, settings.FLStudioFolder, packageOutputFolder);

                        projectPackagerResults.Add(projectPackagerResult);

                        task.Increment(1);
                    });
                }
            });

        if (settings.ShowSummary is true)
            ShowSummary(projectPackagerResults);

        _logger.Information("Finished in {Time}.", TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).ToString(@"hh\:mm\:ss"));

        return 0;
    }

    #endregion Public Methods

    #endregion Methods
}