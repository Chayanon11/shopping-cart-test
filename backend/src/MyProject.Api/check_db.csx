using Microsoft.EntityFrameworkCore;
using MyProject.Api.Infrastructure.Persistence;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;

public class CheckDb {
    public static async System.Threading.Tasks.Task Run(IServiceProvider sp) {
        using var scope = sp.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var items = await ctx.CartItems.ToListAsync();
        Console.WriteLine($"Total CartItems: {items.Count}");
        foreach(var i in items) {
            Console.WriteLine($"CartId: {i.CartId}, Product: {i.ProductId}, Id: {i.Id}, Qty: {i.Quantity}");
        }
    }
}
