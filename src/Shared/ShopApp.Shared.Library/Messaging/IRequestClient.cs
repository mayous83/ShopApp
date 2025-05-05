namespace ShopApp.Shared.Library.Messaging;

public interface IRequestClient
{
    Task<TResponse> RequestAsync<TRequest, TResponse>(
        string requestQueue,
        TRequest request,
        CancellationToken ct = default
    );
}