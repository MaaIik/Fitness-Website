using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitness4Life.Models
{
    public class Forum
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DisplayName("Title")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 20)]
        [DisplayName("Message")]
        public string Message { get; set; }
        
        private string _dateNow = DateTime.Now.ToString("g");
        public string Time
        {
            get { return _dateNow;}
            set { _dateNow = value; }
        }
       
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public List<Comment> Comments { get; set; }
    }
}