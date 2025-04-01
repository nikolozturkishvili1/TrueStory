using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueStory.Domain;
using TrueStory.Domain.Base;

namespace MyGPI.Persistence.Configurations
{
    public class BaseObjectConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TKey>
    {
        private readonly string _schema;
        private readonly string _name;

        public BaseObjectConfiguration(string schema, string name = null)
        {
            _schema = schema;
            _name = name ?? typeof(TEntity).Name;
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(_name, _schema);

            builder.Property(a => a.ID).ValueGeneratedOnAdd();
            builder.HasKey(a => a.ID);
        }
    }
}