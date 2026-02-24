using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.RemoveItem;

public class RemoveItemHandler(AppDbContext context) : IRequestHandler<RemoveItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RemoveItemCommand request, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            return Result.Failure<Guid>(new Error("Cart.NotFound", "Cart not found"));
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem is not null)
        {
            cart.Items.Remove(existingItem);
            cart.UpdatedAt = DateTimeOffset.UtcNow;
            await context.SaveChangesAsync(ct);
        }

        return Result.Success(cart.Id);
    }
}
