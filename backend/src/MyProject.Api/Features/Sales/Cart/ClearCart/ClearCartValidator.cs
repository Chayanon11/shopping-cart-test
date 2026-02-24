using FluentValidation;

namespace MyProject.Api.Features.Sales.Cart.ClearCart;

public class ClearCartValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
