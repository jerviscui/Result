using System;

namespace ResultCore;

public interface IError<TError, TCode> where TError : Error, new() where TCode : struct
{
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public TCode Code { get; }

    public static abstract Result<TError> Result();

    public static abstract Result<TError> Result(TCode code, string? reason = null,
        Exception? exception = null);
}
