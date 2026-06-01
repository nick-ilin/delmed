namespace Order.Infrastructure.Entities;

/// <summary>
/// Медицинские препараты в заказе
/// </summary>
public class OrderMedicine
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Заказ
    /// </summary>
    public Order Order { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Препарат
    /// </summary>
    public Medicine Medicine { get; set; }

    /// <summary>
    /// Идентификатор препарата
    /// </summary>
    public int MedicineId { get; set; }
}
