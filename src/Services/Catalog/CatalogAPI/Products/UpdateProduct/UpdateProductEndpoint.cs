namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProductResponse(bool isSuccess);
public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageUrl, decimal Price);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            
            return Results.Ok(response);

        }).WithName("UpdateProductEndpoint")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Product updated Successfully")
        .WithDescription("Update successful");
    }
}