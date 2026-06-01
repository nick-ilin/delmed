using Catalog.Features.Medicines.Create;
using Catalog.Infrastructure.Data;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Order.Contracts;

namespace Catalog.UnitTests;

public class CreateMedicineCommandTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateMedicine()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);

        var mockPublishEndpoint = new Mock<IPublishEndpoint>();
        var handler = new CreateMedicineCommandHandler(context, mockPublishEndpoint.Object);

        var command = new CreateMedicineCommand(
            Name: "Аспирин",
            Description: "От головы",
            Manufacturer: "Байер",
            Price: 150,
            IsRequiredPrescription: false
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Аспирин");

        // Проверяем, что лекарство реально сохранилось в БД
        var savedMedicine = await context.Medicines.FirstOrDefaultAsync(m => m.Name == "Аспирин");
        savedMedicine.Should().NotBeNull();
        savedMedicine.Price.Should().Be(150);

        // Проверяем, что событие было опубликовано
        mockPublishEndpoint.Verify(
            x => x.Publish(
                It.IsAny<MedicineAddedEvent>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}