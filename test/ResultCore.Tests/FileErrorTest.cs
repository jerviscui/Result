using ObjectLayoutInspector;
using Shouldly;
using System.Runtime.CompilerServices;

namespace ResultCore.Tests;

public class FileErrorTest
{

    #region Constants & Statics

    private static Result<FileError> Result_FileError_Failed()
    {
        var result = FileError.Result(FileErrorCode.B);

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`1'
        //Size: 8 bytes. Paddings: 3 bytes (%37 of empty space)
        //|==========================================================|
        //|     0: Boolean _hasError (1 byte)                        |
        //|----------------------------------------------------------|
        //|   1-3: padding (3 bytes)                                 |
        //|----------------------------------------------------------|
        //|   4-7: FileError _error (4 bytes)                        |
        //| |======================================================| |
        //| |   0-3: FileErrorCode <Code>k__BackingField (4 bytes) | |
        //| | |================================|                   | |
        //| | |   0-3: Int32 value__ (4 bytes) |                   | |
        //| | |================================|                   | |
        //| |======================================================| |
        //|==========================================================|

        var r2 = BaseError.Result(BaseErrorCode.NotFound);
        TypeLayout.PrintLayout(r2.GetType(), true);
        //Type layout for 'Result`1'
        //Size: 32 bytes. Paddings: 11 bytes (%34 of empty space)
        //|===========================================================|
        //|     0: Boolean _hasError (1 byte)                         |
        //|-----------------------------------------------------------|
        //|   1-7: padding (7 bytes)                                  |
        //|-----------------------------------------------------------|
        //|  8-31: BaseError _error (24 bytes)                        |
        //| |=======================================================| |
        //| |   0-7: String <Reason>k__BackingField (8 bytes)       | |
        //| |-------------------------------------------------------| |
        //| |  8-15: Exception <Exception>k__BackingField (8 bytes) | |
        //| |-------------------------------------------------------| |
        //| | 16-19: BaseErrorCode <Code>k__BackingField (4 bytes)  | |
        //| | |================================|                    | |
        //| | |   0-3: Int32 value__ (4 bytes) |                    | |
        //| | |================================|                    | |
        //| |-------------------------------------------------------| |
        //| | 20-23: padding (4 bytes)                              | |
        //| |=======================================================| |
        //|===========================================================|

        return result;
    }

