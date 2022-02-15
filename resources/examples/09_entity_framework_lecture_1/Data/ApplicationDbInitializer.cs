using System;
using System.ComponentModel;
using Example.Models;

namespace Example.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var author1 = new Author
            {
                FirstName = "Lise",
                LastName = "Larsen",
                Birthdate = new DateTime(2001, 12, 21)
            };

            db.Authors.Add(author1);

            var author2 = new Author
            {
                FirstName = "Knut",
                LastName = "Knutsen",
                Birthdate = new DateTime(1995, 3, 10)
            };

            db.Authors.Add(author1);
            db.Authors.Add(author2);

            db.SaveChanges();
        }
    }
}