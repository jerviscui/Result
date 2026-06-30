using MemoryPack;
using MessagePack;
using Orleans;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace ResultCore;

/// <summary>
/// Wrap the error or return value.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
/// <typeparam name="TError">The type of the error.</typeparam>
[MemoryPackable(SerializeLayout.Explicit)]
[GenerateSerializer]
[Alias("ResultCore.Result`2")]
[Immutable]
[MessagePackObject(AllowPrivate = true)]
[JsonConverter(typeof(ResultConverterFactory))]
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Use field in struct")]
[DebuggerDisplay("{DisplayText}")]
public readonly partial record struct Result<TData, TError>
    where TData : class?
    where TError : struct
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    [MemoryPackOrder(2)]
    [Id(2)]
    [Key(2)]
    public readonly TData? Data;

    /// <summary>
    /// Gets the error.
    /// </summary>
    [MemoryPackInclude]
    [MemoryPackOrder(0)]
    [Id(0)]
    [Key(0)]
    internal readonly TError error;

    [MemoryPackInclude]
    [MemoryPackOrder(1)]
    [Id(1)]
    [Key(1)]
    internal readonly bool? hasError;

    [MemoryPackConstructor]
    [SerializationConstructor]
    internal Result(TError error, bool? hasError, TData? data)
    {
        // just use for Serialize
        this.error = error;
        this.hasError = hasError;
        Data = data;
    }

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
        hasError = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with error.
    /// The result is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(in TError error)
    {
        this.error = error;
        hasError = true;
    }

    #region Properties

    [MemoryPackIgnore]
    [IgnoreMember]
    [JsonIgnore]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string DisplayText =>
                                $"HasError = {hasError}, {((hasError ?? false) ? $"Error = {error}" : $"Data = {Data}")}";

    #endregion

    #region Methods

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly TError GetErrorRefUnsafe()
    {
        Debug.Assert(IsError(), $"{nameof(hasError)} is true");
        return ref error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out bool isError, out TError error, out TData? data)
    {
        isError = IsError();
        if (isError)
        {
            error = this.error;
            data = null;
        }
        else
        {
            error = default;
            data = Data;
        }
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="NotInitializeException">Result is default</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [MemberNotNullWhen(false, nameof(Data))]
    public readonly bool IsError()
    {
        if (hasError is null)
        {
            throw new NotInitializeException("Result is default");
        }

        return hasError.Value;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <param name="error">output the <typeparamref name="TError"/> </param>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "Standard Try-Parse pattern implementation")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        if (IsError())
        {
            error = this.error;
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
        Justification = "Standard Try-Parse pattern implementation")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error, [NotNullWhen(false)] out TData? data)
    {
        if (IsError())
        {
            error = this.error;
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
                                        result.IsError()
                                            ? new(in result.GetErrorRefUnsafe())
                                            : new(default!);

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
[MemoryPackable(SerializeLayout.Explicit)]
[GenerateSerializer]
[Alias("ResultCore.Result`1")]
[Immutable]
[MessagePackObject(AllowPrivate = true)]
[JsonConverter(typeof(ResultConverterFactory))]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Use field in struct")]
[DebuggerDisplay("{DisplayText}")]
public readonly partial record struct Result<TError>
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
    [MemoryPackInclude]
    [MemoryPackOrder(0)]
    [Id(0)]
    [Key(0)]
    internal readonly TError error;

    [MemoryPackInclude]
    [MemoryPackOrder(1)]
    [Id(1)]
    [Key(1)]
    internal readonly bool? hasError;

    private Result(bool _)
    {
        // just use for Ok
        hasError = false;
    }

    [MemoryPackConstructor]
    [SerializationConstructor]
    internal Result(TError error, bool? hasError)
    {
        // just use for Serialize
        this.error = error;
        this.hasError = hasError;
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
        this.error = error;
        hasError = true;
    }

    #region Properties

    [MemoryPackIgnore]
    [IgnoreMember]
    [JsonIgnore]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string DisplayText =>
                                (hasError ?? false)
                                    ? $"HasError = {hasError}, Error = {error}"
                                    : $"HasError = {hasError}";

    #endregion

    #region Methods

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly TError GetErrorRefUnsafe()
    {
        Debug.Assert(IsError(), $"{nameof(hasError)} is true");
        return ref error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="NotInitializeException">Result is default</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError()
    {
        if (hasError is null)
        {
            throw new NotInitializeException("Result is default");
        }

        return hasError.Value;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <param name="error">output the <typeparamref name="TError"/> </param>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "Standard Try-Parse pattern implementation")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        if (IsError())
        {
            error = this.error;
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
    Ok = 0
}
