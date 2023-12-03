using QuestSolve.Config;

namespace QuestSolve.controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using models;

    // Контролер за началната страница на апликацията.
public class HomeController : Controller
{
    private readonly ForumDbContext _context;

    public HomeController(ForumDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Показва списък с въпроси. Позволява търсене по заглавие и съдържание.
    /// </summary>
    /// <param name="searchQuery">Търсеният текст в заглавието и съдържанието.</param>
    /// <param name="page">Номер на текущата страница.</param>
    /// <returns>Визуализация с въпросите.</returns>
    public async Task<IActionResult> Index(string searchQuery, int page = 1)
    {
        int pageSize = 6;
        IQueryable<Question> query = _context.Questions
            .Include(q => q.PostedBy)
            .Include(q => q.Responses)
            .ThenInclude(r => r.RespondedBy);

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(q => q.Title.Contains(searchQuery) || q.Content.Contains(searchQuery));
        }

        var totalQuestions = await query.CountAsync();
        var questions = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        foreach (var question in questions)
        {
            if (!string.IsNullOrEmpty(question.ImagePath))
            {
                question.ImageUrl = "/uploads/" + Path.GetFileName(question.ImagePath);
            }

            foreach (var response in question.Responses)
            {
                if (!string.IsNullOrEmpty(response.ImagePath))
                {
                    response.ImageUrl = "/uploads/" + Path.GetFileName(response.ImagePath);
                }
            }
        }

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize);
        ViewBag.SearchQuery = searchQuery;

        return View(questions);
    }


}