using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fitness4Life.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Fitness4Life.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um,
            RoleManager<IdentityRole> rm)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var adminRole = new IdentityRole("Admin");
            rm.CreateAsync(adminRole).Wait();

            var admin = new ApplicationUser
            {
                UserName = "admin@uia.no",
                Email = "admin@uia.no",
                Nickname = "Cool Nick",
                EmailConfirmed = true
            };
            um.CreateAsync(admin, "Password1.").Wait();
            um.AddToRoleAsync(admin, "Admin");


            for (var i = 0; i < 10; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"user{i}@uia.no",
                    Email = $"user{i}@uia.no",
                    Nickname = $"Nickname{i}",
                    EmailConfirmed = true
                };
                um.CreateAsync(user, "Password1.").Wait();

                db.Forums.Add(new Forum()
                {
                    Title = $"Title{i}",
                    Message = $"Message{i}",
                    ApplicationUserId = user.Id
                });
                db.SaveChanges();
            }

            for (var i = 0; i < 10; i++)
            {
                db.WorkoutProgrammes.Add(new WorkoutProgramme()
                {
                    Title = $"Tittel på programme {i}",
                    Text = $"Teksten til programme {i}",
                });
                db.SaveChanges();
            }
        }
    }
}