using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features.CreateAddress;
using ShopApp.Addresses.Service.Features.UpdateAddress;

namespace ShopApp.Services.Addresses.Tests.TestUtils;

public static class ServiceProviderFactory
{
    public static ServiceProvider Create(string dbName = "TestDb")
    {
        var services = new ServiceCollection();

        // Register your services here
        services.AddDbContext<AddressesDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        services.AddScoped<IValidator<CreateAddressRequest>, CreateAddressValidator>();
        services.AddScoped<IValidator<UpdateAddressRequest>, UpdateAddressValidator>();

        // Add other necessary services

        return services.BuildServiceProvider();
    }
}