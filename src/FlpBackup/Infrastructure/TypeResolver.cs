using Spectre.Console.Cli;

namespace FlpBackup.Infrastructure;

/// <summary>
/// A service to resolve types from an <see cref="IServiceProvider"/>.
/// </summary>
public sealed class TypeResolver : ITypeResolver, IDisposable
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="TypeResolver"/>.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    #endregion Constructors

    #region Fields

    private readonly IServiceProvider _provider;

    #endregion Fields

    #region Methods

    #region Public Methods

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_provider is IDisposable disposable)
            disposable.Dispose();
    }

    /// <inheritdoc/>
    public object? Resolve(Type? type)
    {
        if (type == null)
            return null;

        return _provider.GetService(type);
    }

    #endregion Public Methods

    #endregion Methods
}