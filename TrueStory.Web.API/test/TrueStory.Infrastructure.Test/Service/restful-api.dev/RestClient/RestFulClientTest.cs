using Moq;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.DependencyInjection;
using TrueStory.Common.Exceptions;
using TrueStory.Infrastructure.Service.restful_api.dev.Model;
using TrueStory.Infrastructure.Service.restful_api.dev.RestClient;
using TrueStory.Presentation.Web.API.Test.TestData;
using System.Text.Json;

namespace TrueStory.Infrastructure.Test.Service.restful_api.dev.RestClient;

/// <summary>
/// Unit tests for <see cref="RestFulClient"/>.
/// </summary>
public class RestFulClientTests : IDisposable
{
    private readonly HttpTest _httpTest;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly IRestFulRestClient _client;

    public RestFulClientTests()
    {
        _httpTest = new HttpTest();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _client = new RestFulClient(_httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnProductId_WhenSuccessful()
    {
        // Arrange
        var request = new AddProductRequestModel 
        { 
            name = ProductTestData.DEFAULT_NAME,
            data = ProductTestData.DEFAULT_DATA
        };
        var expectedResponse = new { id = ProductTestData.Product_ID.ToString() };

        _httpTest.RespondWithJson(expectedResponse, 200);

        // Act
        var result = await _client.AddProductAsync(request, CancellationToken.None);

        // Assert
        result.Should().Be(ProductTestData.Product_ID);
        _httpTest.ShouldHaveCalled("*/objects")
            .WithVerb(HttpMethod.Post)
            .Times(1);
    }

    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var request = new AddProductRequestModel 
        { 
            name = ProductTestData.DEFAULT_NAME,
            data = ProductTestData.DEFAULT_DATA
        };

        _httpTest.RespondWith("Bad Request", 400);

        // Act
        var act = async () => await _client.AddProductAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        _httpTest.ShouldHaveCalled("*/objects")
            .WithVerb(HttpMethod.Post)
            .Times(1);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var productId = ProductTestData.Product_ID;
        _httpTest.RespondWith("", 200);

        // Act
        var result = await _client.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _httpTest.ShouldHaveCalled("*/objects/*")
            .WithVerb(HttpMethod.Delete)
            .Times(1);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var productId = ProductTestData.Product_ID;
        _httpTest.RespondWith("", 404);

        // Act
        var act = async () => await _client.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{productId}*");
        _httpTest.ShouldHaveCalled("*/objects/*")
            .WithVerb(HttpMethod.Delete)
            .Times(1);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowException_WhenServiceError()
    {
        // Arrange
        var productId = ProductTestData.Product_ID;
        _httpTest.RespondWith("", 500);

        // Act
        var act = async () => await _client.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _httpTest.ShouldHaveCalled("*/objects/*")
            .WithVerb(HttpMethod.Delete)
            .Times(1);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProducts_WhenSuccessful()
    {
        // Arrange
        var productIds = new List<Guid> { ProductTestData.Product_ID };
        var expectedResponse = new List<ProductResponse>
        {
            new ProductResponse(
                ProductTestData.Product_ID.ToString(),
                ProductTestData.DEFAULT_NAME,
                null
            )
        };

        _httpTest.RespondWithJson(expectedResponse, 200);

        // Act
        var result = await _client.GetProductsAsync(productIds, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().ID.Should().Be(ProductTestData.Product_ID.ToString());
        result.First().Name.Should().Be(ProductTestData.DEFAULT_NAME);
        result.First().Data.Should().Be(null);
        _httpTest.ShouldHaveCalled("*/objects")
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var productIds = new List<Guid> { ProductTestData.Product_ID };
        var errorResponse = new { error = "Server Error", message = "An error occurred" };
        _httpTest.RespondWithJson(errorResponse, 500);

        // Act
        var act = async () => await _client.GetProductsAsync(productIds, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<FlurlHttpException>();
        exception.Which.StatusCode.Should().Be(500);
        _httpTest.ShouldHaveCalled("*/objects")
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }

    public void Dispose()
    {
        _httpTest.Dispose();
    }
} 