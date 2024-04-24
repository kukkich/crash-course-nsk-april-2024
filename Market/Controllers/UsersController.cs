using Market.Authentication;
using Market.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private UsersRepository UsersRepository { get; }

    public UsersController()
    {
        UsersRepository = new UsersRepository(new PasswordHasher());
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserCreateDto newUser)
    {
        var result = await UsersRepository.CreateUser(newUser);

        return result.MatchActionResult(id => Ok(id));
    }

    [HttpPatch("{userId:guid}/seller-status")]
    public async Task<IActionResult> SetSellerStatus(Guid userId, [FromBody] bool isSeller)
    {
        var result = await UsersRepository.SetSellerStatus(userId, isSeller);

        return result.MatchActionResult(_ => Ok());
    }
}