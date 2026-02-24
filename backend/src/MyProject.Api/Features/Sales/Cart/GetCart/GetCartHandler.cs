using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.GetCart;

public class GetCartHandler(AppDbContext context) : IRequestHandler<GetCartQuery, Result<CartResponse>>
{
    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            return Result.Failure<CartResponse>(new Error("Cart.NotFound", "Cart not found"));
        }

        var items = cart.Items.Select(i => new CartItemResponse(
            i.ProductId,
            i.Product.ProductName,
            i.Product.ProductPrice,
            i.Product.ProductImage,
            i.Quantity,
            i.Quantity * i.Product.ProductPrice
        )).ToList();

        var totalBalance = items.Sum(i => i.TotalPrice);

        return Result.Success(new CartResponse(cart.Id, items, totalBalance));
    }
}
