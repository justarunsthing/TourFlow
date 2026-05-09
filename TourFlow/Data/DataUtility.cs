using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TourFlow.Client.Enums;
using TourFlow.Models;

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
            await SeedDefaultTravelAgentsAsync(dbContextSvc);
            await SeedDefaultUsersAsync(userManagerSvc, dbContextSvc, defaultPassword);
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

        public static async Task SeedDefaultTravelAgentsAsync(ApplicationDbContext context)
        {
            if (await context.TravelAgents.AnyAsync())
            {
                return;
            }

            try
            {
                IList<TravelAgent> defaultAgents =
                [
                    new TravelAgent() { Name = "Sky Travels" },
                    new TravelAgent() { Name = "Imperial Tour & Travels" }
                ];

                var dbTravelAgents = context.TravelAgents.Select(t => t.Name).ToList();

                await context.TravelAgents.AddRangeAsync(defaultAgents.Where(a => !dbTravelAgents.Contains(a.Name)));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Travel Agents");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string defaultPassword)
        {
            await SeedInternalUsers(userManager, defaultPassword);
            await SeedTravelAgentUsers(userManager, context, defaultPassword);
        }

        private static async Task SeedInternalUsers(UserManager<ApplicationUser> userManager, string defaultPassword)
        {
            var usersToSeed = new List<(string Email, string FirstName, string LastName, Role Role)>
            {
                ("admin@tourflow.com", "System", "Administrator", Role.Admin),
                ("manager@tourflow.com", "Sarah", "Chen", Role.Manager),
                ("sales1@tourflow.com", "James", "Rodriguez", Role.Sales),
                ("sales2@tourflow.com", "Priya", "Patel", Role.Sales)
            };

            foreach (var (email, first, last, role) in usersToSeed)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = first,
                        LastName = last,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role.ToString());
                    }
                }
            }
        }

        private static async Task SeedTravelAgentUsers(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string defaultPassword)
        {
            var skyTravels = await context.TravelAgents.FirstOrDefaultAsync(t => t.Name == "Sky Travels");
            var imperial = await context.TravelAgents.FirstOrDefaultAsync(t => t.Name == "Imperial Tour & Travels");

            if (skyTravels == null || imperial == null) return;

            var agentUsers = new List<(string Email, string FirstName, string LastName, int TravelAgentId)>
            {
                ("agent@skytravels.com", "Emma", "Thompson", skyTravels.Id),
                ("lead@skytravels.com", "Marcus", "Okoro", skyTravels.Id),
                ("agent@imperial.com", "Sophie", " Laurent", imperial.Id)
            };

            foreach (var (email, first, last, agentId) in agentUsers)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = first,
                        LastName = last,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, nameof(Role.Agent));

                        // Link user to TravelAgent
                        var travelAgent = await context.TravelAgents
                            .Include(t => t.Members)
                            .FirstAsync(t => t.Id == agentId);

                        travelAgent.Members.Add(user);
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}