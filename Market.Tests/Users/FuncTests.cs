using System.Net;
using System.Net.Http.Json;

namespace Market.Tests.Users;

public class FuncTests
{
    private readonly string baseUrl = "https://localhost:7057";

    [Test]
    public async Task CreateSameUserTwiceTest()
    {
        var url = $"{baseUrl}/users";

        var httpClient = new HttpClient();

        var request = JsonContent.Create(new
        {
            name = "name",
            login = "login",
            password = "password"
        });

        await httpClient.PostAsync(url, request);

        var secondCreateResult = await httpClient.PostAsync(url, request);

        Assert.That(secondCreateResult.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}