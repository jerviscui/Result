using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ResultCore;

/// <summary>
/// Wrap the error or return value.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
public record Result<TData, TError>
    where TError : BasicError, new()
{

    #region Constants & Statics

    /// <inheritdoc/>
    public static implicit operator Result<TData, TError>(Result<TError> result) =>
                                        new Result<TData, TError>().From(result);

    /// <inheritdoc/>
    public static implicit operator Result<TData, TError>(TData data) => new(data);

    /// <inheritdoc/>
    public static implicit operator Result<TData, TError>(TError error) => new(error);

    #endregion

    private Result() : this(new TError())
    {
    }

    /// <inheritdoc/>
    public Result(TData data)
    {
        Data = data;
    }

    /// <inheritdoc/>
    public Result(TError error)
    {
        Error = error;
    }

    #region Properties

    /// <summary>
    /// Gets the data.
    /// </summary>
    public TData? Data { get; init; }

    /// <summary>
    /// Gets the error.
    /// </summary>
    public TError? Error { get; init; }

    #endregion

    #region Methods

    internal TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, $"{nameof(Error)} != null");
        return Error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise <c>false</c>.
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
    /// <c>true</c> if result is error; otherwise <c>false</c> and the data must not be null.
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

        Debug.Assert(Data != null, $"{nameof(Data)} != null");
        data = Data;
        return false;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">Error is null.</exception>
    public TError UnwrapError()
    {
        if (Error is null)
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new NullReferenceException("Error is null.");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        return Error;
    }

    #endregion

}

/// <summary>
/// Wrap the error or return void.
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
public record Result<TError>
    where TError : BasicError
{

    #region Constants & Statics

    /// <summary>
    /// No errors, just return.
    /// </summary>
    private static readonly Result<TError> Ok = new();

    /// <inheritdoc/>
    public static implicit operator Result<TError>(Result _) => Ok;

    /// <inheritdoc/>
    public static implicit operator Result<TError>(TError error) => new(error);

    #endregion

    private Result()
    {
    }

    /// <inheritdoc/>
    public Result(TError error)
    {
        Error = error;
    }

    #region Properties

    /// <summary>
    /// Gets the error.
    /// </summary>
    public TError? Error { get; init; }

    #endregion

    #region Methods

    internal TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, $"{nameof(Error)} != null");
        return Error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    public bool IsError()
    {
        return Error is not null;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise, <c>false</c>.
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
    /// <exception cref="NullReferenceException">Error is null.</exception>
    public TError UnwrapError()
    {
        if (Error is null)
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new NullReferenceException("Error is null.");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        return Error;
    }

    #endregion

}

/// <inheritdoc/>
public enum Result
{
    /// <summary>
    /// No errors, just return.
    /// </summary>
    Ok
}
