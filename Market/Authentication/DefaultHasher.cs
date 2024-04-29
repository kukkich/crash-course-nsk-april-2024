namespace Market.Authentication;

public class DefaultHasher : IPasswordHasher
{
    public string Hash(string password, string salt)
    {
        return password;
    }
}