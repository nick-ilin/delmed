using Order.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Entities.Order>
{
    public void Configure(EntityTypeBuilder<Entities.Order> builder)
    {
        builder.HasQueryFilter(m => m.Status != 0);

        builder.HasKey(m => m.Id);

        builder.Property(o => o.TotalPrice)
            .HasPrecision(18, 2);
    }
}