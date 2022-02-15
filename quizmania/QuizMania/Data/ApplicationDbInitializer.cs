using QuizMania.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace QuizMania.Data {
    public static class ApplicationDbInitializer {
        public static void Initialize(ApplicationDbContext context, UserManager<IdentityUser> um) {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            for (var i = 1; i < 11; i++)
            {
                var user = new IdentityUser()
                {
                    UserName = $"user{i}@uia.no",
                    Email = $"user{i}@uia.no",
                    EmailConfirmed = true
                };
                um.CreateAsync(user, "Password1.").Wait();
            }

            for (var i = 1; i < 11; i++)
            {
                context.Quizzes.Add(new Quiz()
                    {
                        Text = $"text{i}",
                        Category = $"Category {i}"
                    });
            }

            context.SaveChanges();
        }
    }
}