namespace MyProject.Api.Features.Sales.Cart.GetCart;

public record CartResponse(Guid Id, IEnumerable<CartItemResponse> Items, decimal TotalBalance);

public record CartItemResponse(Guid ProductId, string ProductName, decimal ProductPrice, string? ProductImage, int Quantity, decimal TotalPrice);
