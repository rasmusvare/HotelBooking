using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    public static async void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration conf)
    {
        using var serviceScope = app
            .ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope
            .ServiceProvider
            .GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No DB context!");
        }

        if (conf.GetValue<bool>("DataInitialization:DropDatabase")
            && !context.Database.IsInMemory())
        {
            context.Database.EnsureDeleted();
        }

        if (conf.GetValue<bool>("DataInitialization:MigrateDatabase")
            && !context.Database.IsInMemory())
        {
            context.Database.Migrate();
        }

        if (conf.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (userManager == null || roleManager == null)
            {
                throw new NullReferenceException("userManager or roleManager cannot be null");
            }

            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var roleName in roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;

                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole() {Name = roleName}).Result;

                    if (!identityResult.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed");
                    }
                }
            }

            var users =
                new (string username, string firstname, string lastname, string roles, string password, string idCode)[]
                {
                    ("admin@hotell.ee", "Hotelli", "Admin", "admin", "Kala.maja1", "38902180335"),
                    ("rasmus@hotell.ee", "Rasmus", "Vare", "user", "Kala.maja1", "38002210335"),
                };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = userInfo.username,
                        FirstName = userInfo.firstname,
                        LastName = userInfo.lastname,
                        UserName = userInfo.username,
                        EmailConfirmed = true,
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;

                    if (!identityResult.Succeeded)
                    {
                        throw new AggregateException("Cannot create user");
                    }

                    await context.SaveChangesAsync();
                }

                if (!string.IsNullOrWhiteSpace(userInfo.roles))
                {
                    var identityResultRoles = userManager.AddToRolesAsync(user,
                            userInfo.roles.Split(",").Select(r => r.Trim()))
                        .Result;
                }
            }
        }

        if (conf.GetValue<bool>("DataInitialization:SeedData")
            && !context.Database.IsInMemory()
           )
        {
            var hotel = new Hotel
            {
                Address = "Pohla tee 4",
                Description = "Very nice hotel",
                Email = "admin@pohlahotell.ee",
                Name = "Pohla hotell",
                TelephoneNumber = "512345678"
            };
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            // Add room types
            var roomTypes =
                new (Guid hotelId, string name, string description, int count,int noOfBeds, decimal pricePerNight)[]
                {
                    (hotel.Id, "Standard room", "Standard twin room", 0, 2, 100),
                    (hotel.Id, "Deluxe room", "Presidential suite", 0, 3, 12000),
                };

            foreach (var each in roomTypes)
            {
                var roomType = new RoomType
                {
                    HotelId = each.hotelId,
                    Name = each.name,
                    NumberOfBeds = each.noOfBeds,
                    Description = each.description,
                    Count = each.count,
                    PricePerNight = each.pricePerNight
                };

                context.RoomTypes.Add(roomType);
                await context.SaveChangesAsync();
            }

            // Add amenities
            var amenities =
                new (Guid hotelId, string name, string description)[]
                {
                    (hotel.Id, "TV", "Big television set"),
                    (hotel.Id, "Bath", "Cold water only"),
                };

            foreach (var each in amenities)
            {
                var amenity = new Amenity()
                {
                    HotelId = each.hotelId,
                    Name = each.name,
                    Description = each.description
                };

                context.Amenities.Add(amenity);
                await context.SaveChangesAsync();
            }

            // Add rooms
            // var rooms = new (Guid hotelId, string roomNumber, string roomType)
        }
    }
}