using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.ClearCart;

public record ClearCartCommand(Guid CartId) : IRequest<Result>;
