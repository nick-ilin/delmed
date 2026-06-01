using Catalog.Features.Medicines.Delete;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Entities;
using Catalog.Infrastructure.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.UnitTests;

public class DeleteMedicineCommandTests
{
    [Fact]
    public async Task Delete_WhenMedicineExists_RemovesFromDatabase()
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

        var command = new DeleteMedicineCommand(1);
        var handler = new DeleteMedicineCommandHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deleted = await context.Medicines.FindAsync(1);
        deleted!.Status.Should().Be(0);
    }

    [Fact]
    public async Task Delete_WhenMedicineNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        var handler = new DeleteMedicineCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new DeleteMedicineCommand(999), CancellationToken.None)
        );

        exception.Message.Should().Contain("не найдено");
    }
}
