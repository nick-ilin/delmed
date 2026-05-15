namespace Catalog.Infrastructure.Entities;

/// <summary>
/// Медицинский препарат
/// </summary>
public class Medicine
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внешний идентификатор
    /// </summary>
    public Guid ExternalId { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Производитель
    /// </summary>
    public required string Manufacturer { get; set; }

    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Признак необходимости рецепта врача
    /// </summary>
    public bool IsRequiredPrescription { get; set; }

    /// <summary>
    /// Статус записи
    /// </summary>
    public int Status { get; set; } = 1;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
