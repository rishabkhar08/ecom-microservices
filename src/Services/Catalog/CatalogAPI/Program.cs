var builder = WebApplication.CreateBuilder(args);
// add services to the Container
builder.Services.AddCarter();                                                                 
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(options => 
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

var app = builder.Build();
// configure the HTTP request pipeline

app.MapGet("/", () => "Hello World!");

app.MapCarter();
app.Run();