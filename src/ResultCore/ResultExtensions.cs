using System.Collections.Generic;
using System.Threading.Tasks;

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
    public static Result<TData, TError> From<TData, TError>(this Result<TData, TError> result, Result<TError> other)
        where TError : BasicError, new()
    {
        if (result.IsError(out var error))
        {
            var from = other.UnwrapError();
            _ = error.From(from);
        }
        return result;
    }

    /// <summary>
    /// Sequences the specified results.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="results">The results.</param>
    public static Result<IEnumerable<TData>, TError> Sequence<TData, TError>(
        this IEnumerable<Result<TData, TError>> results)
        where TError : BasicError, new()
    {
        var list = new List<TData>();

        foreach (var item in results)
        {
            if (item.IsError(out var error, out var data))
            {
                return error;
            }

            list.Add(data);
        }

        return list;
    }

    /// <summary>
    /// Sequences the specified results.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="results">The results.</param>
    public static async Task<Result<IEnumerable<TData>, TError>> SequenceAsync<TData, TError>(
        this IAsyncEnumerable<Result<TData, TError>> results)
        where TError : BasicError, new()
    {
        var list = new List<TData>();

        await foreach (var item in results)
        {
            if (item.IsError(out var error, out var data))
            {
                return error;
            }

            list.Add(data);
        }

        return list;
    }

    #endregion

}
