using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MyProject.Api.Features.Sales.Cart.UpdateItem;

public class UpdateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/cart/{cartId:guid}/items/{productId:guid}", async (
            Guid cartId, 
            Guid productId,
            [FromBody] UpdateItemRequest request, 
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateItemCommand(cartId, productId, request.Quantity));
            
            if (result.IsSuccess)
            {
                return Results.Ok(new { CartId = result.Value });
            }

            return result.Error.Code switch
            {
                "Cart.StockWarning" => Results.BadRequest(new { result.Error.Code, result.Error.Message }),
                "Cart.NotFound" => Results.NotFound(new { result.Error.Code, result.Error.Message }),
                "CartItem.NotFound" => Results.NotFound(new { result.Error.Code, result.Error.Message }),
                _ => Results.BadRequest(new { result.Error.Code, result.Error.Message })
            };
        })
        .WithTags("Cart")
        .WithName("UpdateCartItem")
        .WithOpenApi();
    }
}
