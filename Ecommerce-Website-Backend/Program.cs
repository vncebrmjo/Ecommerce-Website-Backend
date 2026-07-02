using Ecommerce_Website_Backend.Configuration;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Extensions;
using Ecommerce_Website_Backend.Helpers;
using Ecommerce_Website_Backend.Middleware;
using Ecommerce_Website_Backend.Services;
using Ecommerce_Website_Backend.Services.AuthService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Secrets
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Services
builder.Services.AddScoped<ProductCategoryService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LogoutService>();
builder.Services.AddScoped<UserManagementService>();

// Helpers
builder.Services.AddScoped<JwtHelper>();

// Infrastructure
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidationErrorHandling();
builder.Services.AddCorsPolicies(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

await app.CreateSuperAdminAsync();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

var corsOptions = app.Services.GetRequiredService<CorsOptions>();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors(corsOptions.PolicyName);
app.UseAuthentication();    
app.UseAuthorization();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.MapControllers();

app.Run();