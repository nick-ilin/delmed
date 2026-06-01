using MediatR;

namespace Catalog.Features.Medicines.Delete;

public record DeleteMedicineCommand(int Id) : IRequest<bool>;