using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResultCore;

/// <summary>
/// ResultExtensions
/// </summary>
public static class ResultExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Sequences the specified results.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="results">The results.</param>
    public static Result<IEnumerable<TData>, TError> Sequence<TData, TError>(
        this IEnumerable<Result<TData, TError>> results)
        where TError : struct
    {
        var list = new List<TData>();

        foreach (var item in results)
        {
            if (item.IsError(out var error, out var data))
            {
                return error.Value;
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
        this IAsyncEnumerable<Result<TData, TError>> results,
        CancellationToken cancellationToken = default)
        where TError : struct
    {
        var list = new List<TData>();

        await foreach (var item in results.WithCancellation(cancellationToken))
        {
            if (item.IsError(out var error, out var data))
            {
                return error.Value;
            }

            list.Add(data);
        }

        return list;
    }

    #endregion

}
