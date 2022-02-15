using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Example.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Add 5 authors
            for (int i = 1; i <= 5; i++)
            {
                db.Authors.Add(new Author
                {
                    FirstName = $"First { i }",
                    LastName = $"Last { i }",
                    Birthdate = new DateTime(1900 + i, i, 10 + i)
                });
            }

            // Add 10 books
            for (int i = 1; i <= 10; i++)
            {
                db.Books.Add(new Book
                {
                    Title = $"Title { i }",
                    Summary = $"Summary { i }",
                    Published = new DateTime(1900 + 20 + i, i, 10 + i)
                });
            }

            db.SaveChanges();

            foreach (var book in db.Books.Include(b => b.Reviews))
            {
                // Add 3 reviews to each book
                for (int i = 1; i <= 3; i++)
                {
                    // We're adding the review directly to an existing book so BookId is set automatically
                    book.Reviews.Add(new Review
                    {
                            Stars = i,
                            Text = $"Review {i}"
                    });
                }
            }

            db.SaveChanges();

            // Connect authors to books by adding AuthorBook model instances to the database
            db.AuthorBooks.AddRange(new List<AuthorBook>
            {
                new AuthorBook { AuthorId = 1, BookId = 1 },
                new AuthorBook { AuthorId = 1, BookId = 2 },
                new AuthorBook { AuthorId = 2, BookId = 1 },
                new AuthorBook { AuthorId = 2, BookId = 2 },
                new AuthorBook { AuthorId = 3, BookId = 3 },
                new AuthorBook { AuthorId = 3, BookId = 4 },
                new AuthorBook { AuthorId = 3, BookId = 5 },
                new AuthorBook { AuthorId = 4, BookId = 4 },
                new AuthorBook { AuthorId = 4, BookId = 5 },
                new AuthorBook { AuthorId = 4, BookId = 6 },
                new AuthorBook { AuthorId = 5, BookId = 7 }
            });

            db.SaveChanges();
        }
    }
}