using Microsoft.AspNetCore.Identity;

namespace Example.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
        
        public int Age { get; set; }
    }
}