using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EF.Compiled;

internal class Initializer
{
    public static async Task<AppDbContext> Run()
    {
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseInMemoryDatabase(nameof(AppDbContext));
        optionsBuilder.UseLazyLoadingProxies();

        AppDbContext _context = new AppDbContext(optionsBuilder.Options);
        await SeedData(_context);

        return _context;
    }

    private static async Task SeedData(AppDbContext context)
    {
        #region User

        Faker<User> users = new Faker<User>()
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.ProfileImage, f => f.Image.Image());

        List<User> usersList = users.GenerateBetween(1, 1);
        await context.Users.AddRangeAsync(usersList);
        await context.SaveChangesAsync();

        #endregion

        #region Shop

        Faker<Shop> shop = new Faker<Shop>()
            .RuleFor(x => x.Name, f => f.Person.Company.Name)
            .RuleFor(x => x.Description, f => f.Person.Company.CatchPhrase)
            .RuleFor(x => x.LogoUrl, f => f.Image.Image())
            .RuleFor(x => x.Owner, usersList.First());

        List<Shop> shopList = shop.GenerateBetween(1, 1);
        await context.Shops.AddRangeAsync(shopList);
        await context.SaveChangesAsync();

        #endregion

        #region Products

        Faker<Product> product = new Faker<Product>()
            .RuleFor(x => x.Name, f => f.Commerce.Product())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Image, f => f.Image.Image())
            .RuleFor(x => x.Price, f => f.Commerce.Price(symbol: "$"))
            .RuleFor(x => x.Shop, shopList.First());

        List<Product> productList = product.GenerateBetween(100, 100);
        await context.Products.AddRangeAsync(productList);
        await context.SaveChangesAsync();

        #endregion

        #region Phones

        Faker<Phone> phone = new Faker<Phone>()
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Shop, shopList.First());

        List<Phone> phoneList = phone.GenerateBetween(50, 50);
        await context.Phones.AddRangeAsync(phoneList);
        await context.SaveChangesAsync();

        #endregion

        #region Location

        Faker<Location> location = new Faker<Location>()
            .RuleFor(x => x.Locale, f => f.Commerce.Locale)
            .RuleFor(x => x.ShortDescription, f => f.Address.StreetAddress(true))
            .RuleFor(x => x.Shop, shopList.First());

        List<Location> locationList = location.GenerateBetween(50, 50);
        await context.Locations.AddRangeAsync(locationList);
        await context.SaveChangesAsync();

        #endregion
    }
}
