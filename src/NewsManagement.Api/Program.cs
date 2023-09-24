using Shared.Middleware;
using Microsoft.OpenApi.Models;
using NewsManagement.Api.Configuration;
using NewsManagement.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console())
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        builder.AddEnvironmentVariables();
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


app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();
Log.Information("Finished Configuration");

app.Run();