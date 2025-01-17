using System;

namespace ResultCore;

public interface IError<TError, TCode>
    where TError : BasicError, new()
    where TCode : struct
{

    #region Constants & Statics

    public static abstract Result<TError> Result();

    public static abstract Result<TError> Result(TCode code, string? reason = null, Exception? exception = null);

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public TCode Code { get; }

    #endregion

}
