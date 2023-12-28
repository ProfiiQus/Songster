namespace Songster.Lib.Exceptions;

/// <summary>
/// Exception thrown when the song queue is empty.
/// </summary>
public class EmptyQueueException : Exception
{
    /// <summary>
    /// Creates a new instance of the empty queue exception.
    /// </summary>
    public EmptyQueueException() : base("The song queue is empty.") { }
}