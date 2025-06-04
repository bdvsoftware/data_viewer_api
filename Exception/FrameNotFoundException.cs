namespace DataViewerApi.Exception;
using System;


public class FrameNotFoundException : Exception
{
    public FrameNotFoundException() : base("Frame not found.") { }
    public FrameNotFoundException(string message) : base(message) { }

    public FrameNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}