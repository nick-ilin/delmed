using Order.Contracts;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Features.Medicines.Create;

public class CreateMedicineCommandHandler(CatalogDbContext context, IPublishEndpoint publishProvider) : IRequestHandler<CreateMedicineCommand, CreateMedicineResponse>
{
    public async Task<CreateMedicineResponse> Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        var totalCount = await context.Medicines
            .Where(m => m.Status != 0)
            .CountAsync(cancellationToken);

        if (totalCount >= 9)
            throw new InvalidOperationException(
                "Ограничение на добавление записей в справочник лекарственных средств (9 шт). Удалите, чтобы добавить новое или отредактируйте старое.");

        var medicine = new Medicine
        {
            ExternalId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Manufacturer = request.Manufacturer,
            Price = request.Price,
            IsRequiredPrescription = request.IsRequiredPrescription,
            Status = 1,
            CreatedAt = DateTime.UtcNow
        };

        context.Medicines.Add(medicine);
        await context.SaveChangesAsync(cancellationToken);

        // Отправляем событие в RabbitMQ
        await publishProvider.Publish(new MedicineAddedEvent
        {
            Id = medicine.ExternalId,
            Name = medicine.Name,
            Description = medicine.Description,
            Manufacturer = medicine.Manufacturer,
            Price = medicine.Price,
            IsRequiredPrescription = medicine.IsRequiredPrescription,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return new CreateMedicineResponse(medicine.Id, medicine.Name);
    }
}