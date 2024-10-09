

namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageUrl, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            ImageFile = command.ImageUrl,
            Price = command.Price
        };
        
        // store to database
        
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return  new CreateProductResult(product.Id);

    }
}