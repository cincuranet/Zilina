using System.Diagnostics;
using System.Runtime.InteropServices;

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

//var data = Enumerable.Range(0, 128).ToArray();
//var sw = Stopwatch.StartNew();
//var tasks = new List<Task>();
//for (var i = 0; i < data.Length; i++)
//{
//    tasks.Add(RunTask(i));
//}
//Task.WaitAll([.. tasks]);
//Console.WriteLine(sw.Elapsed);

var data = Enumerable.Range(0, 128).ToArray();
var sw = Stopwatch.StartNew();
var sum = 0;
//for (var i = 0; i < data.Length; i++)
//{
//    sum = Sum(sum, data[i]);
//}
//try
//{
//    using var cts = new CancellationTokenSource();
//    cts.CancelAfter(1000);
//    Console.WriteLine(SumArray(data, cts.Token));
//}
//finally
//{
//    Console.WriteLine(sw.Elapsed);
//}

//data
//    .AsParallel()
//    //.WithCancellation()
//    //.WithDegreeOfParallelism(Environment.ProcessorCount + 1)
//    //.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
//    //.WithMergeOptions(ParallelMergeOptions.NotBuffered)
//    .Select(x => Sum(x, x))
//    .ToArray();

//Parallel.For(0, data.Length, /*new ParallelOptions() {, */ (i, pls) =>
////for (var i = 0; i < data.Length; i++)
//{
//    data[i] = Sum(data[i], data[i]);
//    if (data[i] > 100)
//        pls.Stop();
//});

//Parallel.Invoke()
//Invoke2(() => "ssss", () => 10);

//Parallel.For(0, data.Length,
//    () => 0,
//    (index, pls, local) => Sum(local, data[index]),
//    local => Interlocked.Add(ref sum, local));
//Console.WriteLine(sum);
Console.WriteLine(sw.Elapsed);

Timer timer = null;
timer = new Timer(o =>
{
    Console.WriteLine(DateTime.Now);
    timer.Change(1000, -1);
}, null, -1, -1);
timer.Change(0, -1);
//timer.Change()
//timer.Dispose()

Console.ReadLine();

int SumArray(int[] data, CancellationToken cancellationToken)
{
    cancellationToken.ThrowIfCancellationRequested();
    switch (data)
    {
        case [var x]:
            return x;
        case [var x, var y]:
            return Sum(x, y);
        default:
            var left = data.Take(data.Length / 2).ToArray();
            var right = data.Skip(data.Length / 2).ToArray();
            var leftTask = Task.Run(() => SumArray(left, cancellationToken));
            var rightTask = Task.Run(() => SumArray(right, cancellationToken));
            return Sum(leftTask.Result, rightTask.Result);
    }
}

//Task RunTask(int i)
//{
//    return Task.Run(() =>
//    {
//        data[i] = Sum(data[i], data[i]);
//    });
//}

int Sum(int a, int b)
{
    Thread.SpinWait(6000000);
    return a + b;
}

object[] Invoke2(params Func<object>[] funcs)
{
    var tasks = new Task<object>[funcs.Length];
    for (var i = 0; i < funcs.Length; i++)
    {
        tasks[i] = Task.Run(() => funcs[i]());
    }
    Task.WaitAll(tasks);
    return tasks.Select(x => x.Result).ToArray();
}