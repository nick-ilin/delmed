using Order.Infrastructure.Entities.Enums;

namespace Order.Infrastructure.Entities;

/// <summary>
/// Заказ
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Статус записи
    /// </summary>
    public CommonStatus Status { get; set; } = CommonStatus.Active;

    /// <summary>
    /// Статус заказа
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// Сумма заказа
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Медицинские препараты в заказе
    /// </summary>
    public ICollection<OrderMedicine> OrderMedicines { get; set; } = [];
}
