using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.UpdateItem;

public record UpdateItemCommand(Guid CartId, Guid ProductId, int Quantity) : IRequest<Result<Guid>>;
