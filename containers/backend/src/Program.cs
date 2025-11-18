using Microsoft.EntityFrameworkCore;
// Dodane przez Rafała
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Infrastructure;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfile.Infrastructure;
using System.Reflection;
using Scalar.AspNetCore;
using Mapster;
using MapsterMapper;
using PrzepisakApi.src.Features.Auth.Infrastructure;
using PrzepisakApi.src.Features.Auth.Domain;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Zezwolenie na przyjmowanie zapytań
var allowedOrigin = "http://10.6.57.161:5000";

// Mapster configuration
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(Program).Assembly);

// Zezwolenie na przyjmowanie zapytań
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<EfContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<EfContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEfContext, EfContext>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<EfContext>();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EfContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); //seeding
    dbContext.Database.EnsureCreated();
    dbContext.SeedData(userManager); //seeding
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Zezwolenie na przyjmowanie zapytań
app.UseCors("FrontendCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
