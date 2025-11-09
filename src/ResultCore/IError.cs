using System;

namespace ResultCore;

public interface IError<TError, TCode>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// Create a default result.
    /// </summary>
    public static abstract Result<TError> Result();

    /// <summary>
    /// Create a result with the specified code, reason and exception.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="reason">The reason.</param>
    /// <param name="exception">The exception.</param>
    public static abstract Result<TError> Result(TCode code, string? reason = null, Exception? exception = null);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public TCode Code { get; }

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; }

    #endregion

}
