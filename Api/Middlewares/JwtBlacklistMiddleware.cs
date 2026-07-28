using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;


namespace MenuFast.Api.Api.Middlewares {
    public sealed class JwtBlacklistMiddleware {
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDistributedCache redisCache) {
            var authorization = context.Request.Headers.Authorization.ToString();

            if(!string.IsNullOrWhiteSpace(authorization) &&
                authorization.StartsWith("Bearer "))
            {
                var token = authorization [ "Bearer ".Length.. ].Trim();

                var existe = await redisCache.GetStringAsync($"blacklist:{token}");

                if(existe != null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token inválido.");
                    return;
                }
            }

            await _next(context);
        }
