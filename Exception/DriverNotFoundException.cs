namespace DataViewerApi.Exception;
using System;

public class DriverNotFoundException : Exception
{
    public DriverNotFoundException() : base("Driver not found.") { }

    public DriverNotFoundException(string message) : base(message) { }

    public DriverNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}