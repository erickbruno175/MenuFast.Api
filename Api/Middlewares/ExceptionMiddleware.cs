using System.Text.Json;

namespace MenuFast.Api.Middlewares;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try
        {
            await _next(context);
        }
        catch(BusinessLogicException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            if(context.Response.HasStarted)
            {
                _logger.LogWarning("A resposta já havia iniciado. Não foi possível alterar o StatusCode.");
                throw;
            }

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                sucesso = false,
                mensagem = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch(UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            if(context.Response.HasStarted)
            {
                _logger.LogWarning("A resposta já havia iniciado. Não foi possível alterar o StatusCode.");
                throw;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                sucesso = false,
                mensagem = "Usuário não autorizado."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Erro interno no servidor. Data geração: {Data}", DateTime.Now);

            if(context.Response.HasStarted)
            {
                _logger.LogWarning("A resposta já havia iniciado. Não foi possível alterar o StatusCode.");
                throw;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                sucesso = false,
                mensagem = "Ocorreu um erro interno no servidor."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}