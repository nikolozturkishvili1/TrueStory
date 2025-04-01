namespace TrueStory.Common.Exceptions;

/// <summary>
/// Exception that is thrown when an entity already exists in the system.
/// </summary>
public class AlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public AlreadyExistsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class with a specified error message and an inner exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}