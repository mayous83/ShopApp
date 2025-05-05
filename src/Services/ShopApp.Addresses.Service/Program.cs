using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features;
using ShopApp.Addresses.Service.Features.CreateAddress;
using ShopApp.Addresses.Service.Features.GetAddressById;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add dbcontext postgres
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AddressesDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressValidator>();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Addresses Service is running!");
CreateAddressEndpoint.Map(app);
GetAddressByIdEndpoint.Map(app);


app.Run();
