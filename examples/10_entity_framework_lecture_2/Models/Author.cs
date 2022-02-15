using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Birthdate")]
        public DateTime Birthdate { get; set; }

        public List<AuthorBook> AuthorBooks { get; set; }
    }
}