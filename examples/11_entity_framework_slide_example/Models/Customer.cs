using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Customer
    {
        public Customer() {}

        public Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public int Id { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public List<Order> Orders { get; set; }
    }
}
