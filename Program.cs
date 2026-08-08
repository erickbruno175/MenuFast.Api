using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Application.Services.Seguranca;
using MenuFast.Api.Api.Middlewares;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MenuFast API",
        Version = "v1"
    });
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UsuarioContextService>();
builder.Services.AddScoped<SegurancaService>();
builder.Services.AddScoped<GoogleEmailService>();
//builder.Services.AddScoped<JwtBlacklistMiddleware>();
builder.Services.AddScoped<RedisService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<MenuFastContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

builder.Services.AddScoped<RedisService>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MenuFast API v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<JwtBlacklistMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();