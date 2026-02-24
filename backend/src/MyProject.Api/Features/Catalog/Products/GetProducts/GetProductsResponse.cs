namespace MyProject.Api.Features.Catalog.Products.GetProducts;

public record ProductResponse(Guid Id, string Name, decimal Price, string? Image, int StockQuantity);
