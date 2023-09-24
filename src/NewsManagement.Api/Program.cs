using System.IdentityModel.Tokens.Jwt;
using Shared.Middleware;
using Microsoft.OpenApi.Models;
using NewsManagement.Api.Configuration;
using NewsManagement.Application;
using Serilog;
using NewsManagementMinimal.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NewsManagement.Application.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
        lc.ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.File(
                path: builder.Configuration["LogFilePath"] + "/" + DateTime.Today.ToString("yy.MM.dd") + "/",
                rollingInterval: RollingInterval.Hour))
    .ConfigureAppConfiguration((_, configurationBuilder) =>
    {
        configurationBuilder.AddEnvironmentVariables();
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});

builder.Services.AddControllers();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsManagement.Api API V1", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapHealthChecks("/hc-lb");

app.UseMiddleware(typeof(ErrorHandlingMiddleware));
//app.UseMiddleware(typeof(CustomMiddleware));


app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();
app.Run();
return;

IResult Login(UserLogin user, IUserRepository userRepo)
{
    if (string.IsNullOrEmpty(user.Username) ||
        string.IsNullOrEmpty(user.Password)) return Results.BadRequest("Invalid user credentials");
    var loggedInUser = userRepo.Get(user);
    if (loggedInUser is null) return Results.NotFound("User not found");

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
        new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
        new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
        new Claim(ClaimTypes.Surname, loggedInUser.Surname),
        new Claim(ClaimTypes.Role, loggedInUser.Role)
    };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
            SecurityAlgorithms.HmacSha256)
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(tokenString);
}