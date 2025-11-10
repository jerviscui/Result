using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ResultCore.Tests;

[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct Result2<TError>
    where TError : struct
{
    #region Constants & Statics

    /// <summary>
    /// No errors, just return.
    /// </summary>
    private static readonly Result2<TError> Ok = new(default, false);

    #endregion

    /// <summary>
    /// Gets the error.
    /// </summary>
    public readonly TError _error;

    /// <summary>
    /// Indicates whether this Result2 contains an error.
    /// </summary>
    private readonly bool _hasError;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result2{TError}"/> without error.
    /// </summary>
    private Result2(TError error, bool hasError)
    {
        _error = error;
        _hasError = hasError;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result2{TError}"/> with default <typeparamref name="TError"/>.
    /// </summary>
    public Result2() : this(new TError(), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result2{TError}"/> with error.
    /// The Result2 is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result2(in TError error)
    {
        _error = error;
        _hasError = true;
    }

    #region Methods

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsError()
    {
        return _hasError;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <param name="error">A readonly reference to the error if it exists.</param>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "Performance optimization: avoid struct copy")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsError([NotNullWhen(true)] out TError? error)
    {
        if (_hasError)
        {
            error = _error;
            return true;
        }

        error = default;
        return false;
    }

    /// <summary>
    /// Tries to get the error as a readonly reference.
    /// </summary>
    /// <param name="error">A readonly reference to the error if it exists.</param>
    /// <returns><c>true</c> if this instance contains an error; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetError(out TError error)
    {
        error = _error;
        return _hasError;
    }

    #endregion

    public static implicit operator Result2<TError>(TError error) => new(in error);
}

public static class Extensions
{
    public static ref readonly TError RefD<TError>(this in Result2<TError> program)
        where TError : struct
    {
        return ref program._error;
    }
}

public struct SimpleError
{
    public int Code { get; set; }

    public string Code1 { get; set; } = string.Empty;

    public string Code2 { get; set; } = string.Empty;

    public string Code3 { get; set; } = string.Empty;

    public SimpleError(int code)
    {
        Code = code;
    }

    public override string ToString()
    {
        return $"Error {Code}";
    }

    public static Result2<SimpleError> Result2(int i)
    {
        //return new Result2<SimpleError>(new SimpleError());
        return new SimpleError(i);
    }
}

public static class MyClass2
{
    public static void Example1BasicUsage()
    {
        Console.WriteLine("【示例 1】基本用法\n");

        // 创建错误
        var error = new SimpleError(404);
        var Result2 = new Result2<SimpleError>(in error);//都是传引用
        var Result22 = new Result2<SimpleError>(error);//与上一行效果一致，都是传引用
        var Result23 = SimpleError.Result2(404);

        // 方式 1: IsError() + UnwrapError()
        if (Result2.IsError())
        {
            ref readonly var err = ref Result2.RefD(); //接收地址
            var error1 = Result2.RefD(); //复制值
            Console.WriteLine($"方式 1: {err}");

            //err.Code = 2; // wrong
            error1.Code = 2; // 不影响 Result2
            error.Code = 500;
            Console.WriteLine($"方式 1: {err}");
        }

        // 方式 2: IsError(out)
        if (Result2.IsError(out var err2))//可以直接引用内部字段吗
        {
            var a = err2.Value.Code;
            Console.WriteLine($"方式 2: {err2}");
        }

        // 方式 3: 直接访问属性（已知有错误时）
        ref readonly var err3 = ref Result2._error;
        Console.WriteLine($"方式 3: {err3}");

        Console.WriteLine();
    }
}
