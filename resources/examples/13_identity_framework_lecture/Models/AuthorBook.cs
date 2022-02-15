namespace Example.Models
{
    // This class describes the many to many relationship between the Author and Book models.
    // Additional configuration must be done in the database context, see ApplicationDbContext.cs.
    // Also see: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many
    public class AuthorBook
    {
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}