namespace ResultCore;

public static class ErrorExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Change error froms the specified other error.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="error">The error.</param>
    /// <param name="other">The other.</param>
    internal static TError From<TError>(this TError error, TError other)
        where TError : BasicError
    {
        error.Code = other.Code;
        error.Reason = other.Reason;
        error.Exception = other.Exception;

        return error;
    }

    #endregion

}
