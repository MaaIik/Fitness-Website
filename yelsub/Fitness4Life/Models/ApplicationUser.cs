using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Fitness4Life.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
        public List<Forum> Blogs { get; set; }
        public List<Comment> Comments { get; set; }
        
    }
}