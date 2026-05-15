using MassTransit;
using Catalog.Contracts;
using Serilog;

namespace Catalog.Infrastructure.Messaging;

public class MedicineAddedEventConsumer : IConsumer<MedicineAddedEvent>
{
    private readonly Serilog.ILogger _logger;

    public MedicineAddedEventConsumer()
    {
        _logger = Log.ForContext<MedicineAddedEventConsumer>();
    }

    public async Task Consume(ConsumeContext<MedicineAddedEvent> context)
    {
        _logger.Information("Received MedicineAddedEvent: {MedicineId} - {MedicineName}",
            context.Message.Id,
            context.Message.Name);

        await Task.CompletedTask;
    }
}