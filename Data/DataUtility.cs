using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Blog.Data
{
    public static class DataUtility
    {
        // Admin & Moderator roles
        private const string? _adminRole = "Admin";
        private const string? _moderatorRole = "Moderator";

        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();
            var configurationsSvc = svcProvider.GetRequiredService<IConfiguration>();
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Align the datbase by checking the Migrations
            await dbContextSvc.Database.MigrateAsync();

            // Seed some info!
            await SeedRolesAsync(roleManagerSvc);
            await SeedBlogUsersAsync(userManagerSvc, configurationsSvc);


        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole!))  /*do something if admin role does not exist*/
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole!)); /* */
            }

            if (!await roleManager.RoleExistsAsync(_moderatorRole!))  /*do something if moderator does not exist*/
            {
                await roleManager.CreateAsync(new IdentityRole(_moderatorRole!)); /* */
            }


        }


        private static async Task SeedBlogUsersAsync(UserManager<BlogUser> userManager, IConfiguration configuration)
        {
            string? adminEmail = configuration["AdminEmail"] ?? Environment.GetEnvironmentVariable("AdminEmail");  /* check locally then online for adminEmail*/
            string? adminPassword = configuration["AdminPWD"] ?? Environment.GetEnvironmentVariable("AdminPWD");  /* check locally then online for adminEmail*/

            string? moderatorEmail = configuration["ModeratorEmail"] ?? Environment.GetEnvironmentVariable("ModeratorEmail");  /* check locally then online for adminEmail*/
            string? moderatorPassword = configuration["ModeratorPWD"] ?? Environment.GetEnvironmentVariable("ModeratorPWD");  /* check locally then online for adminEmail*/

            try
            {
                // Seed the Admin
                BlogUser? adminUser = new BlogUser() 
                { 
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Tom",
                    LastName = "Farrrell",
                    EmailConfirmed = true
                };

                BlogUser? blogUser = await userManager.FindByEmailAsync(adminEmail!);
                
                if(blogUser == null)
                {
                    await userManager.CreateAsync(adminUser, adminPassword!);
                    await userManager.AddToRoleAsync(adminUser, _adminRole!);
                }


                // Seed the Moderator
                BlogUser? moderatorUser = new BlogUser()
                {
                    UserName = moderatorEmail,
                    Email = moderatorEmail,
                    FirstName = "Tom",
                    LastName = "Farrrell",
                    EmailConfirmed = true
                };

                blogUser = await userManager.FindByEmailAsync(moderatorEmail!);  /*reusing the already declared blogUser variable*/

                if (blogUser == null)
                {
                    await userManager.CreateAsync(moderatorUser, moderatorPassword!);
                    await userManager.AddToRoleAsync(moderatorUser, _moderatorRole!);
                }



            }

                catch (Exception ex)
            {
                Console.WriteLine("**************ERROR************************");
                Console.WriteLine("Error Seeding Default Blog Users");
                Console.WriteLine(ex.Message);
                Console.WriteLine("*******************************************");
                throw;
            }

        }

    }
}
