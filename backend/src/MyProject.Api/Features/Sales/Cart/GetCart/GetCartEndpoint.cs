using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MyProject.Api.Features.Sales.Cart.GetCart;

public class GetCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/cart/{cartId:guid}", async (Guid cartId, ISender sender) =>
        {
            var result = await sender.Send(new GetCartQuery(cartId));
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            
            return result.Error.Code == "Cart.NotFound"
                ? Results.NotFound(new { result.Error.Code, result.Error.Message })
                : Results.BadRequest(new { result.Error.Code, result.Error.Message });
        })
        .WithTags("Cart")
        .WithName("GetCart")
        .WithOpenApi();
    }
}
