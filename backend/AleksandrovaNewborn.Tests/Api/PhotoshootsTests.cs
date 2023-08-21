using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace AleksandrovaNewborn.Tests.Api;

[TestFixture]
public class PhotoshootTests : TestBase
{
    [TestFixture]
    public class Admin : PhotoshootTests
    {
        [Test]
        public async Task Can_Get_Photoshoots()
        {
            // Arrange
            var client = await CreateHttpClientFor(AdminEmail, AdminPassword);

            // Act
            var response = await client.GetAsync("api/photoshoots");

            // Assert
            response.Should().BeSuccessful();
        }

        [Test]
        public async Task Can_Add_New_Photoshoot()
        {
            // Arrange
            var http = await CreateHttpClientFor(AdminEmail, AdminPassword);
            var clientId = await http.CreateClient();
            
            // Act
            var response = await http.PostAsJsonAsync("api/photoshoots", new
            {
                ClientId = clientId,
                Name = FakeData.PhotoshootName(),
                PhotoshootDate = FakeData.Random.Date.Future()
            });
            
            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Created);
            var photoshootId = response.ExtractIdFromLocation("/api/photoshoots");

            photoshootId.Should().HaveValue();


        }
        
    }

    [TestFixture]
    public class AnonymousUser : PhotoshootTests
    {
        [Test]
        public async Task Cant_Get_Photoshoots()
        {
            // Arrange
            var client = CreateHttpClient();
        
            // Act
            var response = await client.GetAsync("api/photoshoots");
        
            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }        
    }
    
    


}