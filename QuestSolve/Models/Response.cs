namespace QuestSolve.models;

public class Response
{
    public int ResponseId { get; set; }
    public string Content { get; set; }
    public DateTime ResponseDate { get; set; }
    public string? ImagePath { get; set; }
    public string? ImageUrl { get; set; }
    public int RespondedByUserId { get; set; }
    public int QuestionId { get; set; }
    
    public User RespondedBy { get; set; }
    public Question Question { get; set; }
}
