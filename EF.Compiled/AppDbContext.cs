using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Compiled;

public class AppDbContext : DbContext
{
    #region Ctor

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    #endregion

    #region DbSet's

    public DbSet<User> Users { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }

    #endregion
}
