namespace ShopApp.Shared.Library.DTOs;

public sealed class Result<T>
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }

    private Result(bool success, string message, T? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
    
    public static Result<T> SuccessResult(T data)
    {
        return new Result<T>(true, string.Empty, data);
    }
    
    public static Result<T> FailureResult(string message)
    {
        return new Result<T>(false, message, default);
    }
}
