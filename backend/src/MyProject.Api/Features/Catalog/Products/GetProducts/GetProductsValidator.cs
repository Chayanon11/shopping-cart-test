using FluentValidation;

namespace MyProject.Api.Features.Catalog.Products.GetProducts;

public class GetProductsValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsValidator()
    {
    }
}
