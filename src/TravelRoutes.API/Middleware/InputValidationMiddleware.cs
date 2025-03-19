using System.Text.RegularExpressions;

namespace TravelRoutes.API.Middleware
{
    public class InputValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InputValidationMiddleware> _logger;
        private static readonly Regex SqlInjectionPattern = new(
            @"(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE)?|INSERT( +INTO)?|MERGE|SELECT|UPDATE|UNION( +ALL)?)\b)|[;]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public InputValidationMiddleware(RequestDelegate next, ILogger<InputValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                
                foreach (var param in context.Request.Query)
                {
                    if (param.Value.ToString() is string value && ContainsSqlInjection(value))
                    {
                        _logger.LogWarning("Possível tentativa de SQL injection detectada em parâmetro de query string: {Param}", param.Key);
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { error = "Entrada inválida detectada" });
                        return;
                    }
                }

              
                if (context.Request.Method != "GET" && context.Request.Method != "DELETE")
                {
                    context.Request.EnableBuffering();
                    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    if (ContainsSqlInjection(body))
                    {
                        _logger.LogWarning("Possível tentativa de SQL injection detectada no corpo da requisição");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { error = "Entrada inválida detectada" });
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar entrada");
                throw;
            }
        }

        private static bool ContainsSqlInjection(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return SqlInjectionPattern.IsMatch(input);
        }
    }

    public static class InputValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseInputValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InputValidationMiddleware>();
        }
    }
} 