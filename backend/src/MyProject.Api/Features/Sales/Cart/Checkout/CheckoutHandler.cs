using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.Checkout;

public class CheckoutHandler(AppDbContext context) : IRequestHandler<CheckoutCommand, Result>
{
    public async Task<Result> Handle(CheckoutCommand request, CancellationToken ct)
    {
        // 1. Get Cart
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            return Result.Failure(new Error("Cart.NotFound", "Cart not found"));
        }

        if (!cart.Items.Any())
        {
            return Result.Failure(new Error("Cart.Empty", "Cannot checkout an empty cart"));
        }

        // 2. Wrap in a transaction to ensure valid stock deductions
        using var transaction = await context.Database.BeginTransactionAsync(ct);

        try
        {
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            
            // Get all needed stock entries at once, tracking changes
            var stocks = await context.ProductStocks
                .Where(s => productIds.Contains(s.ProductId))
                .ToDictionaryAsync(s => s.ProductId, ct);

            // 3. Validate and Deduct
            foreach (var item in cart.Items)
            {
                if (!stocks.TryGetValue(item.ProductId, out var stock))
                {
                    return Result.Failure(new Error("Product.NotFound", $"Product {item.ProductId} not found."));
                }

                if (item.Quantity > stock.ProductAmount)
                {
                    return Result.Failure(new Error("Cart.StockWarning", $"Not enough stock for {item.ProductId}"));
                }

                // Deduct stock
                stock.ProductAmount -= item.Quantity;
            }

            // 4. Clear Cart
            context.CartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure(new Error("Checkout.Failed", "An error occurred during checkout."));
        }
    }
}
