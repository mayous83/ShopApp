using System.Net;
using System.Net.Http.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features.CreateAddress;
using ShopApp.Services.Addresses.Tests.TestUtils;
using Program = ShopApp.Addresses.Service.Program;

namespace ShopApp.Services.Addresses.Tests.CreateAddresses;

public class CreateAddressEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateAddressEndpointTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenRequestIsValid()
    {
        var serviceProvider = ServiceProviderFactory.Create();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(serviceProvider);
            });
        }).CreateClient();
        
        var request = new CreateAddressRequest
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "CA",
            PostalCode = "12345",
            Country = "USA",
        };
        
        var response = await client.PostAsJsonAsync("/addresses", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnValidationProblem_WhenRequestIsInvalid()
    {
        var serviceProvider = ServiceProviderFactory.Create();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(serviceProvider);
            });
        }).CreateClient();  
        
        var request = new CreateAddressRequest
        {
            Street = "",
            City = "Anytown",
            State = "CA",
            PostalCode = "12345",
            Country = "USA",
        };
        
        var response = await client.PostAsJsonAsync("/addresses", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}