namespace EntityFrameworkCore.Middlewares;

public interface IMiddleware<in TRequest, out TResponse>
{
    TResponse Invoke(TRequest request);
}