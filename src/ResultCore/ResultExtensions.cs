using System.Runtime.CompilerServices;

namespace ResultCore;

public static class ResultExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">No have Error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly TError GetErrorRef<TData, TError>(this in Result<TData, TError> result)
        where TData : class?
        where TError : struct
    {
        if (!result._hasError)
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new InvalidOperationException("No have Error.");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        return ref result._error;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">No have Error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly TError GetErrorRef<TError>(this in Result<TError> result)
        where TError : struct
    {
        if (!result._hasError)
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new InvalidOperationException("No have Error.");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        return ref result._error;
    }

    /// <summary>
    /// Sequences the specified results.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="results">The results.</param>
    public static Result<IEnumerable<TData>, TError> Sequence<TData, TError>(
        this IEnumerable<Result<TData, TError>> results)
        where TData : class?
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
        where TData : class?
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
