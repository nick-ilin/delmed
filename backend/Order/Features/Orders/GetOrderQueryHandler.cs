using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using Order.Infrastructure.Entities.Enums;

namespace Order.Features.Orders;

public record GetOrderQuery(int OrderId) : IRequest<GetOrderResponse?>;

public record GetOrderResponse(
    int Id,
    DateTime CreatedAt,
    decimal TotalPrice,
    OrderStatus Status,
    List<OrderMedicineDto> Items);

public record OrderMedicineDto(int MedicineId, string MedicineName);

public class GetOrderQueryHandler(OrderDbContext dbContext) : IRequestHandler<GetOrderQuery, GetOrderResponse?>
{
    public async Task<GetOrderResponse?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(o => o.OrderMedicines)
                .ThenInclude(om => om.Medicine)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            return null;

        var items = order.OrderMedicines.Select(om => new OrderMedicineDto(
            om.MedicineId,
            om.Medicine.Name))
            .ToList();

        return new GetOrderResponse(
            order.Id,
            order.CreatedAt,
            order.TotalPrice,
            order.OrderStatus,
            items);
    }
}