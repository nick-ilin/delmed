using Catalog.Features.Medicines.GetList;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.UnitTests;

public class GetMedicineListQueryTests
{
    [Fact]
    public async Task GetList_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        for (int i = 1; i <= 15; i++)
        {
            context.Medicines.Add(new Medicine
            {
                Id = i,
                Name = $"Лекарство {i}",
                Description = $"Описание {i}",
                Manufacturer = $"Производитель {i}",
                ExternalId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                IsRequiredPrescription = false,
                Price = 100 + i * 10,
                Status = 1
            });
        }
        await context.SaveChangesAsync();

        var query = new GetMedicineListQuery();
        var handler = new GetMedicineListQueryHandler(context);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(15);
        result.First().Name.Should().Be("Лекарство 1");
    }

    [Fact]
    public async Task GetList_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new CatalogDbContext(options);
        var handler = new GetMedicineListQueryHandler(context);

        // Act
        var result = await handler.Handle(new GetMedicineListQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
