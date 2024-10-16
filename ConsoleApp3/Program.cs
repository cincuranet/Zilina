using System.Collections.Concurrent;

internal class Program
{
    static int x = 10;

    private static void Main(string[] args)
    {
        //Thread.VolatileWrite(ref x, 11);
        //Volatile.Write(ref x, 12);
        //Interlocked.CompareExchange(ref x, 10, 0);

        var countdown = new Countdown(3);
        var locker = new SimpleLock();
        var xxx = new object();
        //var sem = new SemaphoreSlim(1, 1);
        //await sem.WaitAsync();
        var bl = new BlockingCollection<int>();
        bl.GetConsumingEnumerable();
        for (var i = 0; i < 3; i++)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                //try
                //{
                //    FooBar1();
                //    lock (xxx)
                //    {
                //        FooBar();
                //    }
                //    FoBar3();
                //}
                //catch (Exception)
                //{
                //    cmvdnklvndsk
                //    throw;
                //}
                //try
                //{
                //    Monitor.Enter(xxx, ref lockTaken);
                //}
                //finally
                //{
                //    if (lockTaken)
                //        Monitor.Exit(xxx);
                //}
                Console.WriteLine($"Entering {o}");
                locker.Enter();
                Console.WriteLine($"Entered {o}");
                Thread.Sleep(5000);
                locker.Exit();
                Console.WriteLine($"Exited {o}");
                countdown.Signal();
            }, i);
        }
        countdown.Wait();

        var rwl = new ReaderWriterLockSlim();
        rwl.EnterWriteLock();
        //...
        rwl.ExitWriteLock();

        //using (var mre = new ManualResetEvent(false))
        //{
        //    mre.WaitOne();
        //    mre.Set();
        //    mre.Reset();
        //}
        //using (var are = new AutoResetEvent(false))
        //{
        //    are.WaitOne();
        //    are.Set();
        //    //are.Reset();
        //}
        //using (var sem = new Semaphore(10, 10))
        //{
        //    sem.WaitOne();
        //    sem.Release();
        //}
        //using (var m = new Mutex(false, @"Global\Test", out var createdNew))
        //{
        //    Console.ReadLine();
        //    m.WaitOne();
        //    m.ReleaseMutex();
        //}
    }

    void Order()
    {
        EnterUPGLock();
        if (stock == 0)
        {
            EnterWriteLock();
            SendOrder();
            stock += 10;
        }
        else
        {
            // foo
        }
    }

    object _foo;
    public object Foo
    {
        get
        {
            if (_foo == null)
            {
                lock (_foo)
                {
                    if (_foo == null)
                    {
                        _foo = new object();
                    }
                }
            }
            return _foo;
        }
    }
}

class SimpleLock
{
    int _lockTaken = 0;

    public void Enter()
    {
        while (Interlocked.Exchange(ref _lockTaken, 1) == 1)
        {
            Thread.SpinWait(1000);
        }
    }

    public void Exit()
    {
        Volatile.Write(ref _lockTaken, 0);
    }
}

class Countdown : IDisposable
{
    int _count;
    ManualResetEvent _event;

    public Countdown(int count)
    {
        _count = count;
        _event = new ManualResetEvent(false);
    }

    public void Signal()
    {
        if (Interlocked.Decrement(ref _count) == 0)
            _event.Set();
    }

    public void Wait()
    {
        _event.WaitOne();
    }

    public void Dispose()
    {
        _event.Dispose();
    }
}