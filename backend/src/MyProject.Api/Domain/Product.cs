namespace MyProject.Api.Domain;

public class Product
{
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string? ProductImage { get; set; }
}
