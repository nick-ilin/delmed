namespace Catalog.Features.Medicines.Get;

public record GetMedicineResponse(
    int Id,
    string Name,
    string Description,
    string Manufacturer,
    decimal Price,
    bool IsRequiredPrescription,
    DateTime CreatedAt);