using System.Runtime.ExceptionServices;

namespace HappyDesk.Infrastructure.Exceptions;

public static partial class GlobalExceptionHandler
{
    public static void HandleException(Exception exception, Action<string> action)
    {
        switch (exception)
        {
            case InvalidOperationException invalidOperationException :
                HandleInvalidOperationException(invalidOperationException, action); break;
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}