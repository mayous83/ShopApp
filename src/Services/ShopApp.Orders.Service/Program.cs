using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database;
using ShopApp.Orders.Service.Features.CreateOrder;
using ShopApp.Shared.Library.Messaging;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

var usersServiceUrl = builder.Configuration["Services:Users"];
var productsServiceUrl = builder.Configuration["Services:Products"];
var addressesServiceUrl = builder.Configuration["Services:Addresses"];

//validate the configuration
if (string.IsNullOrEmpty(usersServiceUrl))
{
    throw new ApplicationException("Users service URL is not configured.");
}

if (string.IsNullOrEmpty(productsServiceUrl))
{
    throw new ApplicationException("Products service URL is not configured.");
}

if (string.IsNullOrEmpty(addressesServiceUrl))
{
    throw new ApplicationException("Addresses service URL is not configured.");
}

// Add HttpClients with named keys and config
builder.Services.AddHttpClient("UsersService", client =>
{
    client.BaseAddress = new Uri(usersServiceUrl);
});

builder.Services.AddHttpClient("ProductsService", client =>
{
    client.BaseAddress = new Uri(productsServiceUrl);
});

builder.Services.AddHttpClient("AddressesService", client =>
{
    client.BaseAddress = new Uri(addressesServiceUrl);
});

var rabbitHost = builder.Configuration["RabbitMq:Host"];

if (string.IsNullOrEmpty(rabbitHost))
{
    throw new ApplicationException("RabbitMQ host is not configured.");
}

builder.Services.AddSingleton<IMessageBus>(sp =>
    new RabbitMqMessageBus(rabbitHost));

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateOrderEndpoint.Map(app);
app.MapGet("/", () => "Orders Service is running!");
app.Run();
