namespace Catalog.Features.Medicines.GetList;

public record MedicineDto(
    int Id,
    string Name,
    string Description,
    string Manufacturer,
    decimal Price,
    bool IsRequiredPrescription);