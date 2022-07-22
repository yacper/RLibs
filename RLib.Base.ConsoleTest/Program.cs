// See https://aka.ms/new-console-template for more information

using RLib.Base;

Console.WriteLine($"MainThread:{Thread.CurrentThread.ManagedThreadId}");

Thread t = new Thread(() =>
{
    Console.WriteLine($"Work Thread:{Thread.CurrentThread.ManagedThreadId}");
    MainThreadEx.ExecuteInMainThread(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
    });
});
t.Start();


Console.Read();