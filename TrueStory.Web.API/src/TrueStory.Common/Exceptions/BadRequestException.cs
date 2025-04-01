namespace TrueStory.Common.Exceptions;

/// <summary>
/// Exception that is thrown when a bad request is made by the client.
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public BadRequestException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class 
    /// with a specified error message and an inner exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}