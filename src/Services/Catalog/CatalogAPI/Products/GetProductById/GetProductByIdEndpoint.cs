namespace CatalogAPI.Products.GetProductById;

//public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var product = await sender.Send(new GetProductByIdQuery(id));

            var response = product.Adapt<GetProductByIdResponse>();
            
            return Results.Ok(response);

        }).WithName("GetProductsById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product by Id")
        .WithDescription("Get Product by Id");
    }
}