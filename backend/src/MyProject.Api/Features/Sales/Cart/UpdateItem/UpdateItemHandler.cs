using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.UpdateItem;

public class UpdateItemHandler(AppDbContext context) : IRequestHandler<UpdateItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateItemCommand request, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            return Result.Failure<Guid>(new Error("Cart.NotFound", "Cart not found"));
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem is null)
        {
            return Result.Failure<Guid>(new Error("CartItem.NotFound", "Item not found in cart"));
        }

        var productStock = await context.ProductStocks
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProductId == request.ProductId, ct);

        if (productStock is null || request.Quantity > productStock.ProductAmount)
        {
            return Result.Failure<Guid>(new Error("Cart.StockWarning", "Not enough stock available for this quantity."));
        }

        existingItem.Quantity = request.Quantity;
        existingItem.UpdatedAt = DateTimeOffset.UtcNow;
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        
        await context.SaveChangesAsync(ct);

        return Result.Success(cart.Id);
    }
}
