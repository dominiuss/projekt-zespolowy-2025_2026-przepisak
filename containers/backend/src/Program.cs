using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
// Dodane przez Rafała
using Microsoft.OpenApi;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PrzepisakApi.api.src.Features.Auth.Infrastructure.Options;
using PrzepisakApi.api.src.Features.Auth.Services;
using PrzepisakApi.src;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.Auth.Infrastructure;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Infrastructure;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfile.Infrastructure;
using PrzepisakApi.src.Features.Ratings.Domain;
using PrzepisakApi.src.Features.Ratings.Infrastructure;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;

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

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
builder.Services
    .AddAuthentication(a =>
    {
        a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudiences = jwtSettings.Audiences,
            ValidateLifetime = true,
        };
        jwt.Audience = jwtSettings.Audiences[0];
        jwt.ClaimsIssuer = jwtSettings.Issuer;
    });
builder.Services.AddDbContext<EfContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEfContext, EfContext>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<EfContext>();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EfContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); //seeding
    dbContext.Database.EnsureCreated();
    await dbContext.SeedData(userManager); //seeding
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Zezwolenie na przyjmowanie zapytań
app.UseCors("FrontendCorsPolicy");
//app.UseAuthentication();
app.Urls.Add("http://0.0.0.0:5035");
app.UseAuthorization();

app.MapControllers();

app.Run();
