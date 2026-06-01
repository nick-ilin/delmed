using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Infrastructure.Entities;

namespace Order.Infrastructure.Data.Configurations;

public class OrderMedicineConfiguration : IEntityTypeConfiguration<OrderMedicine>
{
    public void Configure(EntityTypeBuilder<OrderMedicine> builder)
    {
        builder.HasOne(p => p.Order)
            .WithMany(p => p.OrderMedicines)
            .HasForeignKey(p => p.OrderId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Medicine)
            .WithMany(p => p.OrderMedicines)
            .HasForeignKey(p => p.MedicineId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
