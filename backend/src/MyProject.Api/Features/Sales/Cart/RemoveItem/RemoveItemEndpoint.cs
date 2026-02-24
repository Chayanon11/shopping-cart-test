using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MyProject.Api.Features.Sales.Cart.RemoveItem;

public class RemoveItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/cart/{cartId:guid}/items/{productId:guid}", async (
            Guid cartId, 
            Guid productId,
            ISender sender) =>
        {
            var result = await sender.Send(new RemoveItemCommand(cartId, productId));
            
            if (result.IsSuccess)
            {
                return Results.Ok(new { CartId = result.Value });
            }

            return result.Error.Code == "Cart.NotFound"
                ? Results.NotFound(new { result.Error.Code, result.Error.Message })
                : Results.BadRequest(new { result.Error.Code, result.Error.Message });
        })
        .WithTags("Cart")
        .WithName("RemoveCartItem")
        .WithOpenApi();
    }
}
