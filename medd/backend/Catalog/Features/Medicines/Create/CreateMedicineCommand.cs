using MediatR;

namespace Catalog.Features.Medicines.Create;

public record CreateMedicineCommand(
    string Name,
    string Description,
    string Manufacturer,
    decimal Price,
    bool IsRequiredPrescription) : IRequest<CreateMedicineResponse>;