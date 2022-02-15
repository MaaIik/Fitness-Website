using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Fitness4Life.Models
{
    
    public class WorkoutProgramme
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Text")]
        public string Text { get; set; }
        
        //public List<WorkoutProgramme> WorkoutProgrammes { get; set; }
    }
}