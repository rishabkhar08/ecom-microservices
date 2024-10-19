namespace CatalogAPI.Products.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{categoryName}", async (string categoryName, ISender sender) =>
        {
            var products = await sender.Send(new GetProductsByCategoryQuery(categoryName));

            var response = products.Adapt<GetProductsByCategoryResponse>();
            
            return Results.Ok(response);
        }).WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Filter Products by Category")
        .WithDescription("Filter products by Category");
    }
}