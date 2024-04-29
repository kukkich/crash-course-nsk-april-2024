using Market.DAL;
using Market.Misc;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

public static class Extensions
{
    public static IActionResult MatchActionResult<TValue>(
        this Result<TValue, DbError> result, Func<TValue, IActionResult> onSucceed)
    {
        return result.Match(
            onSucceed,
            error => error switch
            {
                DbError.NotFound => new StatusCodeResult(StatusCodes.Status404NotFound),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            }
        );
    }
}