namespace QuizBHXH.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string CorrectAnswer { get; set; }
    }
}