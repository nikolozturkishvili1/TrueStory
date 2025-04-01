using FluentAssertions;
using TrueStory.Domain.Aggregate.Product;

namespace TrueStory.Domain.Test.Aggregates.Product
{
    public class ProductTest
    {
        [Fact]
        public void Create_ShouldCreateProduct_WhenValidDataProvided()
        {
            // Act
            var product = T_Product.Create(new Guid("ff808181932badb60195ed58b91371c5"),"TestName");

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be("TestName");
            product.ID.Should().Be(new Guid("ff808181932badb60195ed58b91371c5"));
        }

    }
}
