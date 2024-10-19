

namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageUrl, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductHandlerValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductHandlerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50).WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
    }
}

internal class CreateProductHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // // validate command
        // var result = await validator.ValidateAsync(command, cancellationToken);
        // var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        // if (errors.Any())
        // {
        //     throw new ValidationException(errors.FirstOrDefault());
        // }
        
        // Business logic to create a product
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageUrl,
            Price = command.Price
        };
        
        // store to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return  new CreateProductResult(product.Id);

    }
}