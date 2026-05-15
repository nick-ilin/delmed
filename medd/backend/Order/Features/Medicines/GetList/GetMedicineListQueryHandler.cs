using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;

namespace Order.Features.Medicines.GetList;

public record GetMedicineListQuery() : IRequest<List<MedicineDto>>;

public record MedicineDto(int Id, string Name, string Description, string Manufacturer, decimal Price, bool IsRequiredPrescription);

public class GetMedicineListQueryHandler(OrderDbContext context) : IRequestHandler<GetMedicineListQuery, List<MedicineDto>>
{
    public async Task<List<MedicineDto>> Handle(GetMedicineListQuery request, CancellationToken cancellationToken)
    {
        return await context.Medicines
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