using Market.Authentication;
using Market.Misc;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal class UsersRepository
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly RepositoryContext _context;

    public UsersRepository(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _context = new RepositoryContext();
    }

    public async Task<Result<User, DbError>> GetUserByLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        if (user is null)
        {
            return DbError.NotFound;
        }

        return user;
    }


    public async Task<Result<Guid, DbError>> CreateUser(UserCreateDto newUser)
    {
        if (await _context.Users.AnyAsync(x => x.Login == newUser.Login))
        {
            return DbError.AlreadyExist;
        }

        var salt = Guid.NewGuid().ToString();
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = newUser.Name,
            Login = newUser.Login,
            Salt = salt,
            PasswordHash = _passwordHasher.Hash(newUser.Password, salt)
        };

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();

            return user.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }

    public async Task<Result<Unit, DbError>> SetSellerStatus(Guid userId, bool newSellerStatus)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
        {
            return DbError.NotFound;
        }

        user.IsSeller = newSellerStatus;

        try
        {
            await _context.SaveChangesAsync();

            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }
}