using AleksandrovaNewborn.Security;
using Microsoft.Extensions.DependencyInjection;

namespace AleksandrovaNewborn.Tests.Api;

public class TestBase
{
    private TestApplicationFactory<Program> _factory;
    
    private string _adminEmail;
    private string _adminPassword;

    protected string AdminEmail => _adminEmail;

    protected string AdminPassword => _adminPassword;

    [OneTimeSetUp]
    public async Task SeedDatabase()
    {
        _adminEmail = FakeData.EmailAddress();
        _adminPassword = FakeData.Password();
        _factory = new TestApplicationFactory<Program>();
        
        using var scope = _factory.Services.CreateScope();
            
        var appointmentsDb = scope.ServiceProvider.GetRequiredService<AleksandrovaNewbornContext>();
        await appointmentsDb.Database.EnsureCreatedAsync();
        
        var dataSeeder = scope.ServiceProvider.GetRequiredService<SeedData>();
        await dataSeeder.Seed(_adminEmail, _adminPassword);        
    }    
    
    protected HttpClient CreateHttpClient() => _factory.CreateClient();

    protected Task<HttpClient> CreateHttpClientFor(string email, string password) => _factory.CreateClientFor(email, password);
}