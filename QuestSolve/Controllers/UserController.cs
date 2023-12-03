using Microsoft.AspNetCore.Mvc;
using QuestSolve.dtos;
using Microsoft.AspNetCore.Authorization;
using QuestSolve.services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace QuestSolve.controllers;

/// <summary>
/// Контролер за управление на потребителските действия като регистрация, вход и изход.
/// </summary>
public class UserController : Controller
{
    private readonly AuthService _authService;

    public UserController(AuthService authService)
    {
        _authService = authService;
    }

    // GET: User/Register
    /// <summary>
    /// Показва формата за регистрация на нов потребител.
    /// </summary>
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    // POST: User/Login
    /// <summary>
    /// Обработва данните от формата за регистрация на нов потребител.
    /// При успешна регистрация насочва потребителя към Login страницата
    /// </summary>
    /// <param name="model">Обект с данни за регистарция на потребителя.</param>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(UserRegistrationDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var success = await _authService.RegisterUserAsync(model);
        if (success)
        {
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "Registration failed. Username may already be in use.");
        return View(model);
    }

    // GET: User/Login
    /// <summary>
    /// Показва формата за вход на нов потребител.
    /// </summary>
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // POST: User/Login
    /// <summary>
    /// Обработва данните от формата за вход и аутентикира потребителя.
    /// </summary>
    /// <param name="model">Обект с данни за вход на потребителя.</param>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _authService.AuthenticateUserAsync(model);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                // Other claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

            return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }
    
}