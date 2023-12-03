namespace QuestSolve.models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public int PostedByUserId { get; set; }
        
        public User PostedBy { get; set; }
        public List<Response> Responses { get; set; }
    }
}