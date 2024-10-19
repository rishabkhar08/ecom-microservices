using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool Success);

public class DeleteProductHandlerValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductHandlerValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id cannot be empty");
    }
}

internal class DeleteProductHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(command.ProductId);
        }
        
        session.Delete<Product>(product.Id);
        await session.SaveChangesAsync(cancellationToken);
        
        return new DeleteProductResult(true);
    }
}