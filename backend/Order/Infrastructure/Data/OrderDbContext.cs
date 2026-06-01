using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data.Configurations;
using Order.Infrastructure.Entities;

namespace Order.Infrastructure.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Entities.Order> Orders { get; set; }
    public DbSet<OrderMedicine> OrderMedicines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MedicineConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderMedicineConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}