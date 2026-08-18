using Auth_Module.Application.Services;

namespace Auth_Module.Infrastructure.Services;

public class ExecuteAtomically : IExecuteAtomically
{
    public async Task<T> ExecuteAtomicallyAsync<T>(Func<Task<T>> mainTask)
    {
        try
        {
            return await mainTask.Invoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
