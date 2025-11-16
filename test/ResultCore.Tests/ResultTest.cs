using Shouldly;

namespace ResultCore.Tests;

public class ResultTest
{

    #region Constants & Statics

    private static Result<BaseError> Return_Result_Error_Test()
    {
        // way 1
        var result1 = new Result<BaseError>(BaseErrorCode.NotFound);
        // way 2
        var result2 = new Result<BaseError>(new BaseError(BaseErrorCode.NotFound));
        // way 3
        var result3 = BaseError.Result(BaseErrorCode.NotFound);
        // way 4
        BaseError result = BaseErrorCode.NotFound;

        return result;
    }

    private static Result<BaseError> Return_Result_Ok_Test()
    {
        return Result.Ok;
    }

    private static Result<MyData, BaseError> Return_ResultData_Error_Test()
    {
        // way 1
        var result1 = new Result<BaseError>(BaseErrorCode.NotFound);
        // way 2
        var result2 = new Result<BaseError>(new BaseError(BaseErrorCode.NotFound));
        // way 3
        var result3 = BaseError.Result(BaseErrorCode.NotFound);
        // way 4
        BaseError result = BaseErrorCode.NotFound;

        return result;
    }

    private static Result<MyData, BaseError> Return_ResultData_Ok_Test()
    {
        return new MyData("test");
    }

    #endregion

    #region Methods

    [Fact]
    public void Result_Test()
    {
        var result = Return_Result_Error_Test();

        // way 1
        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }

        // way 2
        if (result.IsError(out var err))
        {
            err.Value.Code.ShouldBe(BaseErrorCode.NotFound);

            switch (err.Value.Code)
            {
                case BaseErrorCode.Failure:
                    // do 1
                    break;
                case BaseErrorCode.Failed:
                case BaseErrorCode.NotFound:
                    // do 2
                    break;
                default:
                    throw ImpossibleException.Instance;
            }
        }
        else
        {
            // CS8629 Nullable value type may be null.
            //err.Value.Code.ShouldBe(BaseErrorCode.NotFound);
        }

        var ok = Return_Result_Ok_Test();
        if (ok.IsError())
        {
            //error
        }
        else
        {
            _ = Should.Throw<InvalidOperationException>(() => ok.UnwrapError());
        }
    }

    [Fact]
    public void Test()
    {
        var data = Return_ResultData_Error_Test();

        if (data.IsError(out var err, out var my))
        {
            err.ShouldNotBeNull().ShouldBe(BaseErrorCode.NotFound);
            my.ShouldBeNull();
        }

        var ok = Return_ResultData_Ok_Test();

        if (ok.IsError(out err, out my))
        {
            err.ShouldBeNull();

            // CS8602 Dereference of a possibly null reference.
            //var a = my.Name;
        }
        else
        {
            my.Name.ShouldBe("test");
        }
    }

    #endregion

}
