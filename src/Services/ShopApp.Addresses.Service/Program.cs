using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features.CreateAddress;
using ShopApp.Addresses.Service.Features.DeleteAddress;
using ShopApp.Addresses.Service.Features.GetAddressById;
using ShopApp.Addresses.Service.Features.ListAddresses;

namespace ShopApp.Addresses.Service;

public class Program
{
    public static void Main(string[] args)
    {
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
        ListAddressesEndpoint.Map(app);
        DeleteAddressEndopoint.Map(app);

        app.Run();
    }
}