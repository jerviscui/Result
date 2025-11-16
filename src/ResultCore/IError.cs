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
    /// Create a result with the specified code.
    /// </summary>
    public static abstract Result<TError> Result(TCode code);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public TCode Code { get; }

    #endregion

}
