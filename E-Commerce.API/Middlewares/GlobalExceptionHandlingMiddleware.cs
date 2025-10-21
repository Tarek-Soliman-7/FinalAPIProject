
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace E_Commerce.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandleNotFoundApiAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong ==> {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleNotFoundApiAsync(HttpContext context)
        {
            context.Request.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The end point with url {context.Request.Path} not found"
            };
            
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //1] Change statue code
            //context.Response.StatusCode =StatusCodes.Status500InternalServerError;
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                (_) => StatusCodes.Status500InternalServerError
            };
            //2] Change content type
            context.Response.ContentType = "application/json";
            //3] Write response in body
            var response = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            }.ToString();
            await context.Response.WriteAsync(response);

        }
    }
}
