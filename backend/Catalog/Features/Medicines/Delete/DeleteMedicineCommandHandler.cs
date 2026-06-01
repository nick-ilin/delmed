using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Features.Medicines.Delete;

public class DeleteMedicineCommandHandler(CatalogDbContext context) : IRequestHandler<DeleteMedicineCommand, bool>
{
    public async Task<bool> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = await context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Лекарственное средство не найдено.");

        medicine.Status = 0;

        context.Medicines.Update(medicine);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}