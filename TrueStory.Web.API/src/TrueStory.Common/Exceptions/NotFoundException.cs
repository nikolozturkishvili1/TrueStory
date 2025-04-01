namespace TrueStory.Common.Exceptions;

/// <summary>
/// Exception that is thrown when a requested entity or resource is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public NotFoundException(string message) : base(message)
    {
    }
}