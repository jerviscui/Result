using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ResultCore;

/// <summary>
/// Wrap the error or return value.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
public record Result<TData, TError> where TError : Error, new()
{
    private Result() : this(new TError())
    {
    }

    public Result(TData data)
    {
        Data = data;
    }

    public Result(TError error)
    {
        Error = error;
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public TData? Data { get; init; }

    /// <summary>
    /// Gets the error.
    /// </summary>
    public TError? Error { get; init; }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is error, and error must not be null; otherwise <c>false</c>.
    /// </returns>
    public bool IsError([NotNullWhen(true)] out TError? error)
    {
        return IsError(out error, out _);
    }

    /// <summary>
    /// Determines whether the specified data is error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="data">The data.</param>
    /// <returns>
    ///   <c>true</c> if result is error; otherwise <c>false</c> and the data must not be null.
    /// </returns>
    public bool IsError([NotNullWhen(true)] out TError? error, [NotNullWhen(false)] out TData? data)
    {
        data = default;
        error = default;

        if (Error is not null)
        {
            error = Error;
            return true;
        }

        Debug.Assert(Data != null, nameof(Data) + " != null");
        data = Data;
        return false;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException">Error is null.</exception>
    public TError UnwrapError()
    {
        if (Error is null)
        {
            throw new NullReferenceException("Error is null.");
        }

        return Error;
    }

    internal TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, nameof(Error) + " != null");
        return Error;
    }

    public static implicit operator Result<TData, TError>(Result<TError> result)
    {
        return new Result<TData, TError>().From(result);
    }

    public static implicit operator Result<TData, TError>(TData data)
    {
        return new Result<TData, TError>(data);
    }

    public static implicit operator Result<TData, TError>(TError error)
    {
        return new Result<TData, TError>(error);
    }
}

/// <summary>
/// Wrap the error or return void.
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
public record Result<TError> where TError : Error
{
    /// <summary>
    /// No errors, just return.
    /// </summary>
    private static readonly Result<TError> Ok = new();

    private Result()
    {
    }

    public Result(TError error)
    {
        Error = error;
    }

    /// <summary>
    /// Gets the error.
    /// </summary>
    public TError? Error { get; init; }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    public bool IsError() => Error is not null;

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is error, and error must not be null; otherwise, <c>false</c>.
    /// </returns>
    public bool IsError([NotNullWhen(true)] out TError? error)
    {
        error = default;

        if (Error is not null)
        {
            error = Error;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException">Error is null.</exception>
    public TError UnwrapError()
    {
        if (Error is null)
        {
            throw new NullReferenceException("Error is null.");
        }

        return Error;
    }

    internal TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, nameof(Error) + " != null");
        return Error;
    }

    public static implicit operator Result<TError>(Result _)
    {
        return Ok;
    }

    public static implicit operator Result<TError>(TError error)
    {
        return new Result<TError>(error);
    }
}

public enum Result
{
    /// <summary>
    /// No errors, just return.
    /// </summary>
    Ok
}
