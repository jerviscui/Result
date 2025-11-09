using ObjectLayoutInspector;
using Shouldly;

namespace ResultCore.Tests;

public class FileErrorTest
{

    #region Constants & Statics

    private static Result<FileError> Result_FileError_Failed()
    {
        var result = FileError.Result(FileErrorCode.B);

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`1'
        //Size: 8 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================================|
        //|   0-7: FileError <Error>k__BackingField (8 bytes) |
        //|===================================================|

        //Type layout for 'Result`1'
        //Size: 32 bytes. Paddings: 0 bytes (%0 of empty space)
        //|====================================|
        //|  0-31: Nullable`1 Error (32 bytes) |
        //|====================================|

        TypeLayout.PrintLayout(result.UnwrapError().GetType(), true);

        //Type layout for 'FileError'
        //Size: 24 bytes. Paddings: 4 bytes (%16 of empty space)
        //|=======================================================|
        //|   0-7: String <Reason>k__BackingField (8 bytes)       |
        //|-------------------------------------------------------|
        //|  8-15: Exception <Exception>k__BackingField (8 bytes) |
        //|-------------------------------------------------------|
        //| 16-19: FileErrorCode <Code>k__BackingField (4 bytes)  |
        //| |================================|                    |
        //| |   0-3: Int32 value__ (4 bytes) |                    |
        //| |================================|                    |
        //|-------------------------------------------------------|
        //| 20-23: padding (4 bytes)                              |
        //|=======================================================|

        return result;
    }

    private static Result<FileError> Result_FileError_Success()
    {
        Result<FileError> result = Result.Ok;

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`1'
        //Size: 8 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================================|
        //|   0-7: FileError <Error>k__BackingField (8 bytes) |
        //|===================================================|

        return result;
    }

    private static Result<MyClass, FileError> ResultData_FileError_Failed()
    {
        //FileError error = FileErrorCode.B;

        Result<MyClass, FileError> error = FileError.Result(FileErrorCode.B);

        TypeLayout.PrintLayout(error.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 16 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================================|
        //|   0-7: MyClass <Data>k__BackingField (8 bytes)    |
        //|---------------------------------------------------|
        //|  8-15: FileError <Error>k__BackingField (8 bytes) |
        //|===================================================|

        return error;
    }

    private static Result<MyClass, FileError> ResultData_FileError_Success()
    {
        Result<MyClass, FileError> result = new MyClass("abc");

        TypeLayout.PrintLayout(result.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 16 bytes. Paddings: 0 bytes (%0 of empty space)
        //|===================================================|
        //|   0-7: MyClass <Data>k__BackingField (8 bytes)    |
        //|---------------------------------------------------|
        //|  8-15: FileError <Error>k__BackingField (8 bytes) |
        //|===================================================|

        return result;
    }

    private static Result<int, FileError> ResultData_Int_Failed()
    {
        Result<int, FileError> error = FileError.Result(FileErrorCode.B);

        TypeLayout.PrintLayout(error.GetType(), true);
        //Type layout for 'Result`2'
        //Size: 16 bytes. Paddings: 4 bytes (%25 of empty space)
        //|===================================================|
        //|   0-7: FileError <Error>k__BackingField (8 bytes) |
        //|---------------------------------------------------|
        //|  8-11: Int32 <Data>k__BackingField (4 bytes)      |
        //|---------------------------------------------------|
        //| 12-15: padding (4 bytes)                          |
        //|===================================================|

        return error;
    }

    #endregion

    #region Methods

    [Fact]
    public void Result_FileError_Test()
    {
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

        var intFailed = ResultData_Int_Failed();
        if (intFailed.IsError(out var err3, out var data3))
        {
            err3.Value.Code.ShouldBe(FileErrorCode.B);
            data3.ShouldBe(0);
        }
    }

    #endregion

}
