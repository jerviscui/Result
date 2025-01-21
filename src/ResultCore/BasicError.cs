using System;

namespace ResultCore;

public abstract record BasicError(int Code, string? Reason = null, Exception? Exception = null)
{
    protected BasicError() : this((int)BasicErrorCode.Failure)
    {
    }

    #region Properties

    /// <inheritdoc/>
    public virtual int Code { get; protected internal set; } = Code;

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; internal set; } = Exception;

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; internal set; } = Reason;

    #endregion

}
