namespace QuizMania.Models
{
    public class Question
    {

        public Question(){}
        public Question(string questiontext)
        {
            QuestionText = questiontext;
        }
        public int Id { get; set; }
        public string QuestionText { get; set; }
        
        public int QuizId { get; set; }
        
        public Quiz Quiz { get; set; }

    }
}