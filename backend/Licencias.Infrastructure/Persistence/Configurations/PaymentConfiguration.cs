using Licencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licencias.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(t => t.Amount)
                .HasPrecision(18, 2);

            builder.HasOne(t => t.Subscription).WithMany(s => s.Payments);
        }
    }
}
