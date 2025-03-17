namespace HappyDesk.Infrastructure.Exceptions;

public static class ExceptionExtensions
{
    public static void Handle(this Exception exception, Action<string> action)
    {
        GlobalExceptionHandler.HandleException(exception, action);
    }
}