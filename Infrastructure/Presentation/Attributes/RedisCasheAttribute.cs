using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction.Contracts;
using System.Text;

namespace Presentation.Attributes
{
    internal class RedisCasheAttribute(int durationInSeconds = 120 ) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashService = context.HttpContext.RequestServices.GetRequiredService<IServiceManger>().CashService;
            //Data cashed or not ==> Key
            //Key ==> PathUrl + query string
            //context.HttpContext.Request.Path // /api/products
            //context.HttpContext.Request.Query // key , value ==> Sort --> NameDesc , PageSize --> 10
            string key = GenerateKey(context.HttpContext.Request);
            var result = await cashService.GetCashedValueAsync(key);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var resultContext = await next.Invoke();
            if (resultContext.Result is OkObjectResult okObjResult)
            {
                await cashService.SetCasheValueAsync(key, okObjResult.Value, TimeSpan.FromSeconds(durationInSeconds));

            }
        }

        private string GenerateKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(x=>x.Key))
            {
                key.Append($"{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
