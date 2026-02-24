using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Catalog.Products.GetProducts;

public class GetProductsHandler(
    AppDbContext context,
    ILogger<GetProductsHandler> logger
) : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductResponse>>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> Handle(
        GetProductsQuery request, CancellationToken ct)
    {
        var products = await context.Products
            .AsNoTracking()
            .Join(
                context.ProductStocks.AsNoTracking(),
                p => p.ProductId,
                s => s.ProductId,
                (p, s) => new ProductResponse(
                    p.ProductId,
                    p.ProductName,
                    p.ProductPrice,
                    p.ProductImage,
                    s.ProductAmount)
            )
            .ToListAsync(ct);

        return Result.Success<IEnumerable<ProductResponse>>(products);
    }
}
