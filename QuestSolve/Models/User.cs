namespace QuestSolve.models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public List<Question> Questions { get; set; }
    public List<Response> Responses { get; set; }
}
