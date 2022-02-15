using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Summary { get; set; }

        [DataType(DataType.Date)]
        public DateTime Published { get; set; }

        // List of reviews. This creates a one to many relationship together with BookId in the Review model.
        public List<Review> Reviews { get; set; }

        // List of AuthorBook junction models. This creates a many to many relationship together with the list in Author
        // and configuration in ApplicationDbContext.cs.
        public List<AuthorBook> AuthorBooks { get; set; }
    }
}