using FA22.P05.Web.Features.Authorization;
using FA22.P05.Web.Features.ItemListings;
using FA22.P05.Web.Features.Listings;
using FA22.P05.Web.Features.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FA22.P05.Web.Data;

public static class MigrateAndSeed
{
    public static async Task Initialize(IServiceProvider services)
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();

        await AddRoles(services);
        await AddUsers(services);

        AddProducts(context);
        await AddListings(context);
    }

    private static void AddProducts(DataContext context)
    {
        var products = context.Set<Product>();
        if (products.Any())
        {
            return;
        }

        products.Add(new Product
        {
            Name = "Persona 5",
            Description = "A PS4 Bluray Disk copy of the Playstation hit Persona 5.",
            
        });
        products.Add(new Product
        {
            Name = "Luigi's Mansion",
            Description = "A small Gamecube copy of Luigi's Mansion",
        });
        products.Add(new Product
        {
            Name = "Splatoon 3 OLED",
            Description = "The special edition Nintendo Switch OLED featuring Splatoon designs.",
        });
        context.SaveChanges();
    }

    private static async Task AddUsers(IServiceProvider services)
    {
        const string defaultPassword = "Password123!";

        var userManager = services.GetRequiredService<UserManager<User>>();
        if (userManager.Users.Any())
        {
            return;
        }

        var adminUser = new User
        {
            UserName = "alkadi"
        };
        await userManager.CreateAsync(adminUser, defaultPassword);
        await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);

        var bobUser = new User
        {
            UserName = "matthew"
        };
        await userManager.CreateAsync(matthewUser, defaultPassword);
        await userManager.AddToRoleAsync(matthewUser, RoleNames.User);

        var sueUser = new User
        {
            UserName = "travis"
        };
        await userManager.CreateAsync(travisUser, defaultPassword);
        await userManager.AddToRoleAsync(travisUser, RoleNames.User);

        await services.GetRequiredService<DataContext>().SaveChangesAsync();
    }

    private static async Task AddRoles(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        if (roleManager.Roles.Any())
        {
            return;
        }

        await roleManager.CreateAsync(new Role
        {
            Name = RoleNames.Admin
        });

        await roleManager.CreateAsync(new Role
        {
            Name = RoleNames.User
        });
    }

    private static async Task AddListings(DataContext context)
    {
        var listings = context.Set<Listing>();
        var users = context.Set<User>();
        var userId = users.Select(x => x.Id).FirstOrDefault();
        if (listings.Any(x => x.EndUtc > DateTimeOffset.UtcNow.Date))
        {
            return;
        }

        listings.Add(new Listing
        {
            Name = "NES Classic Edition",
            Price = 229.99m,
            Description = "I am selling a mint condition classic system the NES (some games included).",
            StartUtc = DateTimeOffset.UtcNow.Date,
            EndUtc = DateTimeOffset.UtcNow.AddDays(10),
            UserId = userId,
            ItemsForSale = new List<ItemListing>()
        });
        listings.Add(new Listing
        {
            Name = "Final Fantasy 7",
            Price = 19.99m,
            Description = "I am selling a copy of Final Fantasy 7.",
            StartUtc = DateTimeOffset.UtcNow.Date,
            EndUtc = DateTimeOffset.UtcNow.AddDays(10),
            UserId = userId,
            ItemsForSale = new List<ItemListing>()
        });
        listings.Add(new Listing
        {
            Name = "Xbox 360 (500 GB).",
            Price = 99.99m,
            Description = "I have and am selling a used Xbox 360.",
            StartUtc = DateTimeOffset.UtcNow.Date,
            EndUtc = DateTimeOffset.UtcNow.AddDays(10),
            UserId = userId,
            ItemsForSale = new List<ItemListing>()
        });
        await context.SaveChangesAsync();
    }
}