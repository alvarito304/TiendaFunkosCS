using System.Net;
using System.Text.Json;
using ApiFunkosCS.CategoryNamespace.Errors;
using ApiFunkosCS.FunkoNamespace.Exception;

namespace ApiFunkosCS.Utils.ExceptionMiddleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Definir el código de estado HTTP por defecto
            var statusCode = HttpStatusCode.BadRequest;
            var errorResponse = new { message = "An unexpected error occurred." };

            // Manejar tipos de excepciones personalizadas
            switch (exception)
            {

                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse = new { message = exception.Message };
                    logger.LogWarning(exception, "Invalid operation.");
                    break;
                
                /**************** STORAGE EXCEPTIONS *****************************************/
                case ApiFunkosCS.Storage.Exceptions.FileNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorResponse = new { message = exception.Message };
                    logger.LogWarning(exception, "File not found.");
                    break;
                
                case ApiFunkosCS.Storage.Exceptions.MaxFileSizeStorageException:
                    statusCode = HttpStatusCode.PreconditionFailed;
                    errorResponse = new { message = exception.Message };
                    logger.LogWarning(exception, "File size exceeds maximum allowed size.");
                    break;

                case ApiFunkosCS.Storage.Exceptions.FileExtensionNotAllowedException:
                    statusCode = HttpStatusCode.UnsupportedMediaType;
                    errorResponse = new { message = exception.Message };
                    logger.LogWarning(exception, "File extension is not allowed.");
                    break;

                case Storage.Exceptions.MinFileSizeStorageException:
                    statusCode = HttpStatusCode.PreconditionFailed;
                    errorResponse = new { message = exception.Message };
                    logger.LogWarning(exception, "File size is less than minimum allowed size.");
                    break;
                
                /********************************************************/
                
                /**************** RESULT EXCEPTIONS *****************************************/
                case CSharpFunctionalExtensions.ResultFailureException:
                    statusCode = HttpStatusCode.NotFound;
                    var errorMessage = exception.Message;
                    // Extraer solo la parte del mensaje después del último "The error was:"
                    var lastErrorIndex = errorMessage.LastIndexOf("The error was:");
                    if (lastErrorIndex != -1)
                    {
                        errorMessage = errorMessage.Substring(lastErrorIndex + "The error was:".Length).Trim();
                    }
                    errorResponse = new { message = errorMessage };
                    logger.LogError(exception, "A Result failure occurred: {ErrorMessage}", errorMessage);
                    break;
                /********************************************************/
                
                default:
                    logger.LogError(exception, "An unhandled exception occurred.");
                    break;
            }

            // Configurar la respuesta HTTP
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(jsonResponse);
        }
    }