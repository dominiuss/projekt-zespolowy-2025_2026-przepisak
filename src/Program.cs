using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Infrastructure;
using System.Reflection;
using Scalar.AspNetCore;
using Mapster;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

// Mapster configuration
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(Program).Assembly);

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EfContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEfContext, EfContext>();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EfContext>();
    dbContext.Database.EnsureCreated();
    dbContext.SeedData();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
