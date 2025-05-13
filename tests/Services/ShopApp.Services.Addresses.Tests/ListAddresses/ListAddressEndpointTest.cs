using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Addresses.Service;
using ShopApp.Addresses.Service.Database;
using ShopApp.Services.Addresses.Tests.TestUtils;

namespace ShopApp.Services.Addresses.Tests.ListAddresses;

public class ListAddressEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ListAddressEndpointTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task ListAddresses_ShouldReturnListOfAddresses()
    {
        // Arrange
        var serviceProvider = ServiceProviderFactory.Create();
        var dbContext = serviceProvider.GetRequiredService<AddressesDbContext>();

        // Seed the database with test data
        dbContext.Addresses.AddRange(new List<Address>
        {
            new Address { Street = "123 Main St", City = "Springfield", State = "IL", PostalCode = "62701", Country = "USA" },
            new Address { Street = "456 Elm St", City = "Springfield", State = "IL", PostalCode = "62702", Country = "USA" }
        });
        await dbContext.SaveChangesAsync();

        // Act
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(dbContext);
            });
        }).CreateClient();
        
        
        var response = await client.GetAsync("/addresses");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }
}