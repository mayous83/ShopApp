using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShopApp.Shared.Library.Messaging;

public class MessageBusHostedService : IHostedService
{
    private readonly IMessageBus               _bus;
    private readonly IServiceProvider          _sp;
    private readonly IEnumerable<Type>         _consumerTypes;

    public MessageBusHostedService(IMessageBus bus, IServiceProvider sp)
    {
        _bus = bus;
        _sp  = sp;

        // find all IEventConsumer<T> implementations in this assembly:
        _consumerTypes = typeof(MessageBusHostedService)
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterfaces()
                         .Any(i => i.IsGenericType &&
                                   i.GetGenericTypeDefinition() == typeof(IEventConsumer<>)))
            .ToList();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // for each consumer type, resolve the generic type argument and subscribe
        foreach (var consumerType in _consumerTypes)
        {
            var @interface = consumerType
                .GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventConsumer<>));

            var eventType   = @interface.GetGenericArguments()[0];
            var queueName   = eventType.Name;  // e.g. "OrderCreatedEvent"

            // use reflection to call Subscribe<T>
            var subscribeMethod = typeof(IMessageBus)
                .GetMethod(nameof(IMessageBus.Subscribe))!
                .MakeGenericMethod(eventType);

            subscribeMethod.Invoke(_bus, new object[]
            {
                queueName,
                // handler: resolve consumer from DI and call Handle
                new Func<object, Task>(async (payload) =>
                {
                    using var scope = _sp.CreateScope();
                    var consumer = scope.ServiceProvider.GetRequiredService(consumerType);
                    var handle    = @interface.GetMethod(nameof(IEventConsumer<object>.Handle))!;
                    await (Task)handle.Invoke(consumer, new[] { payload })!;
                })
            });
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
