using Catalog.Features.Medicines.Get;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Entities;
using Catalog.Infrastructure.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Catalog.UnitTests;

public class GetMedicineQueryTests
{
    [Fact]
    public async Task GetById_WhenMedicineExists_ReturnsMedicine()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        var medicine = new Medicine
        {
            Id = 1,
            Name = "Аспирин",
            Description = "Описание",
            Manufacturer = "Производитель",
            ExternalId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            IsRequiredPrescription = false,
            Price = 100,
            Status = 1
        };
        context.Medicines.Add(medicine);
        await context.SaveChangesAsync();

        var query = new GetMedicineQuery(1);
        var handler = new GetMedicineQueryHandler(context);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Аспирин");
    }

    [Fact]
    public async Task GetById_WhenMedicineNotFound_ReturnsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        var handler = new GetMedicineQueryHandler(context);
        var query = new GetMedicineQuery(999);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(query, CancellationToken.None)
        );

        exception.Message.Should().Contain("не найдено");
    }
}
