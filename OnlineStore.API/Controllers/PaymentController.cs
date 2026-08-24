using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Errors;
using OnlineStore.Application.Features.Payments.Commands.CreatePaymentIntent;
using OnlineStore.Application.Features.Payments.Commands.HandleWebhook;
using OnlineStore.Application.Features.Payments.Dtos;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a payment intent for an order.
    /// </summary>
    [Authorize]
    [HttpPost("create-intent/{orderId}")]
    public async Task<ActionResult<PaymentIntentDto>> CreatePaymentIntent(
        int orderId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreatePaymentIntentCommand { OrderId = orderId },
            cancellationToken);

        return result;
    }

    /// <summary>
    /// Handles payment events sent by Stripe.
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook(
        CancellationToken cancellationToken)
    {
        var payload = await new StreamReader(Request.Body)
            .ReadToEndAsync(cancellationToken);

        var signature = Request.Headers["Stripe-Signature"].ToString();

        try
        {
            await mediator.Send(
                new HandleWebhookCommand
                {
                    Payload = payload,
                    Signature = signature
                },
                cancellationToken);

            return Ok(new { received = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError(400, ex.Message));
        }
    }
}