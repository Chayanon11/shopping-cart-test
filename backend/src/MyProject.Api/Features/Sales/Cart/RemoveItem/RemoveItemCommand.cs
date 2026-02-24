using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.RemoveItem;

public record RemoveItemCommand(Guid CartId, Guid ProductId) : IRequest<Result<Guid>>;
