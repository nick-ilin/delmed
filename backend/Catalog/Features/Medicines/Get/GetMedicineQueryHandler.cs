using MediatR;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure.Data;

namespace Catalog.Features.Medicines.Get;

public class GetMedicineQueryHandler(CatalogDbContext context) : IRequestHandler<GetMedicineQuery, GetMedicineResponse?>
{
    public async Task<GetMedicineResponse?> Handle(GetMedicineQuery request, CancellationToken cancellationToken)
    {
        var medicine = await context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (medicine == null)
            return null;

        return new GetMedicineResponse(
            medicine.Id,
            medicine.Name,
            medicine.Description,
            medicine.Manufacturer,
            medicine.Price,
            medicine.IsRequiredPrescription,
            medicine.CreatedAt);
    }
}