using Market.Authentication;
using Market.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private IUsersRepository UsersRepository { get; }

    public UsersController(IUsersRepository usersRepository)
    {
        UsersRepository = usersRepository;
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