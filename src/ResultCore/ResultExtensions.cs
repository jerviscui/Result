namespace ResultCore;

/// <summary>
/// ResultExtensions
/// </summary>
public static class ResultExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Change Error froms the specified other result.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public static Result<TData, TError> From<TData, TError>(this Result<TData, TError> result, Result<TError> other)
        where TError : BasicError, new()
    {
        var from = other.UnwrapError();

        if (result.IsError(out var error))
        {
            _ = error.From(from);
        }

        return result;
    }

    #endregion

}
