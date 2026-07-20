using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            try
            {
                if (isProd)
                {
                    Console.WriteLine("--> Attempting to apply migration");
                    try
                    {
                        Console.WriteLine("--> Before Migration");
                        context.Database.Migrate();
                        Console.WriteLine("--> After Migration");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"-->Could not run migration {ex.Message}");
                    }
                }

                if (!context.Platforms.Any())
                {
                    Console.WriteLine("-->Seeding Data...");

                    context.Platforms.AddRange(
                        new Models.Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                        new Models.Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                        new Models.Platform() { Name = "Kubernetes", Publisher = "Cloud Native", Cost = "Free" }
                    );

                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("-->We already have data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-->Could not seed data {ex.Message}");
            }
        }
    }
}