namespace QuestSolve.dtos;

public class ResponseDTO
{
    public string Content { get; set; }
    public int QuestionId { get; set; }
    public IFormFile? Image { get; set; }
}