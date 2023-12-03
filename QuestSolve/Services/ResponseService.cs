using QuestSolve.Config;

namespace QuestSolve.services;

using dtos;
using models;
using System;
using System.Threading.Tasks;

public class ResponseService
{
    private readonly ForumDbContext _context;

    public ResponseService(ForumDbContext context)
    {
        _context = context;
    }

    public async Task AddResponseAsync(ResponseDTO responseDto, int userId, string? imagePath)
    {
        var response = new Response
        {
            Content = responseDto.Content,
            QuestionId = responseDto.QuestionId,
            ResponseDate = DateTime.UtcNow,
            RespondedByUserId = userId,
            ImagePath = imagePath
        };

        _context.Responses.Add(response);
        await _context.SaveChangesAsync();
    }

}
