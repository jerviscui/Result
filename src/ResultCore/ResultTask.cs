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

    internal ResultTask(ValueTask<Result<TError>> task)
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
public class AsyncResultTaskMethodBuilder<TResult>
    where TResult : BasicError, new()
{

    #region Constants & Statics

    // 1. 创建一个 AsyncTaskMethodBuilder
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static AsyncResultTaskMethodBuilder<TResult> Create()
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        var builder = AsyncValueTaskMethodBuilder<Result<TResult>>.Create();

        return new AsyncResultTaskMethodBuilder<TResult>(builder);
    }

    #endregion

    private readonly AsyncValueTaskMethodBuilder<Result<TResult>> _builder;

    public AsyncResultTaskMethodBuilder(AsyncValueTaskMethodBuilder<Result<TResult>> builder)
    {
        _builder = builder;
    }

    #region Properties

    // 5. 作为 async 方法的返回值
    public ResultTask<TResult> Task => new(_builder.Task);

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
#pragma warning disable IDE0060 // Remove unused parameter
    public void SetStateMachine(IAsyncStateMachine stateMachine)
#pragma warning restore IDE0060 // Remove unused parameter
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

///// <summary>Represents a builder for asynchronous methods that returns a <see cref="ValueTask{TResult}"/>.</summary>
///// <typeparam name="TResult">The type of the result.</typeparam>
//[StructLayout(LayoutKind.Auto)]
//public struct AsyncResultTaskMethodBuilder<TResult>
//    where TResult : BasicError, new()
//{
//    /// <summary>The wrapped task.  If the operation completed synchronously and successfully, this will be a sentinel object compared by reference identity.</summary>
//    private Task<Result<TResult>>? m_task; // Debugger depends on the exact name of this field.
//    /// <summary>The result for this builder if it's completed synchronously, in which case <see cref="m_task"/> will be <see cref="s_syncSuccessSentinel"/>.</summary>
//    private Result<TResult> _result;

//    #region Constants & Statics

//    /// <summary>Sentinel object used to indicate that the builder completed synchronously and successfully.</summary>
//    /// <remarks>
//    /// To avoid memory safety issues even in the face of invalid race conditions, we ensure that the type of this object
//    /// is valid for the mode in which we're operating.  As such, it's cached on the generic builder per TResult
//    /// rather than having one sentinel instance for all types.
//    /// </remarks>
//    internal static readonly Task<Result<TResult>> s_syncSuccessSentinel = System.Threading.Tasks.Task
//        .FromResult<Result<TResult>>(default);

//    // 1. Static Create method.
//    /// <summary>Creates an instance of the <see cref="AsyncValueTaskMethodBuilder{TResult}"/> struct.</summary>
//    /// <returns>The initialized instance.</returns>
//    public static AsyncValueTaskMethodBuilder<TResult> Create()
//    {
//        return default;
//    }

//    #endregion

//    #region Properties

//    // 2. TaskLike Task property.
//    /// <summary>Gets the value task for this builder.</summary>
//    public ValueTask<Result<TResult>> Task
//    {
//        get
//        {
//            if (m_task == s_syncSuccessSentinel)
//            {
//                return new ValueTask<Result<TResult>>(_result);
//            }

//            // With normal access paterns, m_task should always be non-null here: the async method should have
//            // either completed synchronously, in which case SetResult would have set m_task to a non-null object,
//            // or it should be completing asynchronously, in which case AwaitUnsafeOnCompleted would have similarly
//            // initialized m_task to a state machine object.  However, if the type is used manually (not via
//            // compiler-generated code) and accesses Task directly, we force it to be initialized.  Things will then
//            // "work" but in a degraded mode, as we don't know the TStateMachine type here, and thus we use a
//            // normal task object instead.

//            Task<Result<TResult>>? task = m_task ??= new Task<Result<TResult>>(); // base task used rather than box to minimize size when used as manual promise
//            return new ValueTask<Result<TResult>>(task);
//        }
//    }

//    #endregion

//    #region Methods

//    // 5. AwaitOnCompleted
//    /// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes.</summary>
//    /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
//    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
//    /// <param name="awaiter">the awaiter</param>
//    /// <param name="stateMachine">The state machine.</param>
//    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
//        where TAwaiter : INotifyCompletion
//        where TStateMachine : IAsyncStateMachine
//    {
//        AsyncTaskMethodBuilder<TResult>.AwaitOnCompleted(ref awaiter, ref stateMachine, ref m_task);
//    }

//    // 6. AwaitUnsafeOnCompleted
//    /// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes.</summary>
//    /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
//    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
//    /// <param name="awaiter">the awaiter</param>
//    /// <param name="stateMachine">The state machine.</param>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
//        where TAwaiter : ICriticalNotifyCompletion
//        where TStateMachine : IAsyncStateMachine
//    {
//        AsyncTaskMethodBuilder<TResult>.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine, ref m_task);
//    }

//    // 3. SetException
//    /// <summary>Marks the value task as failed and binds the specified exception to the value task.</summary>
//    /// <param name="exception">The exception to bind to the value task.</param>
//    public void SetException(Exception exception)
//    {
//        AsyncTaskMethodBuilder<TResult>.SetException(exception, ref m_task);
//    }

//    // 4. SetResult
//    /// <summary>Marks the value task as successfully completed.</summary>
//    /// <param name="result">The result to use to complete the value task.</param>
//    public void SetResult(TResult result)
//    {
//        if (m_task is null)
//        {
//            _result = result;
//            m_task = s_syncSuccessSentinel;
//        }
//        else
//        {
//            AsyncTaskMethodBuilder<TResult>.SetExistingTaskResult(m_task, result);
//        }
//    }

//    // 8. SetStateMachine
//    /// <summary>Associates the builder with the specified state machine.</summary>
//    /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
//    public void SetStateMachine(IAsyncStateMachine stateMachine)
//    {
//        AsyncMethodBuilderCore.SetStateMachine(stateMachine, task: null);
//    }

//    // 7. Start
//    /// <summary>Begins running the builder with the associated state machine.</summary>
//    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
//    /// <param name="stateMachine">The state machine instance, passed by reference.</param>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public void Start<TStateMachine>(ref TStateMachine stateMachine)
//        where TStateMachine : IAsyncStateMachine
//    {
//        AsyncMethodBuilderCore.Start(ref stateMachine);
//    }

//    #endregion

//    ///// <summary>
//    ///// Gets an object that may be used to uniquely identify this builder to the debugger.
//    ///// </summary>
//    ///// <remarks>
//    ///// This property lazily instantiates the ID in a non-thread-safe manner.
//    ///// It must only be used by the debugger and tracing purposes, and only in a single-threaded manner
//    ///// when no other threads are in the middle of accessing this or other members that lazily initialize the box.
//    ///// </remarks>
//    //internal object ObjectIdForDebugger => m_task ??= AsyncTaskMethodBuilder<TResult>.CreateWeaklyTypedStateMachineBox();
//}
