using Licencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licencias.Infrastructure.Persistence.Configurations
{
    public class ProductVersionConfiguration : IEntityTypeConfiguration<ProductVersion>
    {
        public void Configure(EntityTypeBuilder<ProductVersion> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(300);

            builder.Property(t => t.Price)
                .HasPrecision(18, 2);

            builder.HasMany(t => t.Subscriptions).WithOne(s => s.ProductVersion);
            builder.HasOne(t => t.Product).WithMany(p => p.ProductVersions);
        }
    }
}
