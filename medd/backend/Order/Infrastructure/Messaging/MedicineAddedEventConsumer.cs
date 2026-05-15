using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Contracts;
using Order.Infrastructure.Data;
using Order.Infrastructure.Entities;

namespace Order.Infrastructure.Messaging;

public class MedicineAddedEventConsumer(OrderDbContext dbContext, ILogger<MedicineAddedEventConsumer> logger) : IConsumer<MedicineAddedEvent>
{
    public async Task Consume(ConsumeContext<MedicineAddedEvent> context)
    {
        logger.LogInformation("Processing MedicineAddedEvent: Id={Id}, Name={Name}",
            context.Message.Id, context.Message.Name);

        // Проверяем, существует ли уже
        var exists = await dbContext.Medicines.AnyAsync(m => m.ExternalId == context.Message.Id);
        if (exists)
        {
            logger.LogWarning("Medicine {Id} already exists, skipping", context.Message.Id);
            return;
        }

        var medicine = new Medicine
        {
            ExternalId = context.Message.Id,
            Name = context.Message.Name,
            Description = context.Message.Description,
            Manufacturer = context.Message.Manufacturer,
            Price = context.Message.Price,
            IsRequiredPrescription = context.Message.IsRequiredPrescription,
            CreatedAt = context.Message.CreatedAt
        };

        await dbContext.Medicines.AddAsync(medicine);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Medicine {Id} saved to database", context.Message.Id);
    }
}