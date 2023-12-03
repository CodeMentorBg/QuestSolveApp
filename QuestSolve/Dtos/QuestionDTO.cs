namespace QuestSolve.dtos
{
    public class QuestionDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; } 
    }
}