using Licencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licencias.Infrastructure.Persistence.Configurations
{
    public class ExtraConfiguration : IEntityTypeConfiguration<Extra>
    {
        public void Configure(EntityTypeBuilder<Extra> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(300);

            builder.Property(t => t.Price)
                .HasPrecision(18, 2);

            builder.HasMany(t => t.Subscriptions).WithMany(t => t.Extras);
        }
    }
}
