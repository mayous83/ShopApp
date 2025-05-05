namespace ShopApp.Shared.Library.Messaging;

public interface IEventConsumer<T> where T : class
{
    Task Handle(T @event);
}