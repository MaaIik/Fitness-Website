using Microsoft.AspNetCore.Identity;

namespace Example.Models
{
    /* This class extends the default IdentityUser class from Identity Framework. To enable it you must:
     * - Edit ApplicationDbContext.cs, replace IdentityDbContext with IdentityDbContext<ApplicationUser>
     * - Edit Startup.cs, replace IdentityUser with ApplicationUser
     * - Edit Views/Shared/_LoginPartial.cshtml, replace IdentityUser with ApplicationUser
     */
    public class ApplicationUser : IdentityUser
    {
        // Add address fields
        public string Street { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
    }
}
