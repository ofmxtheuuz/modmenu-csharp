using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModMenu.Models;

namespace ModMenu.Infra;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Models.ModMenu> ModMenus { get; set; }
    public DbSet<UserModMenu> UserModMenus { get; set; }
    public DbSet<Key> Keys { get; set; }
}