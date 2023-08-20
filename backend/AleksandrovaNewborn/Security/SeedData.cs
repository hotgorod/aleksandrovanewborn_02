using Microsoft.AspNetCore.Identity;

namespace AleksandrovaNewborn.Security;

internal class SeedData
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SeedData> _logger;

    public SeedData(IServiceScopeFactory scopeFactory, ILogger<SeedData> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task InteractiveSeed()
    {
        Console.Write("Enter administrator email:");
        var email = Console.ReadLine();
   
        Console.Write("Enter administrator password:");
        var password = Console.ReadLine();

        return Seed(email, password);
    }

    public async Task Seed(string email, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        await SeedRoles(roleManager);
        await SeedAdministrator(email, password, userManager);
    }

    private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        await SeedRole(Roles.Administrator, roleManager);
        await SeedRole(Roles.Client, roleManager);
    }
    
    private async Task SeedRole(string roleName, RoleManager<IdentityRole> roleManager)
    {
        var result = await roleManager.CreateAsync(new IdentityRole()
        {
            Name = roleName
        });
        if (!result.Succeeded)
        {
            LogIdentityErrors(result.Errors);
        }
    }
    
  
    private async Task SeedAdministrator(string email, string password, UserManager<IdentityUser> userManager)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

        var identityUser = new IdentityUser
        {
            UserName = email, 
            Email = email,
        };
        
        var result = await userManager.CreateAsync(identityUser, password);
        if (!result.Succeeded)
        {
            LogIdentityErrors(result.Errors);
       return;
        }

        await userManager.AddToRoleAsync(identityUser, Roles.Administrator);

    }

    private void LogIdentityErrors(IEnumerable<IdentityError> errors)
    {
        foreach (IdentityError error in errors)
        {
            _logger.LogError("Error {Code}: {Description}", error.Code, error.Description);
        }    
    }
}