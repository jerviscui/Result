using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace ResultCore.Tests;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.LlcMisses, HardwareCounter.LlcReference)]
public class Benchmarks
{

    #region Constants & Statics

    private const int Count = 1_000_000;

    private static Result<MyClass, BaseError> Return_1()
    {
        var result = new Result<MyClass, BaseError>(BaseErrorCode.NotFound);
        return result;
    }

    private static Result<MyClass, BaseError> Return_3()
    {
        BaseError result = BaseErrorCode.NotFound;
        return result;
    }

    private static ResultRef<MyClass, RefError> ReturnRef_1()
    {
        var result = new ResultRef<MyClass, RefError>(BaseErrorCode.NotFound);
        return result;
    }

    private static ResultRef<MyClass, RefError> ReturnRef_2()
    {
        var error = new RefError(BaseErrorCode.NotFound);
        return new ResultRef<MyClass, RefError>(error);
    }

    private static ResultRef<MyClass, RefError> ReturnRef_3()
    {
        RefError result = BaseErrorCode.NotFound;
        return result;
    }

    public static Result<MyClass, BaseError> Return_2()
    {
        var result = BaseError.Result(BaseErrorCode.NotFound);
        return result;
    }

    public static Result<MyClass, FileError> Return_FileError()
    {
        var result = FileError.Result(FileErrorCode.A);
        return result;
    }

    #endregion

    #region Methods

    [Benchmark]
    public void Ref1()
    {
        var list = new List<ResultRef<MyClass, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_1();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark]
    public void Ref2()
    {
        var list = new List<ResultRef<MyClass, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_2();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark]
    public void Ref3()
    {
        var list = new List<ResultRef<MyClass, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_3();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark(Baseline = true)]
    public void Struct1()
    {
        var list = new List<Result<MyClass, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_1();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark]
    public void Struct2()
    {
        var list = new List<Result<MyClass, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_2();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark]
    public void Struct3()
    {
        var list = new List<Result<MyClass, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_3();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            list.Add(a);
        }
    }

    [Benchmark]
    public void StructSmall()
    {
        var list = new List<Result<MyClass, FileError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_FileError();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            list.Add(a);
        }
    }

    #endregion

}
