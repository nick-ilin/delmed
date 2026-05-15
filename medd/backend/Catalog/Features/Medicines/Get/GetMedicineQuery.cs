using MediatR;

namespace Catalog.Features.Medicines.Get;

public record GetMedicineQuery(int Id) : IRequest<GetMedicineResponse?>;