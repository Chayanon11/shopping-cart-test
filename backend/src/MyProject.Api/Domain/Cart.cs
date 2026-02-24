namespace MyProject.Api.Domain;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
