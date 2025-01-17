using Shouldly;

namespace ResultCore.Tests;

public class FileErrorTest
{

    #region Constants & Statics

    private static Result<MyClass, FileError> Result_Data_Test()
    {
        var result = FileError.Result();

        var i = 1;
        if (i == 1)
        {
            return new MyClass();
        }

        return result;
    }

    private static Result<FileError> Result_FileError_Failed()
    {
        return FileError.Result(FileErrorCode.B);
    }

    private static Result<FileError> Result_FileError_Success()
    {
        return Result.Ok;
    }

    private static Result<FileError> Result_FileError_Test()
    {
        var result = FileError.Result(FileErrorCode.B);
        //var result = new Result<FileError>(FileErrorCode.B);

        if (result.IsError())
        {
            FileError error = FileErrorCode.A;
            return error;
        }

        return Result.Ok;
    }

    private static Result<BasicError> Result_Test(FileErrorCode code)
    {
        var result = BasicError.Result();

        var fileError = FileError.Result(code);

        if (fileError.IsError(out var error))
        {
            switch (error.Code)
            {
                case FileErrorCode.A:
                    return Result.Ok;
                case FileErrorCode.B:
                    break;
                default:
                    throw ImpossibleException.Instance;
            }

            return result.From(fileError);
        }

        return Result.Ok;
    }

    private static Result<FileError> ResultData_FileError_Failed()
    {
        return FileError.Result(FileErrorCode.B);
    }

    private static Result<MyClass, FileError> ResultData_FileError_Success()
    {
        return new MyClass();
    }

    #endregion

    #region Methods

    [Fact]
    public void Result_FileError_Test()
    {
        var success = Result_FileError_Success();
        success.IsError().ShouldBeFalse();

        var success = Result_FileError_();
        success.IsError().ShouldBeTrue();

        var result = Result_FileError_Test();

        if (result.IsError(out var error))
        {
            error.Code.ShouldBe(FileErrorCode.A);
        }

        var data = Result_Data_Test();
        data.IsError(out var err, out var my).ShouldBe(false);
        err.ShouldBeNull();
        _ = my.ShouldNotBeNull();
    }

    [Fact]
    public void Result_Test_A()
    {
        var result = Result_Test(FileErrorCode.A);

        result.IsError().ShouldBeFalse();
    }

    [Fact]
    public void Result_Test_B()
    {
        var result = Result_Test(FileErrorCode.B);

        result.IsError().ShouldBeTrue();
        if (result.IsError(out var error))
        {
            error.Code.ShouldBe((int)FileErrorCode.B);
        }
    }

    #endregion

}
