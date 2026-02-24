namespace MyProject.Api.Domain;

public class ProductStock
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int ProductAmount { get; set; }
}
