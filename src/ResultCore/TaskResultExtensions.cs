using System.Diagnostics.CodeAnalysis;

namespace ResultCore;

public static class TaskResultExtensions
{

    #region Constants & Statics

    /// <summary>
    /// Use Task for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static Task<Result<TError>> AsTask<TError>(this Result result)
        where TError : struct
    {
        return (Result<TError>)result;
    }

    /// <summary>
    /// Use Task for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static Task<Result<TData, TError>> AsTask<TData, TError>(this in Result<TError> result)
        where TData : class?
        where TError : struct
    {
        return (Result<TData, TError>)result;
    }

    /// <summary>
    /// Use Task for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static Task<Result<TData, TError>> AsTask<TData, TError>(this TData data)
        where TData : class?
        where TError : struct
    {
        return (Result<TData, TError>)data;
    }

    /// <summary>
    /// Use ValueTask for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static ValueTask<Result<TError>> AsValueTask<TError>(this Result result)
        where TError : struct
    {
        return (Result<TError>)result;
    }

    /// <summary>
    /// Use ValueTask for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static ValueTask<Result<TData, TError>> AsValueTask<TData, TError>(this in Result<TError> result)
        where TData : class?
        where TError : struct
    {
        return (Result<TData, TError>)result;
    }

    /// <summary>
    /// Use ValueTask for wrapping. This is used for asynchronous methods to return values.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    [SuppressMessage(
        "Minor Code Smell",
        "S4261:Methods should be named according to their synchronicities",
        Justification = "<Pending>")]
    public static ValueTask<Result<TData, TError>> AsValueTask<TData, TError>(this TData data)
        where TData : class?
        where TError : struct
    {
        return (Result<TData, TError>)data;
    }

    #endregion

}
