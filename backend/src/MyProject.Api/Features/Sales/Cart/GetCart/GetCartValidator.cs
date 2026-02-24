using FluentValidation;

namespace MyProject.Api.Features.Sales.Cart.GetCart;

public class GetCartValidator : AbstractValidator<GetCartQuery>
{
    public GetCartValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
