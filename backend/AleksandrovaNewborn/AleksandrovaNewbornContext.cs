using AleksandrovaNewborn.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AleksandrovaNewborn;

public class AleksandrovaNewbornContext : IdentityDbContext
{
    public DbSet<Photoshoot> Photoshoots { get; set; }
    
    public DbSet<Client> Clients { get; set; }
    
    public AleksandrovaNewbornContext()
    {
    }

    public AleksandrovaNewbornContext(DbContextOptions<AleksandrovaNewbornContext> options)
        : base(options)
    {
    }
}
