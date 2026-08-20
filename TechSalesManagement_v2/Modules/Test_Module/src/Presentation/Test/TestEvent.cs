using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Event.Abstract;

namespace Test_Module.Test.Command;

public class TestEvent : IEvent
{
    public string Name {get; set;} = "no-name";
}


public class TestEvent1Handler : IEventHandler<TestEvent>
{
    public async Task Handle(TestEvent eve, CancellationToken cancellationToken = default)
    {
        
        await Task.Delay(2000, cancellationToken);
        Console.WriteLine("1 " + eve.Name);
    }
}


public class TestEvent2Handler : IEventHandler<TestEvent>
{
    public async Task Handle(TestEvent eve, CancellationToken cancellationToken = default)
    {
        await Task.Delay(2000);
        Console.WriteLine("2 " + eve.Name);
    }
}

public class TestEvent3Handler : IEventHandler<TestEvent>
{
    public async Task Handle(TestEvent eve, CancellationToken cancellationToken = default)
    {
        
        await Task.Delay(3000);
        Console.WriteLine("3 " + eve.Name);
    }
}
