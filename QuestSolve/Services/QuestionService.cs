
using QuestSolve.dtos;
using QuestSolve.models;
using Microsoft.EntityFrameworkCore;
using QuestSolve.Config;

namespace QuestSolve.services;

public class QuestionService
{
    private readonly ForumDbContext _context;

    public QuestionService(ForumDbContext context)
    {
        _context = context;
    }

    public async Task AddQuestionAsync(QuestionDTO questionDto, int userId, string? imagePath)
    {
        var question = new Question
        {
            Title = questionDto.Title,
            Content = questionDto.Content,
            PostDate = DateTime.UtcNow,
            PostedByUserId = userId,
            ImagePath = imagePath
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
    }

    
    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Questions.Include(q => q.Responses).FirstOrDefaultAsync(q => q.QuestionId == questionId);

        if (question != null)
        {
            _context.Responses.RemoveRange(question.Responses);
            _context.Questions.Remove(question);

            await _context.SaveChangesAsync();
        }
    }

}