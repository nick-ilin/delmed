using Catalog.Features.Medicines.Update;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Entities;
using Catalog.Infrastructure.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.UnitTests;

public class UpdateMedicineCommandTests
{
    [Fact]
    public async Task Update_WhenMedicineExists_UpdatesData()
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

        var command = new UpdateMedicineCommand(
            Id: 1,
            Name: "Новое имя",
            Description: "Новое описание",
            Manufacturer: "Новый производитель",
            Price: 200,
            IsRequiredPrescription: true
        );
        var handler = new UpdateMedicineCommandHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Новое имя");

        var updated = await context.Medicines.FindAsync(1);
        updated!.Price.Should().Be(200);
        updated.IsRequiredPrescription.Should().BeTrue();
    }

    [Fact]
    public async Task Update_WhenMedicineNotFound_ReturnsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        var handler = new UpdateMedicineCommandHandler(context);

        var command = new UpdateMedicineCommand(999, "Имя", "Описание", "Производитель", 100, false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None)
        );

        exception.Message.Should().Contain("не найдено");
    }
}
