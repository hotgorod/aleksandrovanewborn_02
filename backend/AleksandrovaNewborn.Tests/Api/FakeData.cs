using Bogus;

namespace AleksandrovaNewborn.Tests.Api;

public class FakeData
{
    public static readonly Faker Random;

    static FakeData()
    {
        Random = new Faker("en");
    }
    
    public static string EmailAddress(string? userName = null) => Random.Internet.Email(lastName: userName);

    public static string Password() => "Test_" + Guid.NewGuid().ToString("N");

    public static string PhotoshootName() => "Photoshoot-" + Random.Name.FirstName();
}