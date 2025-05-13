using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShopApp.Addresses.Service.Database;
using ShopApp.Services.Addresses.Tests.TestUtils;
using Program = ShopApp.Addresses.Service.Program;

namespace ShopApp.Services.Addresses.Tests.DeleteAddress;

public class DeleteAddressEnpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeleteAddressEnpointTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenAddressExists()
    {
        var serviceProvider = ServiceProviderFactory.Create();
        
        var addressId = Guid.NewGuid();
        using var serviceProviderScope = serviceProvider.CreateScope();
        var dbContext = serviceProviderScope.ServiceProvider.GetRequiredService<AddressesDbContext>();

        dbContext.Add(new Address
        {
            EntityId = addressId,
            Street = "Piazza Bra",
            City = "Verona",
            State = "VR",
            PostalCode = "37100",
            Country = "Italy",
        });
        
        await dbContext.SaveChangesAsync();
       
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(dbContext);
            });
        }).CreateClient();
        
        var response = await client.DeleteAsync($"/addresses/{addressId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenAddressDoesNotExist()
    {
        var serviceProvider = ServiceProviderFactory.Create();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(serviceProvider);
            });
        }).CreateClient();
        
        var addressId = Guid.NewGuid();
        var response = await client.DeleteAsync($"/addresses/{addressId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}