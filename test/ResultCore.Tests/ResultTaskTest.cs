using Shouldly;

namespace ResultCore.Tests;

public class ResultTaskTest
{

    #region Constants & Statics

    private static async ResultTask<BaseError> ResultTask_Error_Async()
    {
        await Task.Delay(10000);

        return new BaseError(BaseErrorCode.NotFound);
    }

    private static ResultTask<BaseError> ResultTask_Error_Sync()
    {
        return BaseError.Result(BaseErrorCode.NotFound, null, null);
    }

    #endregion

    #region Methods

    [Fact]
    public async Task ResultTask_Error_TestAsync()
    {
        var result = await ResultTask_Error_Sync();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }
    }

    [Fact]
    public async Task ResultTask_ErrorAsync_TestAsync()
    {
        var result = await ResultTask_Error_Async();

        if (result.IsError())
        {
            result.UnwrapError().Code.ShouldBe(BaseErrorCode.NotFound);
        }
    }

    #endregion

}
