using Ecommerce_Website_Backend.Configuration;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Middleware;
using Ecommerce_Website_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Ecommerce_Website_Backend.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidation();
builder.Services.AddScoped<ProductCategoryService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidationErrorHandling();
builder.Services.AddCorsPolicies(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var corsOptions = app.Services.GetRequiredService<CorsOptions>();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
