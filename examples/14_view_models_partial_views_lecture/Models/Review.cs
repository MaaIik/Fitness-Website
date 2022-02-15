using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Review
    {
        public int Id { get; set; }

        // Foreign key to the Book model. This is configured automatically because of the name: <Model>Id
        public int BookId { get; set; }

        // Navigation property to the linked book
        public Book Book { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [StringLength(1000)]
        public string Text { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}