using FluentValidation;

namespace MyProject.Api.Features.Sales.Cart.Checkout;

public class CheckoutValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
