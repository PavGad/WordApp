using Serilog;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace WordApp.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception appException)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (appException)
                {
                    case AuthenticationException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case NotImplementedException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentNullException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ArgumentException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case FormatException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                Log.Error(appException, $"Status code: {response.StatusCode}");

                var result = JsonSerializer.Serialize(new { message = appException?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
