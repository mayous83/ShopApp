using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Addresses.Service;
using ShopApp.Addresses.Service.Database;
using ShopApp.Services.Addresses.Tests.TestUtils;

namespace ShopApp.Services.Addresses.Tests.GetAddressById;

public class GetAddressByIdEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public GetAddressByIdEndpointTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Get_ShouldReturnOk_WhenAddressExists()
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
        
        var response = await client.GetAsync($"/addresses/{addressId}");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}