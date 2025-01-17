using System;

namespace ResultCore;

public static class ResultExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Change Error froms the specified other result.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TFError">The type of the f error.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public static Result<TData, TError> From<TData, TError, TFError>(
        this Result<TData, TError> result,
        Result<TFError> other)
        where TFError : BasicError
        where TError : BasicError, new()
    {
        var from = other.UnwrapError();

        if (result.IsError(out var error))
        {
            _ = error.From(from);
        }

        return result;
    }

    /// <summary>
    /// Change Error froms the specified other result.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TFError">The type of the f error.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">other is Null.</exception>
    public static Result<TError> From<TError, TFError>(this Result<TError> result, Result<TFError> other)
        where TFError : BasicError
        where TError : BasicError
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
