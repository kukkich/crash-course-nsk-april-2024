using Market.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Market.Misc;

internal class ParserDbResult
{
    public static bool DbResultIsSuccessful(DbResult dbResult, out IActionResult error) =>
        DbResultStatusIsSuccessful(dbResult.Status, out error);

    public static bool DbResultIsSuccessful<T>(DbResult<T> dbResult, out IActionResult error) =>
        DbResultStatusIsSuccessful(dbResult.Status, out error);

    public static bool DbResultStatusIsSuccessful(DbResultStatus status, out IActionResult error)
    {
        error = null!;
        switch (status)
        {
            case DbResultStatus.Ok:
                return true;
            case DbResultStatus.NotFound:
                error = new StatusCodeResult(StatusCodes.Status404NotFound);
                return false;
            default:
                error = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return false;
        }
    }
}