namespace TrueStory.Common.Exceptions;

/// <summary>
/// Exception that is thrown when an argument provided to a method is invalid.
/// </summary>
public class ArgumentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public ArgumentException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with a specified error message and an inner exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public ArgumentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}