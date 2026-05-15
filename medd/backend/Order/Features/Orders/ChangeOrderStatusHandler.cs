using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using Order.Infrastructure.Entities.Enums;

namespace Order.Features.Orders;

public record ChangeOrderStatusCommand(int OrderId, OrderStatus Status) : IRequest<ChangeOrderStatusResponse>;

public record ChangeOrderStatusResponse(int OrderId, OrderStatus Status);

public class ChangeOrderStatusHandler(OrderDbContext dbContext, ILogger<ChangeOrderStatusHandler> logger) : IRequestHandler<ChangeOrderStatusCommand, ChangeOrderStatusResponse>
{
    public async Task<ChangeOrderStatusResponse> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Where(m => m.Id == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);


        if (order == null)
            return null;

        order.OrderStatus = request.Status;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order {OrderId} status updated to payed", order.Id);

        return new ChangeOrderStatusResponse(order.Id, order.OrderStatus);
    }
}
