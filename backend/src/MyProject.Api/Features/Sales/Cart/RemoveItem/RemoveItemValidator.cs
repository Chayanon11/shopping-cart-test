using FluentValidation;

namespace MyProject.Api.Features.Sales.Cart.RemoveItem;

public class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
{
    public RemoveItemValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
