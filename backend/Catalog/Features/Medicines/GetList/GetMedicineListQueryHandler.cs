using MediatR;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure.Data;

namespace Catalog.Features.Medicines.GetList;

public class GetMedicineListQueryHandler(CatalogDbContext context) : IRequestHandler<GetMedicineListQuery, List<MedicineDto>>
{
    public async Task<List<MedicineDto>> Handle(GetMedicineListQuery request, CancellationToken cancellationToken)
    {
        return await context.Medicines
            .Where(m => m.Status != 0)
            .Select(m => new MedicineDto(
                m.Id,
                m.Name,
                m.Description,
                m.Manufacturer,
                m.Price,
                m.IsRequiredPrescription))
            .ToListAsync(cancellationToken);
    }
}