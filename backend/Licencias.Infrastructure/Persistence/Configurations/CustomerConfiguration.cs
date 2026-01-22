using Licencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licencias.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Email)
                .HasMaxLength(150);

            builder.Property(t => t.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(t => t.Business)
                .HasMaxLength(100);

            builder.HasMany(t => t.Subscriptions)
                .WithOne(s => s.Customer);
        }
    }
}
