using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Market.DAL.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.Authentication;

public class AuthenticationFilter : ActionFilterAttribute, IAsyncActionFilter
{
    private PasswordHasher _passwordHasher;
    private UsersRepository _usersRepository;
    private readonly bool _acceptOnlySellers;

    public AuthenticationFilter(bool acceptOnlySellers=false)
    {
        _acceptOnlySellers = acceptOnlySellers;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _passwordHasher = new PasswordHasher();
        _usersRepository = new UsersRepository(_passwordHasher);
        var response = context.HttpContext.Response;

        var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers.Authorization);
        if (authHeader.Scheme is not "Basic")
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        var headerValues = authHeader.Parameter!.Split(' ');
        var (login, password) = (headerValues[0], headerValues[1]);

        var result = await _usersRepository.GetUserByLogin(login);
        if (result.IsFailure)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        var user = result.Value!;

        var isPasswordValid = password == _passwordHasher.Hash(user.PasswordHash, user.Salt);

        if (!isPasswordValid)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        if (_acceptOnlySellers && !user.IsSeller)
        {
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            return;
        }

        context.HttpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new ("isSeller", user.IsSeller.ToString()),
                }
            )
        );

        await next.Invoke();
    }
}