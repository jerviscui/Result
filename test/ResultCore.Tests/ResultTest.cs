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

    private static Task<Result<BaseError>> Return_Task_Result_Error_TestAsync()
    {
        // way 1
        var result1 = new Result<BaseError>(BaseErrorCode.NotFound);
        // way 2
        var result2 = new Result<BaseError>(new BaseError(BaseErrorCode.NotFound));
        // way 3
        var result3 = BaseError.Result(BaseErrorCode.NotFound);

        return result3;
    }

    private static Task<Result<BaseError>> Return_Task_Result_Ok_TestAsync()
    {
        // way 1
        var result = BaseError.Result(BaseErrorCode.NotFound);
        //do sth.
        result = Result.Ok;
        return result;

        // way 2
        //return Result.Ok.AsTask<BaseError>();
    }

    private static Task<Result<MyData, BaseError>> Return_Task_ResultData_Error_TestAsync()
    {
        // way 1
        Result<MyData, BaseError> result1 = BaseError.Result(BaseErrorCode.NotFound);
        return result1;

        // way 2
        //var result2 = BaseError.Result(BaseErrorCode.NotFound);
        //return result2.AsTask<MyData, BaseError>();
    }

    private static Task<Result<MyData, BaseError>> Return_Task_ResultData_Ok_TestAsync()
    {
        // way 1
        Result<MyData, BaseError> result1 = BaseError.Result(BaseErrorCode.NotFound);
        //do sth.
        result1 = new MyData("test");
        return result1;

        // way 2
        //var data = new MyData("test");
        //return data.AsTask<MyData, BaseError>();
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
            switch (result.GetErrorRef().Code)
            {
                case BaseErrorCode.Failure:
                    break;
                case BaseErrorCode.Failed:
                    break;
                case BaseErrorCode.NotFound:
                    break;
                default:
                    throw ImpossibleException.Instance;
            }

            ref readonly var error = ref result.GetErrorRef();
            error.Code.ShouldBe(BaseErrorCode.NotFound);
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
            _ = Should.Throw<InvalidOperationException>(() => ok.GetErrorRef());
        }
    }

    [Fact]
    public void ResultData_Test()
    {
        var data = Return_ResultData_Error_Test();

        if (data.IsError(out var err, out var my))
        {
            err.ShouldNotBeNull().ShouldBe(BaseErrorCode.NotFound);
            my.ShouldBeNull();
        }

        if (data.IsError())
        {
            ref readonly var error = ref data.GetErrorRef();
            error.Code.ShouldBe(BaseErrorCode.NotFound);
        }
        else
        {
            data.Data.Name.ShouldBe("test");
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

    [Fact]
    public async Task Task_Result_Test()
    {
        var result = await Return_Task_Result_Error_TestAsync();

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

        var ok = await Return_Task_Result_Ok_TestAsync();
        if (ok.IsError())
        {
            //error
        }
        else
        {
            _ = Should.Throw<InvalidOperationException>(() => ok.GetErrorRef());
        }
    }

    [Fact]
    public async Task Task_ResultData_Test()
    {
        var data = await Return_Task_ResultData_Error_TestAsync();

        if (data.IsError(out var err, out var my))
        {
            err.ShouldNotBeNull().ShouldBe(BaseErrorCode.NotFound);
            my.ShouldBeNull();
        }

        var ok = await Return_Task_ResultData_Ok_TestAsync();

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
