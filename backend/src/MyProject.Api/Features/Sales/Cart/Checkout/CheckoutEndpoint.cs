using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MyProject.Api.Features.Sales.Cart.Checkout;

public class CheckoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/cart/{cartId:guid}/checkout", async (
            Guid cartId, 
            ISender sender) =>
        {
            var result = await sender.Send(new CheckoutCommand(cartId));
            
            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return result.Error.Code switch
            {
                "Cart.StockWarning" => Results.BadRequest(new { result.Error.Code, result.Error.Message }),
                "Cart.Empty" => Results.BadRequest(new { result.Error.Code, result.Error.Message }),
                "Cart.NotFound" => Results.NotFound(new { result.Error.Code, result.Error.Message }),
                _ => Results.BadRequest(new { result.Error.Code, result.Error.Message })
            };
        })
        .WithTags("Cart")
        .WithName("Checkout")
        .WithOpenApi();
    }
}
