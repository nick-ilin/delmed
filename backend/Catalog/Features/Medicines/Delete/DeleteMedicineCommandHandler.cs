using MediatR;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure.Data;

namespace Catalog.Features.Medicines.Delete;

public class DeleteMedicineCommandHandler(CatalogDbContext context) : IRequestHandler<DeleteMedicineCommand, bool>
{
    public async Task<bool> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = await context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (medicine == null)
            return false;

        medicine.Status = 0;

        context.Medicines.Update(medicine);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}