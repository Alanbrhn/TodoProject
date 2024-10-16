using Microsoft.EntityFrameworkCore;
using TodoProject.DataContext;
using TodoProject.Midleware;
using TodoProject.Repositories;
using TodoProject.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();  
builder.Logging.AddConsole();      
builder.Logging.AddFile(builder.Configuration.GetSection("Logging:File")); 

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
