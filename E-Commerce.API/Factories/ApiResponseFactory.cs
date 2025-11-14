using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //context ==> errors , keys [Field]
            //context.ModelState ==> <string,ModelStateEntity>
            //string ==> name of the field
            //ModelStateEntity ==> Errors ==> Error Messages
            // IEnumerable<ValidationError>
            var errors = context.ModelState.
                Where(error => error.Value?.Errors.Any() == true).Select(error => new ValidationError()
                {
                    Field = error.Key,
                    Errors = error.Value?.Errors.Select(error => error.ErrorMessage) ?? new List<string>()
                });
            var response = new ValidationErrorResponse()
            {
                Errors = errors,
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One or more validation error happened"
            };
            
            return new BadRequestObjectResult(response);
        }
    }
}
