using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
// JWT
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UsuarioContextService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<MenuFastContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
{
    return ConnectionMultiplexer.Connect(
        "localhost:6379"
    );
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<RedisService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

