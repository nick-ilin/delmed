using MediatR;

namespace Catalog.Features.Medicines.Update;

public record UpdateMedicineCommand(
    int Id,
    string Name,
    string Description,
    string Manufacturer,
    decimal Price,
    bool IsRequiredPrescription) : IRequest<UpdateMedicineResponse?>;