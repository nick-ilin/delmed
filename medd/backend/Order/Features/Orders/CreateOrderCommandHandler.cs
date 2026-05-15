using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using Order.Infrastructure.Entities;
using Order.Infrastructure.Entities.Enums;

namespace Order.Features.Orders;

public record CreateOrderCommand(List<CreateOrderItemDto> Items) : IRequest<CreateOrderResponse>;

public record CreateOrderItemDto(int MedicineId);

public record CreateOrderResponse(int OrderId, decimal TotalPrice);

public class CreateOrderCommandHandler(OrderDbContext dbContext, ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Получаем все лекарства из БД
        var medicineIds = request.Items.Select(i => i.MedicineId).Distinct().ToList();
        var medicines = await dbContext.Medicines
            .Where(m => medicineIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, cancellationToken);

        // Создаём заказ
        var order = new Infrastructure.Entities.Order
        {
            CreatedAt = DateTime.UtcNow,
            TotalPrice = 0,
            Status = CommonStatus.Active,
            OrderStatus = OrderStatus.Added
        };

        decimal totalPrice = 0;
        var orderMedicines = new List<OrderMedicine>();

        foreach (var item in request.Items)
        {
            if (!medicines.TryGetValue(item.MedicineId, out var medicine))
                throw new Exception($"Medicine {item.MedicineId} not found");

            var orderMedicine = new OrderMedicine
            {
                MedicineId = item.MedicineId,
            };

            totalPrice += medicine.Price;
            orderMedicines.Add(orderMedicine);
        }

        order.TotalPrice = totalPrice;
        order.OrderMedicines = orderMedicines;

        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order {OrderId} created with total price {TotalPrice}", order.Id, totalPrice);

        return new CreateOrderResponse(order.Id, order.TotalPrice);
    }
}