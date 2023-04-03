using FlpBackup.Commands;
using FlpBackup.Infrastructure;
using FlpBackup.Parser;
using FlpBackup.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Spectre;
using Spectre.Console.Cli;

var logger = new LoggerConfiguration()
    .WriteTo.Spectre()
    .WriteTo.File($"log-{DateTime.Now:yyMMddHHmmss}.txt")
    .MinimumLevel.Information()
    .CreateLogger();

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<ILogger>(logger);
serviceCollection.AddTransient<IProjectParser, ProjectParser>();
serviceCollection.AddTransient<ISampleLocator, SampleLocator>();
serviceCollection.AddTransient<IProjectPackager, ProjectPackager>();

var typeRegistrar = new TypeRegistrar(serviceCollection);
var commandApp = new CommandApp(typeRegistrar);

commandApp.Configure(config =>
{
    config.AddCommand<PackageCommand>("package");
});

return commandApp.Run(args);