using Shouldly;

namespace ResultCore.Tests;

public class FileErrorTest
{

    #region Constants & Statics

    private static Result<BasicError> Result_BasicError(FileErrorCode code)
    {
        var result = BasicError.Result();

        var fileError = FileError.Result(code);

        return result.From(fileError);
    }

    private static Result<FileError> Result_FileError_Failed()
    {
        return FileError.Result(FileErrorCode.B);
    }

    private static Result<FileError> Result_FileError_Success()
    {
        return Result.Ok;
    }

    private static Result<MyClass, FileError> ResultData_FileError_Failed()
    {
        FileError error = FileErrorCode.B;
        return error;

        //return FileError.Result(FileErrorCode.B);
    }

    private static Result<MyClass, FileError> ResultData_FileError_Success()
    {
        return new MyClass();
    }

    #endregion

    #region Methods

    [Fact]
    public void Result_Error_From_Test()
    {
        var errorA = Result_BasicError(FileErrorCode.A);
        errorA.IsError(out var error).ShouldBeTrue();
        error.Code.ShouldBe((int)FileErrorCode.A);

        var errorB = Result_BasicError(FileErrorCode.B);
        errorB.IsError(out error).ShouldBeTrue();
        error.Code.ShouldBe((int)FileErrorCode.B);
    }

    [Fact]
    public void Result_FileError_Test()
    {
        var success = Result_FileError_Success();
        success.IsError().ShouldBeFalse();

        var failed = Result_FileError_Failed();
        failed.IsError().ShouldBeTrue();

        if (failed.IsError(out var err))
        {
            err.Code.ShouldBe(FileErrorCode.B);
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
            err2.Code.ShouldBe(FileErrorCode.B);
            data2.ShouldBeNull();
        }
    }

    #endregion

}
