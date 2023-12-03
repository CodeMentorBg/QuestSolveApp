using System.Security.Claims;
using QuestSolve.dtos;
using QuestSolve.services;

namespace QuestSolve.controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

/// <summary>
/// Контролер за управление на въпросите в апликацията.
/// Обработва създаването, отговарянето и изтриването на въпроси.
/// </summary>
public class QuestionController : Controller
{
    private readonly QuestionService _questionService;
    private readonly ResponseService _responseService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public QuestionController(ResponseService responseService ,QuestionService questionService, IWebHostEnvironment hostingEnvironment)
    {
        _questionService = questionService;
        _responseService = responseService;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Question/Create
    /// <summary>
    /// Показва формата за създаване на нов въпрос.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    
    // POST: Question/Create
    /// <summary>
    /// Обработва данните от формата за създаване на нов въпрос.
    /// </summary>
    /// <param name="model">Обектът за създаване на нов въпрос.</param>
    [HttpPost]
    public async Task<IActionResult> Create(QuestionDTO model)
    {
        if (ModelState.IsValid)
        {
            string? imagePath = null;
            if (model.Image != null)
            {
                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
                
                imagePath = filePath; 
            }
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            await _questionService.AddQuestionAsync(model, userId, imagePath);

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }
    
    // POST: Question/AddResponse
    /// <summary>
    /// Обработва добавянето на отговор към въпрос.
    /// </summary>
    /// <param name="responseDto">Обектът с данни за новия отговор.</param>
    [HttpPost]
    public async Task<IActionResult> AddResponse(ResponseDTO responseDto)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "Home");
        }

        string? imagePath = null;
        if (responseDto.Image != null)
        {
            var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(responseDto.Image.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await responseDto.Image.CopyToAsync(fileStream);
            }

            imagePath = filePath; 
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _responseService.AddResponseAsync(responseDto, userId, imagePath);

        return RedirectToAction("Index", "Home");
    }

    
    // GET: Question/Delete
    /// <summary>
    /// Изтрива въпрос по даден идентификатор.
    /// </summary>
    /// <param name="id">Идентификатор на въпроса за изтриване.</param>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await _questionService.DeleteQuestionAsync(id);
        return RedirectToAction("Index", "Home");
    }
    
}