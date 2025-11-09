using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ResultCore.Tests;

/// <summary>
/// Wrap the error or return void.
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct ResultRef<TData, TError>
    where TError : class?, new()
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    public readonly TData? Data;

    /// <summary>
    /// Gets the error.
    /// </summary>
    public readonly TError? Error;

    public ResultRef() : this(new TError())
    {
    }

    public ResultRef(TData data)
    {
        Data = data;
    }

    public ResultRef(TError error)
    {
        Error = error;
    }

    #region Methods

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

        if (Error is not null)
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
        ArgumentNullException.ThrowIfNull(Error);

        return Error;
    }

    #endregion

    public static implicit operator ResultRef<TData, TError>(TData data) => new(data);

    public static implicit operator ResultRef<TData, TError>(TError error) => new(error);
}
