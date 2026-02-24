namespace MyProject.Api.Features.Sales.Cart.AddItem;

public record AddItemRequest(Guid ProductId, int Quantity);
