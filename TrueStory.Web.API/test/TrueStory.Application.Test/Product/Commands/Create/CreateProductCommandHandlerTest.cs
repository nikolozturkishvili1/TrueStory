using Moq;
using FluentAssertions;
using TrueStory.Application.Product.Commands.Create;
using TrueStory.Common.Exceptions;
using TrueStory.Domain.Interfaces;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using System.Text.Json;
using TrueStory.Application.Resources;
using System.Linq.Expressions;
using FluentValidation;

namespace TrueStory.Application.Test.Product.Commands.Create;

/// <summary>
/// Unit tests for <see cref="CreateProductCommandHandler"/>.
/// </summary>
public class CreateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IRestFulServiceImplementator> _serviceMock;
    private readonly CreateProductCommandHandler _handler;
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _serviceMock = new Mock<IRestFulServiceImplementator>();
        _validator = new CreateProductCommandValidator();

        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
        _handler = new CreateProductCommandHandler(_unitOfWorkMock.Object, _serviceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistsException_WhenProductNameAlreadyExists()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", JsonSerializer.SerializeToElement(new Dictionary<string, object> { { "price", 100 } }));

        _productRepositoryMock
            .Setup(repo => repo.AnyAsync(x => x.Name == command.Name.Trim() && !x.IsDeleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistsException>()
            .WithMessage(string.Format("*RecordAlreadyExists*", command.Name));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", JsonSerializer.SerializeToElement(new Dictionary<string, object> { { "price", 100 } }));

        _productRepositoryMock
            .Setup(repo => repo.AnyAsync(x => x.Name == command.Name.Trim() && !x.IsDeleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _serviceMock
            .Setup(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service failed"));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Service failed");

        _productRepositoryMock.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<T_Product, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct_WhenValidCommandProvided()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", JsonSerializer.SerializeToElement(new Dictionary<string, object> { { "price", 100 } }));
        var expectedProductId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(repo => repo.AnyAsync(x => x.Name == command.Name.Trim() && !x.IsDeleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _serviceMock
            .Setup(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProductId);

        _productRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedProductId);

        _productRepositoryMock.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<T_Product, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldThrowArgumentException_WhenProductNameIsInvalid(string invalidName)
    {
        // Arrange
        var command = new CreateProductCommand(invalidName, JsonSerializer.SerializeToElement(new Dictionary<string, object> {  }));

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct_WithEmptyData()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", JsonSerializer.SerializeToElement(new Dictionary<string, object>()));
        var expectedProductId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(repo => repo.AnyAsync(x => x.Name == command.Name.Trim() && !x.IsDeleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _serviceMock
            .Setup(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProductId);

        _productRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedProductId);

        _productRepositoryMock.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<T_Product, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSaveChangesFails()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", JsonSerializer.SerializeToElement(new Dictionary<string, object> { { "price", 100 } }));
        var expectedProductId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(repo => repo.AnyAsync(x => x.Name == command.Name.Trim() && !x.IsDeleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _serviceMock
            .Setup(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProductId);

        _productRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Save failed"));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Save failed");

        _productRepositoryMock.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<T_Product, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(s => s.AddProductAsync(It.IsAny<AddProductRequestDTO>(), It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<T_Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
} 