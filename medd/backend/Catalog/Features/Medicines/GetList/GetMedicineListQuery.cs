using MediatR;

namespace Catalog.Features.Medicines.GetList;

public record GetMedicineListQuery() : IRequest<List<MedicineDto>>;