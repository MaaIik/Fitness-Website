using System.ComponentModel.DataAnnotations;

namespace Fitness4Life.Models
{
    // Simple model used in ExtraController and Views/Extra
    public class Question
    {
        public Question() {}

        public Question(string choice)
        {
            Choice = choice;
        }

        [Display(Name="WHAT TYPE OF WORKOUT DO YOU TYPICALLY DO?")]
        public string Choice { get; set; }
    }
}
