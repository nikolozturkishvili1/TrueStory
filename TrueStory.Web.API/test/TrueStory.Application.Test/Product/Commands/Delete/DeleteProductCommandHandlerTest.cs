using Moq;
using FluentAssertions;
using TrueStory.Application.Product.Commands.Delete;
using TrueStory.Common.Exceptions;
using TrueStory.Domain.Interfaces;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Application.Resources;
using MediatR;
using NSubstitute;

namespace TrueStory.Application.Test.Product.Commands.Delete;

/// <summary>
/// Unit tests for <see cref="DeleteProductCommandHandler"/>.
/// </summary>
public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IRestFulServiceImplementator> _serviceMock;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _serviceMock = new Mock<IRestFulServiceImplementator>();

        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
        _handler = new DeleteProductCommandHandler(_unitOfWorkMock.Object, _serviceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync((T_Product)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*NotFound*");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());
        var product = T_Product.Create(command.Id, "Test Product");

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(product);

        _serviceMock
            .Setup(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service failed"));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Service failed");

        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _serviceMock.Verify(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldDeleteProduct_WhenValidCommandProvided()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());
        var product = T_Product.Create(command.Id, "Test Product");

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(product);

        _serviceMock
            .Setup(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _productRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<T_Product>()))
            .Verifiable();

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _serviceMock.Verify(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.Delete(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSaveChangesFails()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());
        var product = T_Product.Create(command.Id, "Test Product");

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(product);

        _serviceMock
            .Setup(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _productRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<T_Product>()))
            .Verifiable();

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Save failed"));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Save failed");

        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _serviceMock.Verify(s => s.DeleteProductAsync(product.ID, It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.Delete(product), Times.Once);
    }
} 