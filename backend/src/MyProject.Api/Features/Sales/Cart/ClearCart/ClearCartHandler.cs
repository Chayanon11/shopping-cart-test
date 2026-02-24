using MediatR;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.ClearCart;

public class ClearCartHandler(AppDbContext context) : IRequestHandler<ClearCartCommand, Result>
{
    public async Task<Result> Handle(ClearCartCommand request, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

        if (cart is null)
        {
            return Result.Failure(new Error("Cart.NotFound", "Cart not found"));
        }

        context.CartItems.RemoveRange(cart.Items);
        cart.UpdatedAt = DateTimeOffset.UtcNow;
        
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}
