using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ResultCore;

/// <summary>
/// Wrap the error or return value.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct Result<TData, TError>
    where TError : struct
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    public readonly TData? Data;

    /// <summary>
    /// Gets the error.
    /// </summary>
    public readonly TError? Error;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with default <typeparamref name="TError"/>.
    /// </summary>
    public Result() : this(new TError())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with data.
    /// The result is successful.
    /// </summary>
    /// <param name="data">The data.</param>
    public Result(TData data)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with error.
    /// The result is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(TError error)
    {
        Error = error;
    }

    #region Methods

    internal readonly TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, $"{nameof(Error)} != null");
        return Error.Value;
    }

    public void Deconstruct(out TData? data, out TError? error)
    {
        data = Data;
        error = Error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "<Pending>")]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
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
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "<Pending>")]
    public readonly bool IsError([NotNullWhen(true)] out TError? error, [NotNullWhen(false)] out TData? data)
    {
        data = default;
        error = null;

        if (Error.HasValue)
        {
            error = Error;
            return true;
        }

#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
        Debug.Assert(Data != null, $"{nameof(Data)} != null");
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"
        data = Data;
        return false;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Error is null.</exception>
    public readonly TError UnwrapError()
    {
        return Error!.Value!;
    }

    #endregion

    public static implicit operator Result<TData, TError>(Result<TError> result) => new(result.UnwrapError());

    public static implicit operator Result<TData, TError>(TData data) => new(data);

    public static implicit operator Result<TData, TError>(TError error) => new(error);
}

/// <summary>
/// Wrap the error or return void.
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct Result<TError>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// No errors, just return.
    /// </summary>
    private static readonly Result<TError> Ok = new(null);

    #endregion

    /// <summary>
    /// Gets the error.
    /// </summary>
    public readonly TError? Error;

#pragma warning disable IDE0060 // Remove unused parameter
    private Result(TError? error)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // just use for Ok
        Error = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> with default <typeparamref name="TError"/>.
    /// </summary>
    public Result() : this(new TError())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with error.
    /// The result is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(TError error)
    {
        Error = error;
    }

    #region Methods

    internal readonly TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(Error != null, $"{nameof(Error)} != null");
        return Error.Value;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    public readonly bool IsError()
    {
        return Error.HasValue;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "<Pending>")]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        error = null;

        if (Error.HasValue)
        {
            error = Error;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Unwraps the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Error is null.</exception>
    public readonly TError UnwrapError()
    {
        return Error!.Value!;
    }

    #endregion

    public static implicit operator Result<TError>(Result _) => Ok;

    public static implicit operator Result<TError>(TError error) => new(error);
}

/// <inheritdoc/>
public enum Result
{
    /// <summary>
    /// No errors, just return.
    /// </summary>
    Ok
}
