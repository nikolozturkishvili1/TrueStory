using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyGPI.Persistence.Configurations;
using TrueStory.Domain.Aggregate.Product;

namespace PersonRegistry.Persistence.Configurations.PhoneNumberType;

/// <summary>
/// Configuration for Product
/// </summary>
public class ProductConfiguration : BaseObjectConfiguration<T_Product, Guid>
{
    public ProductConfiguration() : base("Product")
    {
    }

    public override void Configure(EntityTypeBuilder<T_Product> builder)
    {
        base.Configure(builder);
        builder.Property(a => a.ID).ValueGeneratedNever();
    }
}
