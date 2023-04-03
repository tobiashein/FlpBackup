namespace FlpBackup.Parser;

/// <summary>
/// Events that control the parsing behavior of <see cref="ProjectParser"/>.
/// </summary>
public enum Event
{
    /// <summary>
    /// Indicates that the event data is stored in the next byte.
    /// </summary>
    Byte = 0,

    /// <summary>
    /// Indicates that the event data is stored in the next two bytes.
    /// </summary>
    Word = 64,

    /// <summary>
    /// Indicates that the event data is stored in the next four bytes.
    /// </summary>
    Int = 128,

    /// <summary>
    /// Indicates that the event data contains text, stored in a chunk of variable length that is determined by a 7-bit encoded integer.
    /// </summary>
    Text = 192,

    /// <summary>
    /// Indicates that the event data contains a sample file name, stored in a chunk of variable length that is determined by a 7-bit encoded integer.
    /// </summary>
    SampleFileName = Text + 4,

    /// <summary>
    /// Indicates that the event data contains the project version, stored in a chunk of variable length that is determined by a 7-bit encoded integer.
    /// </summary>
    Version = Text + 7,

    /// <summary>
    /// Indicates that the event data contains a generator name, stored in a chunk of variable length that is determined by a 7-bit encoded integer.
    /// </summary>
    GeneratorName = Text + 9,

    /// <summary>
    /// Indicates that the event data contains a plugin name, stored in a chunk of variable length that is determined by a 7-bit encoded integer.
    /// </summary>
    PluginName = Text + 11,

    /// <summary>
    /// Indicates that the event data is stored in a chunk of variable length, determined by a 7-bit encoded integer.
    /// </summary>
    Data = 210,
};