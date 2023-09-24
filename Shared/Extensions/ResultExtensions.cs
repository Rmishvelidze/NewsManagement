using Shared.Library;
using Shared.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<ActionResult> Match(
            this Task<Result> resultTask,
            Func<ActionResult> onSuccess,
            Func<Result, ActionResult> onFailure
        )
        {
            var result = await resultTask;
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }

        public static async Task<ActionResult<ApiServiceResponse<TIn>>> Match<TIn>(
            this Task<Result<TIn>> resultTask,
            Func<TIn, ActionResult<ApiServiceResponse<TIn>>> onSuccess,
            Func<Result, ActionResult<ApiServiceResponse<TIn>>> onFailure
        )
        {
            var result = await resultTask;
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
        }
    }
}