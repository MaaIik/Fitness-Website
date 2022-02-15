using System.Collections.Generic;

namespace Example.Models
{
    // This model combines data from two database tables.
    // It is only used in controllers and views, it's not stored as a DbSet.
    public class RecentAdditionsViewModel
    {
        public List<Author> Authors { get; set; }
        public List<Book> Books { get; set; }
        
    }
}