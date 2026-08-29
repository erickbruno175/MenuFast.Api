using MenuFast.Api.Api.Application.Services.CategoriaServices;
using MenuFast.Api.Api.Application.Services.ClienteServices;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.KdsServices;
using MenuFast.Api.Api.Application.Services.LojaConfiguracoes;
using MenuFast.Api.Api.Application.Services.MesaServices;
using MenuFast.Api.Api.Application.Services.PedidoServices;
using MenuFast.Api.Api.Application.Services.ProdutoServices;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Application.Services.Seguranca;
using MenuFast.Api.Api.Application.Services.Services.OpenRouteService;
using MenuFast.Api.Api.Hubs;
using MenuFast.Api.Api.Middlewares;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.BackgroundServices;
using MenuFast.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using StackExchange.Redis;
using System.Text;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration [ "Jwt:Issuer" ],
            ValidAudience = builder.Configuration [ "Jwt:Audience" ],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration [ "Jwt:Key" ]!
                )
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    sucesso = false,
                    mensagem = "Não autorizado. Faça login para continuar."
                });
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MenuFast API",
        Version = "v1"
    });

    options.TagActionsBy(api =>
    {
        var controller = api.ActionDescriptor.RouteValues [ "controller" ];

        return new [ ]
        {
            controller?.EndsWith("Controller") == true
                ? controller[..^"Controller".Length]
                : controller ?? "Default"
        };
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [ new OpenApiSecuritySchemeReference("Bearer", document) ] = [ ]
        });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");

    options.InstanceName = "MenuFast:";
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UsuarioContextService>();
builder.Services.AddScoped<SegurancaService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RedisService>();
builder.Services.AddScoped<ProdutoServices>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ConfiguracaoSistemaLojaServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MesaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<KdsService>();
builder.Services.AddHttpClient<OpenRouteServices>();
builder.Services.AddHostedService<AlertaEstoqueBackgroundService>();
builder.Services.AddSignalR();
builder.Services.AddDbContext<MenuFastContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "MenuFast API v1");
});
app.MapHub<KdsHub>("/hubs/kds");
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<JwtBlacklistMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
