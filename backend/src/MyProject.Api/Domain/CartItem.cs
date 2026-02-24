namespace MyProject.Api.Domain;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
