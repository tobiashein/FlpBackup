using System.Runtime.Serialization;

namespace FlpBackup.Parser;

/// <summary>
/// An exception thrown by <see cref="ProjectParser"/>.
/// </summary>
[Serializable]
public class ProjectParserException : Exception
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectParserException"/>.
    /// </summary>
    public ProjectParserException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectParserException"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    public ProjectParserException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectParserException"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ProjectParserException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectParserException"/>.
    /// </summary>
    /// <param name="info">Contains the data needed for object serialization and deserialization.</param>
    /// <param name="context">The streaming context.</param>
    protected ProjectParserException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    #endregion Constructors
}