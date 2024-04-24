namespace Market.Authentication;

public interface IPasswordHasher
{
    public string Hash(string password, string salt);
}