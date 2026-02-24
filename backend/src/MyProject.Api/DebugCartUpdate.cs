using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Api.Domain;
using MyProject.Api.Infrastructure.Persistence;

namespace MyProject.Api.Tool;

public class DebugCartUpdate 
{
    public static async Task RunAsync(IServiceProvider services, Guid cartId, Guid productId)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Console.WriteLine($"Looking for Cart: {cartId}");
        var cart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
        
        if (cart == null) {
            Console.WriteLine("Cart not found. Creating...");
            cart = new Cart { Id = cartId };
            context.Carts.Add(cart);
            await context.SaveChangesAsync();
            Console.WriteLine("Cart created.");
        }

        var item = new CartItem {
            CartId = cartId,
            ProductId = productId,
            Quantity = 1
        };

        var entry = context.CartItems.Add(item);
        
        Console.WriteLine($"Entity State before save: {entry.State}");

        try {
            await context.SaveChangesAsync();
            Console.WriteLine("Saved successfully!");
        } 
        catch (Exception ex) {
            Console.WriteLine($"Error saving: {ex.Message}");
            if (ex.InnerException != null) {
                 Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
        }
    }
}
