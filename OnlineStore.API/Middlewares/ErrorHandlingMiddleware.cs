using System.Net;
using FluentValidation;
using OnlineStore.API.Errors;
using OnlineStore.Application.Common.Exceptions;

namespace OnlineStore.API.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = ex switch
            {
                // 400 - Validation errors (FluentValidation)
                ValidationException validationEx =>
                    new ApiValidationError(validationEx.Errors.Select(e => e.ErrorMessage)),

                NotFoundException notFoundException =>
                    new ApiError((int)HttpStatusCode.NotFound, notFoundException.Message),
                UnauthorizedException unauthorizedException =>
                    new ApiError((int)HttpStatusCode.Unauthorized, unauthorizedException.Message),
                BadRequestException badRequestEx =>
                     new ApiError((int)HttpStatusCode.BadRequest, badRequestEx.Message),
                _ => new ApiError((int)HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response, response.GetType());
            // response.GetType() =>  tell serilizer serilize the actual type not base type 
        }
    }


}