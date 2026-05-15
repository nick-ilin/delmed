namespace Order.Contracts;

public class MedicineAddedEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Manufacturer { get; set; }
    public decimal Price { get; set; }
    public bool IsRequiredPrescription { get; set; }
    public DateTime CreatedAt { get; set; }
}