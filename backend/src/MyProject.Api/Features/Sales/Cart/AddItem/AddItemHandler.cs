using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Domain;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.AddItem;

public class AddItemHandler(AppDbContext context) : IRequestHandler<AddItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddItemCommand request, CancellationToken ct)
    {
        var productStock = await context.ProductStocks
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProductId == request.ProductId, ct);

        if (productStock is null)
        {
            return Result.Failure<Guid>(new Error("Product.NotFound", "ไม่พบรายการสินค้า"));
        }

        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            cart = new Domain.Cart { Id = request.CartId };
            context.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        var newTotalQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

        if (newTotalQuantity > productStock.ProductAmount)
        {
            return Result.Failure<Guid>(new Error("Cart.StockWarning", "สินค้าไม่เพียงพอ"));
        }

        if (existingItem is not null)
        {
            existingItem.Quantity = newTotalQuantity;
            existingItem.UpdatedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            context.CartItems.Add(newItem);
        }
        
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync(ct);

        return Result.Success(cart.Id);
    }
}
