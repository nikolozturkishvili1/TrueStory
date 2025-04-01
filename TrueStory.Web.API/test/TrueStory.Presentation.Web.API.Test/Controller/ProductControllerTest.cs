using FluentAssertions;
using MediatR;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueStory.Application.Product.Commands.Create;
using TrueStory.Application.Product.Commands.Delete;
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Presentation.Web.API.Test.TestData;
using TrueStory.Web.API.Controllers;

namespace TrueStory.Presentation.Web.API.Test.Controller
{
    public class ProductControllerTest
    {
        private readonly IMediator _mediator;
        private readonly ProductController _ProductController;

        public ProductControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _ProductController = new ProductController(_mediator);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnPagedResult_WhenCalled()
        {
            // Arrange
            var getProductsModelRequest = new GetProductsModelRequest();
            var pagedProductsResponse = ProductTestData.BuildPagedProducts();

            _mediator.Send(Arg.Any<GetProductsQuery>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(pagedProductsResponse));

            // Act
            var result = await _ProductController.GetProducts(getProductsModelRequest, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(pagedProductsResponse);
            await _mediator.Received(1).Send(Arg.Any<GetProductsQuery>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task PostProduct_ShouldReturnProductId_WhenValidRequestProvided()
        {
            // Arrange
            var createProductRequest = ProductTestData.BuildCreateProductRequest(ProductTestData.DEFAULT_NAME);
            var expectedProductId = ProductTestData.Product_ID;

            _mediator.Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(expectedProductId));

            // Act
            var result = await _ProductController.PostProduct(createProductRequest, CancellationToken.None);

            // Assert
            result.Should().Be(expectedProductId);
            await _mediator.Received(1).Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteProduct_ShouldCallMediator_WhenValidRequestProvided()
        {
            // Arrange
            var deleteProductRequest = ProductTestData.BuildDeleteProductRequest(ProductTestData.Product_ID);

            // Act
            await _ProductController.DeleteProduct(deleteProductRequest, CancellationToken.None);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>());
        }
    }
}
