using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.AddItem;

public record AddItemCommand(Guid CartId, Guid ProductId, int Quantity) : IRequest<Result<Guid>>;
