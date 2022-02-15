using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Fitness4Life.Models {
    
    public class Comment {
        public Comment() { }

        public Comment( string summary, ApplicationUser nickname)
        {
            Summary = summary;
            ApplicationUser = nickname;
        }
        public int CommentId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Summary { get; set; }
        
      
        private string _dateNow = DateTime.Now.ToString("g");
        public string Time
        {
            get { return _dateNow;}
            set { _dateNow = value; }
        }
        
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        
        public int ForumId { get; set; }
        public Forum Forum  { get; set; }
    }
   
}