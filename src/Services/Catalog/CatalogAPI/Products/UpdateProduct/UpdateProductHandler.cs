using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageUrl,
    decimal Price)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess = false);

public class UpdateProductHandlerValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50).WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
    }
}

internal class UpdateProductHandler(IDocumentSession session)
    : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        //logger.LogInformation("Handling update product command {command}", command);
        var product  = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        product.Name = command.Name;
        product.Description = command.Description;
        product.Category = command.Category;
        product.ImageFile = command.ImageUrl;
        product.Price = command.Price;
        
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
        
    }
}