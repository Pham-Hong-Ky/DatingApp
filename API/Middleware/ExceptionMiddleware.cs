
using System.Net;
using System.Text.Json;
using API.Errors;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        [Obsolete]
        private readonly IHostingEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware( RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostingEnvironment env )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync( HttpContext context )
        {
            try
            {
                await _next( context );
            }
            catch ( Exception ex )
            {
                _logger.LogError( ex, ex.Message );
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ( int ) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException( context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString() )
                    : new ApiException( context.Response.StatusCode, "Internal Server Error" );

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                
                var json = JsonSerializer.Serialize( response, options );

                await context.Response.WriteAsync( json );
            }
        }
    }
}