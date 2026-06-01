using MediatR;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Exceptions;

namespace Catalog.Features.Medicines.Update;

public class UpdateMedicineCommandHandler(CatalogDbContext context) : IRequestHandler<UpdateMedicineCommand, UpdateMedicineResponse?>
{
    public async Task<UpdateMedicineResponse> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = await context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Лекарственное средство не найдено.");

        medicine.Name = request.Name;
        medicine.Description = request.Description;
        medicine.Manufacturer = request.Manufacturer;
        medicine.Price = request.Price;
        medicine.IsRequiredPrescription = request.IsRequiredPrescription;

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateMedicineResponse(medicine.Id, medicine.Name);
    }
}