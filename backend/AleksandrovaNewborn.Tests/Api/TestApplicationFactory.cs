using System.Data.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AleksandrovaNewborn.Models;
using AleksandrovaNewborn.Tests.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AleksandrovaNewborn.Tests.Api;

public class TestApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public TestApplicationFactory()
    {
        Server.PreserveExecutionContext = true;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AleksandrovaNewbornContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            if (dbConnectionDescriptor is not null)
            {
                services.Remove(dbConnectionDescriptor);
            }
            
            services.AddSingleton<TestSqliteConnectionFactory>();

            services.AddDbContextFactory<AleksandrovaNewbornContext>((container, options) =>
            {
                var connectionFactory = container.GetRequiredService<TestSqliteConnectionFactory>();
                var connection = connectionFactory.GetConnection(nameof(AleksandrovaNewbornContext));
                options.UseSqlite(connection);
                options.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            });

        });

        builder.UseEnvironment("Development");
    }
    
    public async Task<string?> LoginAs(string email, string password)
    {
        var client = CreateClient();
        
        // Act
        var response = await client.PostAsync("/api/auth/token", JsonContent.Create(new LoginCredentials()
        {
            Email = email,
            Password = password
        }));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return result?.Token;
    }

    public async Task<HttpClient> CreateClientFor(string email, string password)
    {
        var token = await LoginAs(email, password);
        if (token is null)
        {
            throw new InvalidOperationException($"Unable to get token for {email}");
        }

        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }    
}