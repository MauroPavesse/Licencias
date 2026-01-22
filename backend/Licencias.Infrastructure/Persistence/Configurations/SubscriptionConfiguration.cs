using Licencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licencias.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasOne(t => t.Customer)
                   .WithMany(t => t.Subscriptions);

            builder.HasOne(t => t.ProductVersion)
                .WithMany(t => t.Subscriptions);

            builder.HasMany(t => t.Payments)
                .WithOne(t => t.Subscription);

            builder.HasMany(t => t.Extras)
                .WithMany(t => t.Subscriptions);
        }
    }
}
