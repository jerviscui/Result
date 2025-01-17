namespace ResultCore;

public static class ErrorExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Change Error froms the specified other error.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TFError">The type of the f error.</typeparam>
    /// <param name="error">The error.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public static TError From<TError, TFError>(this TError error, TFError other)
        where TFError : BasicError
        where TError : BasicError
    {
        error.Code = other.Code;
        error.Reason = other.Reason;
        error.Exception = other.Exception;

        return error;
    }

    /// <summary>
    /// Use the specified error create Error.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="error">The error.</param>
    /// <returns></returns>
    public static BasicError Into<TError>(this TError error)
        where TError : BasicError
    {
        return new BasicError(error.Code, error.Reason, error.Exception);
    }

    #endregion

}
