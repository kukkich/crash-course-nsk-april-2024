using System.Net;
using Market.DAL;
using Market.Misc;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

public static class Extensions
{
    public static IActionResult MatchActionResult<TValue>(
        this Result<TValue, Error> result, Func<TValue, IActionResult> onSucceed)
    {
        return result.Match(
            onSucceed,
            error => error switch
            {
                Error.NotFound => new StatusCodeResult(StatusCodes.Status404NotFound),
                Error.AlreadyExist => new StatusCodeResult((int)HttpStatusCode.Conflict),
                Error.Conflict => new StatusCodeResult((int)HttpStatusCode.Conflict),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            }
        );
    }
}