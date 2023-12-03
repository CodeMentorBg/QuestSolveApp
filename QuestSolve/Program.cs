using Microsoft.EntityFrameworkCore;
using QuestSolve.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using QuestSolve.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Настройка на аутентикацията
builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
{
    //Входна точка на апликацията
    options.LoginPath = "/User/Login";
});


builder.Services.AddMvc(options =>
{
    // Изисква потребителите да са аутентикирани
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Конфигуриране на контекста на базата данни
builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("QuestSolveConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<ResponseService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
    dbContext.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();