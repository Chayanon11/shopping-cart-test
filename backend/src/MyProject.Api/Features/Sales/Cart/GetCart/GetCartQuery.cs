using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.GetCart;

public record GetCartQuery(Guid CartId) : IRequest<Result<CartResponse>>;
