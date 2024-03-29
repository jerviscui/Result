using Shouldly;

namespace ResultCore.Tests;

public class FileErrorTest
{
    private Result<Error> Result_Test(FileErrorCode code)
    {
        var result = Error.Result();

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
                    throw ImpossibleExcption.Instance;
            }

            return result.From(fileError);
        }

        return Result.Ok;
    }

    private Result<FileError> Result_FileError_Test()
    {
        var result = FileError.Result();
        result = new Result<FileError>(FileErrorCode.B);

        if (result.IsError())
        {
            FileError error = FileErrorCode.A;
            return error;
        }

        return Result.Ok;
    }

    private Result<MyClass, FileError> Result_Data_Test()
    {
        var result = FileError.Result();

        var i = 1;
        if (i == 1)
        {
            return new MyClass();
        }

        return result;
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

    [Fact]
    public void Test()
    {
        var result = Result_FileError_Test();

        if (result.IsError(out var error))
        {
            error.Code.ShouldBe(FileErrorCode.A);
        }

        var data = Result_Data_Test();
        data.IsError(out var err, out var my).ShouldBe(false);
        err.ShouldBeNull();
        my.ShouldNotBeNull();
    }
}
