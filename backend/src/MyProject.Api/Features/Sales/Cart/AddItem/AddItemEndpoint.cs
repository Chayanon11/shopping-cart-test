using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MyProject.Api.Features.Sales.Cart.AddItem;

public class AddItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/cart/{cartId:guid}/items", async (
            Guid cartId, 
            [FromBody] AddItemRequest request, 
            ISender sender) =>
        {
            var result = await sender.Send(new AddItemCommand(cartId, request.ProductId, request.Quantity));
            
            if (result.IsSuccess)
            {
                return Results.Ok(new { CartId = result.Value });
            }

            return result.Error.Code == "Cart.StockWarning"
                ? Results.BadRequest(new { result.Error.Code, result.Error.Message })
                : Results.BadRequest(new { result.Error.Code, result.Error.Message });
        })
        .WithTags("Cart")
        .WithName("AddItemToCart")
        .WithOpenApi();
    }
}
