using System.Diagnostics;

//for (var i = 0; i < 200_000; i++)
//{
//    //var t = new Thread(o =>
//    //{
//    //    //Console.WriteLine("Test");
//    //    Thread.Sleep(-1);
//    //});
//    //t.Start();
//    ThreadPool.QueueUserWorkItem(o =>
//    {
//        Console.WriteLine("Test");
//    });
//    Console.WriteLine(i);
//}

//var t = Task.Run(() => { Console.WriteLine("Test"); return 10; });
////var t2 = Task.Factory.StartNew(() => { Console.WriteLine("Test"); });
////var t3 = new Task(() => { Console.WriteLine("Test"); });
////t3.Start();

var data = Enumerable.Range(0, 128).ToArray();
var sw = Stopwatch.StartNew();
var tasks = new List<Task>();
for (var i = 0; i < data.Length; i++)
{
    tasks.Add(RunTask(i));
}
foreach (var task in tasks)
    task.Wait();
Console.WriteLine(sw.Elapsed);

Console.ReadLine();

Task RunTask(int i)
{
    return Task.Run(() =>
    {
        data[i] = Sum(data[i], data[i]);
    });
}

int Sum(int a, int b)
{
    Thread.SpinWait(6000000);
    return a + b;
}