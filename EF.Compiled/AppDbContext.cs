using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EF.Compiled;

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

    #region OnModelCreating

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region User
        
        Faker<User> users = new Faker<User>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.ProfileImage, f => f.Image.Image());

        List<User> usersList = users.GenerateBetween(1, 1);
        builder.Entity<User>().HasData(usersList);

        #endregion

        #region Shop

        Faker<Shop> shop = new Faker<Shop>()
            .RuleFor(x => x.Name, f => f.Person.Company.Name)
            .RuleFor(x => x.Description, f => f.Person.Company.CatchPhrase)
            .RuleFor(x => x.LogoUrl, f => f.Image.Image())
            .RuleFor(x => x.Owner, usersList.First());

        List<Shop> shopList = shop.GenerateBetween(1,1);
        builder.Entity<Shop>().HasData(shopList);

        #endregion

        #region Products

        Faker<Product> product = new Faker<Product>()
            .RuleFor(x => x.Name, f => f.Commerce.Product())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Image, f => f.Image.Image())
            .RuleFor(x => x.Price, f => f.Commerce.Price(symbol: "$"))
            .RuleFor(x => x.Shop, shopList.First());

        List<Product> productList = product.GenerateBetween(100, 100);
        builder.Entity<Product>().HasData(productList);

        #endregion

        #region Phones

        Faker<Phone> phone = new Faker<Phone>()
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Shop, shopList.First());

        List<Phone> phoneList = phone.GenerateBetween(50, 50);
        builder.Entity<Phone>().HasData(phoneList);

        #endregion

        #region Location

        Faker<Location> location = new Faker<Location>()
            .RuleFor(x => x.Locale, f => f.Commerce.Locale)
            .RuleFor(x => x.ShortDescription, f => f.Address.StreetAddress(true))
            .RuleFor(x => x.Shop, shopList.First());

        List<Location> locationList = location.GenerateBetween(50, 50);
        builder.Entity<Location>().HasData(locationList);

        #endregion
    }

    #endregion
}
