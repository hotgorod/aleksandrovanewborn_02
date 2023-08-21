using System.Net.Http.Json;
using FluentAssertions;

namespace AleksandrovaNewborn.Tests.Api;

public static class HttpClientExtensions
{
    public static async Task<int> CreateClient(this HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/clients", new
        {
            ChildName = FakeData.Random.Name.FirstName(),
            EmailAddress = FakeData.EmailAddress()
        });

        response.EnsureSuccessStatusCode();

        var clientId = response.ExtractIdFromLocation("/api/clients");
        clientId.Should().HaveValue();
        return clientId!.Value;
    }
}