using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
    where TData : class?
    where TError : struct
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    public readonly TData? Data;

    /// <summary>
    /// Gets the error.
    /// </summary>
    internal readonly TError _error;

    internal readonly bool _hasError;

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
        _hasError = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with error.
    /// The result is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(in TError error)
    {
        _error = error;
        _hasError = true;
    }

    #region Methods

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(_hasError, $"{nameof(_hasError)} is true");
        return ref _error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out TData? data, out TError? error)
    {
        data = Data;
        error = _hasError ? _error : null;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError()
    {
        return _hasError;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        if (_hasError)
        {
            error = _error;
            return true;
        }

        error = null;
        return false;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error, [NotNullWhen(false)] out TData? data)
    {
        if (_hasError)
        {
            error = _error;
            data = null;
            return true;
        }

        error = null;
        Debug.Assert(Data != null, $"{nameof(Data)} != null");
        data = Data;
        return false;
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TData, TError>(in Result<TError> result) =>
                                        new(in result.UnwrapErrorWithoutCheck());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TData, TError>(TData data) => new(data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TData, TError>(in TError error) => new(in error);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Task<Result<TData, TError>>(Result<TData, TError> result) =>
                                        Task.FromResult(result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTask<Result<TData, TError>>(Result<TData, TError> result) =>
                                        ValueTask.FromResult(result);
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
    private static readonly Result<TError> Ok = new(true);

    #endregion

    /// <summary>
    /// Gets the error.
    /// </summary>
    internal readonly TError _error;

    internal readonly bool _hasError;

#pragma warning disable IDE0060 // Remove unused parameter
    private Result(bool isOk)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // just use for Ok
        _hasError = false;
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
    public Result(in TError error)
    {
        _error = error;
        _hasError = true;
    }

    #region Methods

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(_hasError, $"{nameof(_hasError)} is true");
        return ref _error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError()
    {
        return _hasError;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        if (_hasError)
        {
            error = _error;
            return true;
        }

        error = null;
        return false;
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TError>(Result _) => Ok;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TError>(in TError error) => new(in error);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Task<Result<TError>>(Result<TError> result) => Task.FromResult(result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTask<Result<TError>>(Result<TError> result) => ValueTask.FromResult(result);
}

public enum Result
{
    /// <summary>
    /// No errors, just for return Result.
    /// </summary>
    Ok
}
