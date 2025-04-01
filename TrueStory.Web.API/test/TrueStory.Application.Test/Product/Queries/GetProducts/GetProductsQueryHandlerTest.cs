using Moq;
using FluentAssertions;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Common.Application.Paging;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Presentation.Web.API.Test.TestData;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TrueStory.Application.Test.Product.Queries.GetProducts;

/// <summary>
/// Unit tests for <see cref="GetProductsQueryHandler"/>.
/// </summary>
public class GetProductsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IRestFulServiceImplementator> _serviceMock;
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _serviceMock = new Mock<IRestFulServiceImplementator>();

        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
        _handler = new GetProductsQueryHandler(_unitOfWorkMock.Object, _serviceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedProducts_WhenValidQueryProvided()
    {
        // Arrange
        var query = new GetProductsQuery(ProductTestData.DEFAULT_NAME, ProductTestData.PAGE_SIZE, ProductTestData.PAGE_NUMBER);
        var products = new List<T_Product>
        {
            T_Product.Create(ProductTestData.Product_ID, ProductTestData.DEFAULT_NAME)
        };

        var productResponses = new List<ProductResponseDTO>
        {
            new ProductResponseDTO
            {
                ID = ProductTestData.Product_ID,
                Name = ProductTestData.DEFAULT_NAME,
                Data = null
            }
        };

        var mockDbSet = new Mock<IQueryable<T_Product>>();
        mockDbSet.As<IAsyncEnumerable<T_Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T_Product>(products.GetEnumerator()));

        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T_Product>(products.AsQueryable().Provider));
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.GetEnumerator()).Returns(products.AsQueryable().GetEnumerator());

        _productRepositoryMock
            .Setup(repo => repo.Query())
            .Returns(mockDbSet.Object);

        _serviceMock
            .Setup(s => s.GetProductsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productResponses);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().ID.Should().Be(ProductTestData.Product_ID);
        result.Items.First().Name.Should().Be(ProductTestData.DEFAULT_NAME);
        result.Items.First().Data.Should().Be(null);

        _productRepositoryMock.Verify(repo => repo.Query(), Times.Once);
        _serviceMock.Verify(s => s.GetProductsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var query = new GetProductsQuery(ProductTestData.DEFAULT_NAME, ProductTestData.PAGE_SIZE, ProductTestData.PAGE_NUMBER);
        var products = new List<T_Product>
        {
            T_Product.Create(ProductTestData.Product_ID, ProductTestData.DEFAULT_NAME)
        };

        var mockDbSet = new Mock<IQueryable<T_Product>>();
        mockDbSet.As<IAsyncEnumerable<T_Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T_Product>(products.GetEnumerator()));

        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T_Product>(products.AsQueryable().Provider));
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.GetEnumerator()).Returns(products.AsQueryable().GetEnumerator());

        _productRepositoryMock
            .Setup(repo => repo.Query())
            .Returns(mockDbSet.Object);

        _serviceMock
            .Setup(s => s.GetProductsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service failed"));

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Service failed");

        _productRepositoryMock.Verify(repo => repo.Query(), Times.Once);
        _serviceMock.Verify(s => s.GetProductsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFilterProductsByName()
    {
        // Arrange
        var searchName = "Test";
        var query = new GetProductsQuery(searchName, ProductTestData.PAGE_SIZE, ProductTestData.PAGE_NUMBER);
        var products = new List<T_Product>
        {
            T_Product.Create(ProductTestData.Product_ID, "Test Product"),
            T_Product.Create(Guid.NewGuid(), "Different Name")
        };

        var mockDbSet = new Mock<IQueryable<T_Product>>();
        mockDbSet.As<IAsyncEnumerable<T_Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T_Product>(products.GetEnumerator()));

        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T_Product>(products.AsQueryable().Provider));
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<T_Product>>()
            .Setup(m => m.GetEnumerator()).Returns(products.AsQueryable().GetEnumerator());

        _productRepositoryMock
            .Setup(repo => repo.Query())
            .Returns(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("Test Product");

        _productRepositoryMock.Verify(repo => repo.Query(), Times.Once);
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncEnumerable<TEntity>, IQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<TEntity>(_inner.Execute<IEnumerable<TEntity>>(null).GetEnumerator());
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return new ValueTask();
    }
} 