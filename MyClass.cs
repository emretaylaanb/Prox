public interface IMyService
{
    [Log]
    int DoWork(int a, int b);

    void DoOtherWork();
}

public class MyService : IMyService
{
    public int DoWork(int a, int b)
    {
        Console.WriteLine("Doing important work...");
        return a * b;
    }

    public void DoOtherWork()
    {
        Console.WriteLine("Doing other work...");
    }
}
