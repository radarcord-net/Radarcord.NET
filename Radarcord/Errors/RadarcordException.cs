using System;

namespace Radarcord.Errors;

/// <summary>
/// Something catastrophic happened while using Radarcord.
/// </summary>
public class RadarcordException : Exception
{
    internal RadarcordException() { }
    internal RadarcordException(string message) : base(message) { }
    internal RadarcordException(string message, Exception innerException) : base(message, innerException) { }
}
