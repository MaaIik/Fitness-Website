using Microsoft.EntityFrameworkCore.Query;
namespace Fitness4Life.Models
{
    public class ForumCommentViewModel
    {
        public IIncludableQueryable<Forum, ApplicationUser> Forums { get; set;}
        public IIncludableQueryable<Comment, ApplicationUser> Comments { get; set; }
        
    }
}