using Common_Module.Mediator.Command.Abstract;

namespace Test_Module.Test.Command;

public class TestCommand1 : ICommand
{
    public string Name {get; set;} = "no-name";
}

public class TestCommand2 : ICommand<string>
{
    public string Name {get; set;} = "no-name";
}

public class TestCommand1Handler : ICommandHandler<TestCommand1>
{
    public async Task Handle(TestCommand1 command, CancellationToken cancellationToken = default)
    {
        await Task.Delay(3000);
        Console.WriteLine("Command 1 (no result): " + command.Name);

    }
}

public class TestCommand2Handler : ICommandHandler<TestCommand2, string>
{
    public async Task<string?> Handle(TestCommand2 command, CancellationToken cancellationToken = default)
    {
        await Task.Delay(3000);
        string s = "Command 1 (have result): " + command.Name;
        Console.WriteLine(s);
        return s;
    }
}
