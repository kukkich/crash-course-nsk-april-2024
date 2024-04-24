using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Market.DAL.Repositories;

namespace Market.Authentication;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _passwordHasher = new PasswordHasher();
        _usersRepository = new UsersRepository(_passwordHasher);
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value!.Contains("products") &&
            context.Request.Method.ToLower() == "post")
        {
            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers.Authorization);
            if (authHeader.Scheme is not "Basic")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var headerValues = authHeader.Parameter!.Split(' ');
            var (login, password) = (headerValues[0], headerValues[1]);

            var result = await _usersRepository.GetUserByLogin(login);
            if (result.IsFailure)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var user = result.Value!;

            var isPasswordValid = password == _passwordHasher.Hash(user.PasswordHash, user.Salt);

            if (!isPasswordValid)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            if (!user.IsSeller)
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return;
            }

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new ("isSeller", user.IsSeller.ToString()),
                    }
                )
            );
        }

        await _next(context);
    }
}