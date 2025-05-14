using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApp.Products.Service.Database;
using ShopApp.Products.Service.Features.CreateProduct;
using ShopApp.Products.Service.Features.DecrementProductStockQuantity;
using ShopApp.Products.Service.Features.DeleteProduct;
using ShopApp.Products.Service.Features.GetProductById;
using ShopApp.Products.Service.Features.ListProducts;
using ShopApp.Products.Service.Features.ReserveStockRequest;
using ShopApp.Shared.Library.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

var rabbitMqHost = builder.Configuration["RabbitMq:Host"];
ArgumentNullException.ThrowIfNull(rabbitMqHost);

builder.Services.AddSingleton<IMessageBus>(sp =>
    new RabbitMqMessageBus(rabbitMqHost));

builder.Services.Scan(scan => scan
    .FromAssemblyOf<OrderCreatedConsumer>()
    .AddClasses(classes => classes.AssignableTo(typeof(IEventConsumer<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddHostedService<MessageBusHostedService>();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateProductEndpoint.Map(app);
GetProductByIdEndpoint.Map(app);
ReserveStockEndpoint.Map(app);
ListProductsEndpoint.Map(app);

app.MapGet("/", () => "Products Service is running!");

app.Run();
