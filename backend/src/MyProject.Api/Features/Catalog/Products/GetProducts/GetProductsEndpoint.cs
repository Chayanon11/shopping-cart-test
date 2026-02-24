using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Catalog.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            return Results.BadRequest(new { result.Error.Code, result.Error.Message });
        })
        .WithTags("Catalog")
        .WithName("GetProducts")
        .WithOpenApi();
    }
}
