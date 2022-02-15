using System;
using Microsoft.AspNetCore.Identity;

using Example.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Data
{
    public class ApplicationDbInitializer
    {
        // Receive the service provider as a parameter, allows the initializer itself to get the services it needs
        public static void Initialize(IServiceProvider services)
        {
            // Get the db context
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Recreate database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Get the user manager and role manager
            var um = services.GetRequiredService<UserManager<ApplicationUser>>();
            var rm = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Create the admin role
            var adminRole = new IdentityRole("Admin");

            // Add the admin role
            rm.CreateAsync(adminRole).Wait();

            // Add a regular user (no extra roles)
            var user = new ApplicationUser
            {
                UserName = "user@uia.no",
                Email = "user@uia.no",
                Street = "User Street 89",
                ZipCode = 1234,
                City = "User City",
                EmailConfirmed = true
            };

            um.CreateAsync(user, "Password1.").Wait();

            // Add an admin user (in the admin role)
            var admin = new ApplicationUser
            {
                UserName = "admin@uia.no",
                Email = "admin@uia.no",
                Street = "Admin Street 21",
                ZipCode = 5678,
                City = "Admin City",
                EmailConfirmed = true
            };

            um.CreateAsync(admin, "Password1.").Wait();
            um.AddToRoleAsync(admin, adminRole.Name).Wait();
        }
    }
}
