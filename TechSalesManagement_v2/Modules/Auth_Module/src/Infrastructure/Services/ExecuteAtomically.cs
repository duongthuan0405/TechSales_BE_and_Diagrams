using Auth_Module.src.Application.Services;

namespace Auth_Module.src.Infrastructure.Services;

public class ExecuteAtomically : IExecuteAtomically
{
    public async Task<T> ExecuteAtomicallyAsync<T>(Func<Task<T>> mainTask, CancellationToken cancellationToken)
    {
        try
        {
            return await mainTask.Invoke();
        }
        catch (Exception ex)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw;
        }
    }
}
