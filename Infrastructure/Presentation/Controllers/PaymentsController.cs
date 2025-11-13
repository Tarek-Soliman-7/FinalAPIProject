using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManger _serviceManger) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrdUpdatePaymentIntentAsync(string basketId)
            => Ok(await _serviceManger.paymentService.CreateOrUpdatePaymentIntentAsync(basketId));

        [HttpPost("WebHook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            await _serviceManger.paymentService.UpdatePaymentStatusAsync(json, signatureHeader);
            return new EmptyResult();
        }
    }
}