    private static Result<FileError> Result_FileError_Success()
    {
        Result<FileError> result = Result.Ok;

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`1'
        //Size: 8 bytes. Paddings: 3 bytes (%37 of empty space)
        //|==========================================================|
        //|     0: Boolean _hasError (1 byte)                        |
        //|----------------------------------------------------------|
        //|   1-3: padding (3 bytes)                                 |
        //|----------------------------------------------------------|
        //|   4-7: FileError _error (4 bytes)                        |
        //| |======================================================| |
        //| |   0-3: FileErrorCode <Code>k__BackingField (4 bytes) | |
        //| | |================================|                   | |
        //| | |   0-3: Int32 value__ (4 bytes) |                   | |
        //| | |================================|                   | |
        //| |======================================================| |
        //|==========================================================|

        return result;
    }

    private static Result<MyClass, FileError> ResultData_FileError_Failed()
    {
        Result<MyClass, FileError> error = FileError.Result(FileErrorCode.B);

        TypeLayout.PrintLayout(error.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 16 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================|
        //|   0-7: MyClass Data (8 bytes)     |
        //|-----------------------------------|
        //|  8-15: Nullable`1 Error (8 bytes) |
        //|===================================|

        //TypeLayout.PrintLayout<FileError?>(true);
        TypeLayout.PrintLayout(error.UnwrapError().GetType(), true);
        //Type layout for 'FileError'
        //Size: 4 bytes. Paddings: 0 bytes (%0 of empty space)
        //|======================================================|
        //|   0-3: FileErrorCode <Code>k__BackingField (4 bytes) |
        //| |================================|                   |
        //| |   0-3: Int32 value__ (4 bytes) |                   |
        //| |================================|                   |
        //|======================================================|

        Result<MyClass, BaseError> r2 = BaseError.Result(BaseErrorCode.NotFound);
        TypeLayout.PrintLayout(r2.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 40 bytes. Paddings: 0 bytes (%0 of empty space)
        //|====================================|
        //|   0-7: MyClass Data (8 bytes)      |
        //|------------------------------------|
        //|  8-39: Nullable`1 Error (32 bytes) |
        //|====================================|

        //TypeLayout.PrintLayout<BaseError?>(true);
        TypeLayout.PrintLayout(r2.UnwrapError().GetType(), true);
        //Type layout for 'BaseError'
        //Size: 24 bytes. Paddings: 4 bytes (%16 of empty space)
        //|=======================================================|
        //|   0-7: String <Reason>k__BackingField (8 bytes)       |
        //|-------------------------------------------------------|
        //|  8-15: Exception <Exception>k__BackingField (8 bytes) |
        //|-------------------------------------------------------|
        //| 16-19: BaseErrorCode <Code>k__BackingField (4 bytes)  |
        //| |================================|                    |
        //| |   0-3: Int32 value__ (4 bytes) |                    |
        //| |================================|                    |
        //|-------------------------------------------------------|
        //| 20-23: padding (4 bytes)                              |
        //|=======================================================|

        return error;
    }

    private static Result<MyClass, FileError> ResultData_FileError_Success()
    {
        Result<MyClass, FileError> result = new MyClass("abc");

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 16 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================|
        //|   0-7: MyClass Data (8 bytes)     |
        //|-----------------------------------|
        //|  8-15: Nullable`1 Error (8 bytes) |
        //|===================================|

        return result;
    }

    #endregion

    #region Methods

    //private static Result<int, FileError> ResultData_Int_Failed()
    //{
    //    Result<int, FileError> error = FileError.Result(FileErrorCode.B);

    //    TypeLayout.PrintLayout(error.GetType(), true);
    //    //Type layout for 'Result`2'
    //    //Size: 16 bytes. Paddings: 4 bytes (%25 of empty space)
    //    //|===================================|
    //    //|   0-3: Int32 Data (4 bytes)       |
    //    //|-----------------------------------|
    //    //|  4-11: Nullable`1 Error (8 bytes) |
    //    //|-----------------------------------|
    //    //| 12-15: padding (4 bytes)          |
    //    //|===================================|

    //    return error;
    //}

    [Fact]
    public void Result_FileError_Test()
    {
        var size = Unsafe.SizeOf<Result<BaseError>>();
        var errorSize = Unsafe.SizeOf<BaseError>();

        Console.WriteLine($"Result<{nameof(BaseError)}> 布局:");
        Console.WriteLine($"  TError 大小: {errorSize} 字节");
        Console.WriteLine($"  bool 偏移: {errorSize} 字节");
        Console.WriteLine($"  总大小: {size} 字节");
        Console.WriteLine($"  填充: {size - errorSize - 1} 字节");

        var success = Result_FileError_Success();
        success.IsError().ShouldBeFalse();

        var failed = Result_FileError_Failed();
        failed.IsError().ShouldBeTrue();

        if (failed.IsError(out var err))
        {
            err.Value.Code.ShouldBe(FileErrorCode.B);
        }
    }

    [Fact]
    public void ResultData_FileError_Test()
    {
        var success = ResultData_FileError_Success();
        success.IsError(out _).ShouldBeFalse();
        if (success.IsError(out var err1, out var data1))
        {
            err1.ShouldBeNull();
            return;
        }
        _ = data1.ShouldNotBeNull();

        var failed = ResultData_FileError_Failed();
        if (failed.IsError(out var err2, out var data2))
        {
            err2.Value.Code.ShouldBe(FileErrorCode.B);
            data2.ShouldBeNull();
        }

        //var intFailed = ResultData_Int_Failed();
        //if (intFailed.IsError(out var err3, out var data3))
        //{
        //    err3.Value.Code.ShouldBe(FileErrorCode.B);
        //    data3.ShouldBe(0);
        //}
    }

    #endregion

}
