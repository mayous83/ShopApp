namespace ShopApp.Shared.Library.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(string queue, T message, CancellationToken cancellationToken = default);
    void Subscribe<T>(string queue, Func<T, Task> handler);
}
