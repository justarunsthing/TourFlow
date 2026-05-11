using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TourFlow.Client.Enums;
using TourFlow.Models;
using static MudBlazor.CategoryTypes;

namespace TourFlow.Data
{
    public static class DataUtility
    {
        public static string? GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        public static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var database = Environment.GetEnvironmentVariable("RAILWAY_SERVICE_NAME")
                ?? typeof(DataUtility).Assembly.GetName().Name;

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = database,
                SslMode = SslMode.Prefer
            };

            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            //Service: An instance of ApplicationDbContext
            await using var dbContextSvc = svcProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //Service: An IConfiguration instance to get appsettings/secrets/environment variables
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();
            //Migration: This is the programmatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();

            string defaultPassword = configurationSvc["DefaultPassword"] ?? throw new ApplicationException("Error seeding data - no DefaultPassword configured!");

            await SeedRolesAsync(roleManagerSvc);
            await SeedInternalUsersAsync(userManagerSvc, dbContextSvc, defaultPassword);
            await SeedTravelAgentAsync(userManagerSvc, dbContextSvc, defaultPassword);
            await dbContextSvc.DisposeAsync();
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames<Role>())
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedInternalUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string defaultPassword)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin.tourflow@mailinator.com",
                Email = "admin.tourflow@mailinator.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, defaultPassword);
                await userManager.AddToRoleAsync(defaultUser, nameof(Role.Admin));
            }

            defaultUser = new ApplicationUser
            {
                UserName = "manager.tourflow@mailinator.com",
                Email = "manager.tourflow@mailinator.com",
                FirstName = "Sarah",
                LastName = "Chen",
                EmailConfirmed = true
            };

            user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, defaultPassword);
                await userManager.AddToRoleAsync(defaultUser, nameof(Role.Manager));
            }

            defaultUser = new ApplicationUser
            {
                UserName = "sales1.tourflow@mailinator.com",
                Email = "sales1.tourflow@mailinator.com",
                FirstName = "James",
                LastName = "Rodriguez",
                EmailConfirmed = true
            };

            user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, defaultPassword);
                await userManager.AddToRoleAsync(defaultUser, nameof(Role.Manager));
            }

            defaultUser = new ApplicationUser
            {
                UserName = "sales2.tourflow@mailinator.com",
                Email = "sales2.tourflow@mailinator.com",
                FirstName = "Priya",
                LastName = "Patel",
                EmailConfirmed = true
            };

            user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, defaultPassword);
                await userManager.AddToRoleAsync(defaultUser, nameof(Role.Manager));
            }
        }

        public static async Task SeedTravelAgentAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string defaultPassword)
        {
            if (await context.TravelAgents.AnyAsync())
            {
                return;
            }

            var defaultTravelAgent = new TravelAgent
            {
                Name = "Imperial Tours"
            };

            await context.TravelAgents.AddAsync(defaultTravelAgent);
            await context.SaveChangesAsync();

            var travelAgent = await context.TravelAgents.FirstAsync(t => t.Name == "Imperial Tours");

            var defaultUser = new ApplicationUser
            {
                UserName = "imperialtours@mailinator.com",
                Email = "imperialtours@mailinator.com",
                FirstName = "Imperial",
                LastName = "Tours",
                EmailConfirmed = true,
                TravelAgentId = travelAgent.Id
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, defaultPassword);
                await userManager.AddToRoleAsync(defaultUser, nameof(Role.TravelAgent));
            }
        }
    }
}