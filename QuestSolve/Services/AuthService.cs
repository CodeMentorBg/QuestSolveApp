using QuestSolve.Config;
using QuestSolve.dtos;
using QuestSolve.models;

namespace QuestSolve.services;

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class AuthService
{
    private readonly ForumDbContext _context;

    public AuthService(ForumDbContext context)
    {
        _context = context;
    }

    
    /// <summary>
    /// Регистрира нов потребител в системата.
    /// </summary>
    /// <param name="registrationDto">DTO с данни за регистрацията.</param>
    /// <returns>Връща true, ако регистрацията е успешна.</returns>
    public async Task<bool> RegisterUserAsync(UserRegistrationDTO registrationDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registrationDto.Username))
        {
            return false;
        }

        var hashedPassword = HashPassword(registrationDto.Password);
        var user = new User
        {
            Username = registrationDto.Username,
            Email = registrationDto.Email,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }


    /// <summary>
    /// Аутентикира потребител на база потребителско име и парола.
    /// </summary>
    /// <param name="loginDto">DTO с данни за входа на потребителя.</param>
    /// <returns>Връща обект User, ако аутентикацията е успешна.</retu
    public async Task<User> AuthenticateUserAsync(UserLoginDto loginDto)
    {
        // Търси потребителя в базата данни
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

        // Проверява паролата
        if (user != null && VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }

    //Хеширанме на паролата
    private string HashPassword(string password)
    {
        // Използва SHA256 за хеширането.
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }
    }

    // Проверява дали дадена парола съвпада със съхранения хеш
    private bool VerifyPassword(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == storedHash;
    }
}
