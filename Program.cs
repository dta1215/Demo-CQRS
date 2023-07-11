using Demo.Application.Configuration.Notifications.Product;
using Demo.Application.Products.CreateProduct;
using Demo.Application.Products.GetProducts;
using Demo.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddDbContext<DemoDBContext>(options =>
{
    options.UseInMemoryDatabase("DemoCQRSContext");
});


//Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<IRequestHandler<GetProductsQuery, IEnumerable<ProductDTO>>, GetProductsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<CreateProductCommand, int>, CreateProductCommandHandler>();

builder.Services.AddScoped<INotificationHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();


//Configure

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
