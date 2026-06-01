using MassTransit;

namespace Order.Infrastructure.Messaging;

public class ConsumeLogFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        Console.WriteLine($"[CONSUME LOG] Starting: {typeof(T).Name}, MessageId: {context.MessageId}");

        try
        {
            await next.Send(context);
            Console.WriteLine($"[CONSUME LOG] Completed: {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CONSUME LOG] Error: {ex.Message}");
            throw;
        }
    }

    public void Probe(ProbeContext context) { }
}