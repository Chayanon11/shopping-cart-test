using MediatR;
using MyProject.Api.Shared;

namespace MyProject.Api.Features.Catalog.Products.GetProducts;

public record GetProductsQuery() : IRequest<Result<IEnumerable<ProductResponse>>>;
