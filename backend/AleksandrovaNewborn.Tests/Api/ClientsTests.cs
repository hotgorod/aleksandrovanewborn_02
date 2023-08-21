using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace AleksandrovaNewborn.Tests.Api;

[TestFixture]
public class ClientsTests : TestBase
{
    [TestFixture]
    public class Admin : ClientsTests
    {
        [Test]
        public async Task Can_Create_Clients()
        {
            // Arrange
            var client = await CreateHttpClientFor(AdminEmail, AdminPassword);

            // Act
            var response = await client.PostAsJsonAsync("/api/clients", new
            {
                ChildName = FakeData.Random.Name.FirstName(),
                EmailAddress = FakeData.EmailAddress()
            });

            // Assert
            response.Should().BeSuccessful();
            
            var clientId = response.ExtractIdFromLocation("/api/clients");
            clientId.Should().HaveValue();
        }

        [Test]
        public async Task Can_Get_Clients()
        {
            // Arrange
            var client = await CreateHttpClientFor(AdminEmail, AdminPassword);
            
            // client 1
            var client1 = await client.CreateClient();
            
            // client 2
            var client2 = await client.CreateClient();
            
            // Act
            
            var clients = await client.GetFromJsonAsync<(string ChildName, string EmailAddress)[]>("/api/clients");
            
            // Assert
            clients.Should().HaveCount(2);
        }
        
    }
    
    [TestFixture]
    public class AnonymousUser : ClientsTests
    {
        [Test]
        public async Task Cant_Get_Clients()
        {
            // Arrange
            var client = CreateHttpClient();
        
            // Act
            var response = await client.GetAsync("api/clients");
        
            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public async Task Cant_Add_Clients()
        {
            // Arrange
            var client = CreateHttpClient();
        
            // Act
            var response = await client.PostAsJsonAsync("/api/clients", new
            {
                ChildName = FakeData.Random.Name.FirstName(),
                EmailAddress = FakeData.EmailAddress()
            });
        
            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }         
    }
}