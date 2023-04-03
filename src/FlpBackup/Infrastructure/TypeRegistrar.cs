using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace FlpBackup.Infrastructure;

/// <summary>
/// A service to register types with an <see cref="IServiceCollection"/>.
/// </summary>
public sealed class TypeRegistrar : ITypeRegistrar
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="TypeRegistrar"/>.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public TypeRegistrar(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
    }

    #endregion Constructors

    #region Fields

    private readonly IServiceCollection _serviceCollection;

    #endregion Fields

    #region Methods

    #region Public Methods

    /// <inheritdoc/>
    public ITypeResolver Build() => new TypeResolver(_serviceCollection.BuildServiceProvider());

    /// <inheritdoc/>
    public void Register(Type service, Type implementation) => _serviceCollection.AddSingleton(service, implementation);

    /// <inheritdoc/>
    public void RegisterInstance(Type service, object implementation) => _serviceCollection.AddSingleton(service, implementation);

    /// <inheritdoc/>
    public void RegisterLazy(Type service, Func<object> factory)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        _serviceCollection.AddSingleton(service, _ => factory());
    }

    #endregion Public Methods

    #endregion Methods
}