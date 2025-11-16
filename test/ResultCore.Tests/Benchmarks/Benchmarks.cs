using BenchmarkDotNet.Attributes;

namespace ResultCore.Tests;

[MemoryDiagnoser]
//[DisassemblyDiagnoser(maxDepth: 10, exportCombinedDisassemblyReport: true)]
//[InliningDiagnoser(logFailuresOnly: false, allowedNamespaces: new[] { "ResultCore.Tests" })]
//[HardwareCounters(HardwareCounter.LlcMisses, HardwareCounter.LlcReference)]
public class Benchmarks
{

    #region Constants & Statics

    private const int Count = 1_000_000;

    private static Result<MyData, BaseError> Return_1()
    {
        var result = new Result<MyData, BaseError>(BaseErrorCode.NotFound);
        return result;
    }

    private static ResultRef<MyData, RefError> ReturnRef_1()
    {
        var result = new ResultRef<MyData, RefError>(BaseErrorCode.NotFound);
        return result;
    }

    private static ResultRef<MyData, RefError> ReturnRef_3()
    {
        RefError result = BaseErrorCode.NotFound;
        return result;
    }

    public static Result<MyData, BaseError> Return_2()
    {
        var result = BaseError.Result(BaseErrorCode.NotFound);
        return result;
    }

    public static Result<MyData, BaseError> Return_3()
    {
        BaseError result = BaseErrorCode.NotFound;
        return result;
    }

    public static Result<MyData, FileError> Return_FileError()
    {
        var result = FileError.Result(FileErrorCode.A);
        return result;
    }

    public static ResultRef<MyData, RefError> ReturnRef_2()
    {
        var error = new RefError(BaseErrorCode.NotFound);
        return error;
    }

    #endregion

    #region Methods

    [Benchmark]
    public void Ref1()
    {
        //var list = new List<ResultRef<MyData, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_1();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark]
    public void Ref2()
    {
        //var list = new List<ResultRef<MyData, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_2();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark]
    public void Ref3()
    {
        //var list = new List<ResultRef<MyData, RefError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = ReturnRef_3();
            if (a.IsError(out var error))
            {
                var code = error.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark(Baseline = true)]
    public void Struct1()
    {
        //var list = new List<Result<MyData, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_1();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark]
    public void Struct2()
    {
        //var list = new List<Result<MyData, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_2();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark]
    public void Struct3()
    {
        //var list = new List<Result<MyData, BaseError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_3();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            //list.Add(a);
        }
    }

    [Benchmark]
    public void StructSmall()
    {
        //var list = new List<Result<MyData, FileError>>(Count);
        for (var i = 0; i < Count; i++)
        {
            var a = Return_FileError();
            if (a.IsError(out var error))
            {
                var code = error.Value.Code;
            }
            //list.Add(a);
        }
    }

    #endregion

}
