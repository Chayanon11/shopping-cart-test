using FluentValidation;

namespace MyProject.Api.Features.Sales.Cart.AddItem;

public class AddItemValidator : AbstractValidator<AddItemCommand>
{
    public AddItemValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
