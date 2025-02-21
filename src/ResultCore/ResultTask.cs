using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ResultCore;

/// <summary>
/// Wrap the error or return void.
/// </summary>
[SuppressMessage(
    "Async/await",
    "CRR0034:An asynchronous method's name is missing an 'Async' suffix",
    Justification = "<Pending>")]
[SuppressMessage(
    "Async/await",
    "CRR0035:No CancellationToken parameter in the asynchronous method",
    Justification = "<Pending>")]
[AsyncMethodBuilder(typeof(AsyncResultTaskMethodBuilder<>))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct ResultTask<TError>
    where TError : BasicError, new()
{
    private readonly ValueTask<Result<TError>> _task;

    #region Constants & Statics

    /// <inheritdoc/>
    public static implicit operator ResultTask<TError>(Result _) => new(Result.Ok);

    /// <inheritdoc/>
    public static implicit operator ResultTask<TError>(TError error) => new(error);

    /// <inheritdoc/>
    public static implicit operator ResultTask<TError>(Result<TError> result) => new(result);

    #endregion

    internal ResultTask(ref ValueTask<Result<TError>> task)
    {
        _task = task;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> with default <typeparamref name="TError"/>.
    /// </summary>
    public ResultTask() : this(new TError())
    {
    }

    /// <inheritdoc/>
    public ResultTask(TError error) : this(new Result<TError>(error))
    {
    }

    /// <inheritdoc/>
    public ResultTask(Result<TError> result)
    {
        _task = new ValueTask<Result<TError>>(result);
    }

    /// <inheritdoc/>
    public ResultTask(Task<Result<TError>> task)
    {
        _task = new ValueTask<Result<TError>>(task);
    }

    #region Await Support

    /// <summary>Gets an awaiter for this <see cref="ValueTask{TResult}"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTaskAwaiter<Result<TError>> GetAwaiter()
    {
        return _task.GetAwaiter();
    }

    /// <summary>Configures an awaiter for this <see cref="ValueTask{TResult}"/>.</summary>
    /// <param name="continueOnCapturedContext">
    /// true to attempt to marshal the continuation back to the captured context; otherwise, false.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConfiguredValueTaskAwaitable<Result<TError>> ConfigureAwait(bool continueOnCapturedContext)
    {
        return _task.ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

    #region ValueTask

    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> object to represent this ValueTask.
    /// </summary>
    /// <remarks>
    /// It will either return the wrapped task object if one exists, or it'll
    /// manufacture a new task object to represent the result.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<Result<TError>> AsTask()
    {
        return _task.AsTask();
    }

    /// <summary>Gets a <see cref="ValueTask{TResult}"/> that may be used at any point in the future.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<Result<TError>> Preserve()
    {
        return _task.Preserve();
    }

    #endregion

}

[SuppressMessage("Members", "CRR0026:Unused member", Justification = "<Pending>")]
[StructLayout(LayoutKind.Auto)]
public struct AsyncResultTaskMethodBuilder<TResult>
    where TResult : BasicError, new()
{
    private AsyncValueTaskMethodBuilder<Result<TResult>> _builder;

    #region Constants & Statics

    // 1. 创建一个 AsyncTaskMethodBuilder
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static AsyncResultTaskMethodBuilder<TResult> Create()
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        var builder = AsyncValueTaskMethodBuilder<Result<TResult>>.Create();

        return new AsyncResultTaskMethodBuilder<TResult>(ref builder);
    }

    #endregion

    public AsyncResultTaskMethodBuilder(ref AsyncValueTaskMethodBuilder<Result<TResult>> builder)
    {
        _builder = builder;
    }

    #region Properties

    // 5. 作为 async 方法的返回值
    public ResultTask<TResult> Task
    {
        get
        {
            var task = _builder.Task;
            return new(ref task);
        }
    }

    #endregion

    #region Methods

    // 3. 将状态机的 MoveNext 方法注册为 async 方法内 await 的 Task 的回调
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _builder.AwaitOnCompleted(ref awaiter, ref stateMachine);
    }

    // 4. 同 AwaitOnCompleted，但是清空 ExecutionContext
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }

    // 7. SetException
    public void SetException(Exception exception)
    {
        _builder.SetException(exception);
    }

    // 8. SetResult
    public void SetResult(TResult result)
    {
        _builder.SetResult(result);
    }

    // 6. 绑定状态机，但编译器的编译结果不会调用
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        _builder.SetStateMachine(stateMachine);
    }

    // 2. 开始执行 AsyncTaskMethodBuilder 及其绑定的状态机
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        _builder.Start(ref stateMachine);
    }

    #endregion

}
