using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TourFlow.Client.Enums;
using TourFlow.Models;

namespace TourFlow.Data
{
    public static class DataUtility
    {
        private static readonly List<int> seededTravelAgentIds = [];

        private static Faker faker = new();

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
            await SeedDefaultUsersAsync(userManagerSvc, defaultPassword);
            await SeedDefaultEnquiriesQuotationsAndBookingsAsync(dbContextSvc, userManagerSvc);
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
                var faker = new Faker<TravelAgent>()
                    .RuleFor(t => t.CompanyName, f => f.Company.CompanyName())
                    .RuleFor(t => t.ContactPerson, f => f.Name.FullName())
                    .RuleFor(t => t.Phone, f => f.Phone.PhoneNumber("+44 ## ### ####"))
                    .RuleFor(t => t.Email, (f, t) => $"{t.ContactPerson!.ToLower().Replace(" ", ".")}@mailinator.com");

                var travelAgents = faker.Generate(5);

                await context.TravelAgents.AddRangeAsync(travelAgents);
                await context.SaveChangesAsync();

                // Store the IDs for later use when seeding enquiries
                seededTravelAgentIds.Clear();
                seededTravelAgentIds.AddRange(travelAgents.Select(t => t.Id));
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

        public static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager, string defaultPassword)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@tourflow.com",
                Email = "admin@tourflow.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };

            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, defaultPassword);
                    await userManager.AddToRoleAsync(defaultUser, nameof(Role.Admin));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Admin User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            defaultUser = new ApplicationUser
            {
                UserName = "opsmanager@tourflow.com",
                Email = "opsmanager@tourflow.com",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                EmailConfirmed = true
            };

            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, defaultPassword);
                    await userManager.AddToRoleAsync(defaultUser, nameof(Role.OperationsManager));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Default Operations Manager User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            // Create 3 Sales Executives
            for (int i = 1; i <= 3; i++)
            {
                var email = $"sales{i}@tourflow.com";
                var existing = await userManager.FindByEmailAsync(email);

                if (existing == null)
                {
                    var salesFaker = new Faker<ApplicationUser>()
                        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                        .RuleFor(u => u.LastName, f => f.Name.LastName())
                        .RuleFor(u => u.Email, (f, u) => email)
                        .RuleFor(u => u.UserName, (f, u) => email);

                    var generatedSales = salesFaker.Generate();

                    var salesUser = new ApplicationUser
                    {
                        UserName = generatedSales.Email,
                        Email = generatedSales.Email,
                        FirstName = generatedSales.FirstName,
                        LastName = generatedSales.LastName,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(salesUser, defaultPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(salesUser, nameof(Role.SalesExecutive));
                    }
                }
            }
        }

        public static async Task SeedDefaultEnquiriesQuotationsAndBookingsAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (await context.TourEnquiries.AnyAsync())
                {
                    return;
                }

                var opsManager = await userManager.FindByEmailAsync("opsmanager@tourflow.com");
                var salesUsers = await userManager.GetUsersInRoleAsync(Role.SalesExecutive.ToString());
                var random = new Random();
                var enquiries = new List<TourEnquiry>();
                var quotations = new List<Quotation>();
                var bookings = new List<Booking>();

                foreach (var agentId in seededTravelAgentIds)
                {
                    int numEnquiries = random.Next(2, 5);

                    for (int i = 0; i < numEnquiries; i++)
                    {
                        var status = (EnquiryStatus)random.Next(0, 6);

                        string? assignedToId = status == EnquiryStatus.New ? opsManager?.Id : salesUsers.Any() ? salesUsers[random.Next(salesUsers.Count)].Id : null;

                        var enquiry = new TourEnquiry
                        {
                            EnquiryNumber = $"ENQ-{DateTime.UtcNow:yyyyMMdd}-{random.Next(1000, 9999)}",
                            GroupSize = random.Next(8, 35),
                            StartDate = DateTimeOffset.UtcNow.AddDays(random.Next(30, 120)),
                            EndDate = DateTimeOffset.UtcNow.AddDays(random.Next(140, 160)),
                            Destination = random.Next(0, 2) == 0 ? "Italy (Rome & Florence)" : "Spain (Barcelona & Costa Brava)",
                            Budget = random.Next(0, 3) switch
                            {
                                0 => "£25,000 - £35,000",
                                1 => "£40,000 - £55,000",
                                _ => "£60,000 - £80,000"
                            },
                            RequestedServices = "Coach, Hotel, Tour Leader, Meals",
                            AdditionalNotes = random.Next(0, 2) == 0 ? "Clients prefer 4-star hotels with central location." : "",
                            Status = status,
                            TravelAgentId = agentId,
                            AssignedToId = assignedToId,
                            Created = DateTimeOffset.UtcNow.AddDays(-random.Next(0, 15))
                        };

                        enquiries.Add(enquiry);
                    }
                }

                await context.TourEnquiries.AddRangeAsync(enquiries);
                await context.SaveChangesAsync();

                foreach (var enquiry in enquiries)
                {
                    if (enquiry.Status == EnquiryStatus.Rejected || enquiry.Status == EnquiryStatus.Closed)
                        continue;

                    var quotation = new Quotation
                    {
                        QuotationNumber = $"QUO-{DateTime.UtcNow:yyyyMMdd}-{random.Next(1000, 9999)}",
                        TourEnquiryId = enquiry.Id,
                        TotalAmount = enquiry.GroupSize * random.Next(1200, 2800),
                        Currency = "GBP",
                        AIItinerarySummary = "A wonderful journey through historic cities with premium accommodations.",
                        Status = (QuotationStatus)random.Next(0, 4),
                        Created = enquiry.Created.AddDays(random.Next(1, 5)),
                        SentAt = enquiry.Created.AddDays(random.Next(3, 8))
                    };

                    quotations.Add(quotation);

                    if (quotation.Status == QuotationStatus.Accepted)
                    {
                        var booking = new Booking
                        {
                            BookingNumber = $"BK-{DateTime.UtcNow:yyyyMMdd}-{random.Next(1000, 9999)}",
                            TourEnquiryId = enquiry.Id,
                            QuotationId = quotation.Id,
                            TotalAmount = quotation.TotalAmount,
                            Currency = "GBP",
                            Status = BookingStatus.Confirmed,
                            BookingDate = quotation.SentAt?.AddDays(random.Next(2, 7)) ?? DateTimeOffset.UtcNow,
                            ConfirmedAt = DateTimeOffset.UtcNow.AddDays(-random.Next(0, 10))
                        };

                        bookings.Add(booking);
                        enquiry.Status = EnquiryStatus.Converted;
                    }
                }

                await context.Quotations.AddRangeAsync(quotations);
                await context.SaveChangesAsync();

                foreach (var booking in bookings)
                {
                    var relatedQuotation = quotations.FirstOrDefault(q => q.TourEnquiryId == booking.TourEnquiryId);

                    if (relatedQuotation != null)
                    {
                        booking.QuotationId = relatedQuotation.Id;
                    }
                }

                await context.Bookings.AddRangeAsync(bookings);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Enquiries, Quotations and Bookings");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }
    }
}