using System.Collections.Generic;

namespace QuizMania.Models
{
    public class Quiz
    {

        public Quiz(){}
        public Quiz(string text, string category)
        {
            Text = text;
            Category = category;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
       
        public List<Question> Questions  { get; set; }
    }
}