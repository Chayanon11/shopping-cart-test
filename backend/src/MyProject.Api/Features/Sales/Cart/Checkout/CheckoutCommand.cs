using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Sales.Cart.Checkout;

public record CheckoutCommand(Guid CartId) : IRequest<Result>;
