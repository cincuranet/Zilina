using System.Net;

//for (var i = 0; i < 1000; i++)
//{
//    var request = WebRequest.CreateHttp("https://bing.com");
//    request.BeginGetResponse(ar =>
//    {
//        var response = request.EndGetResponse(ar);
//        var stream = response.GetResponseStream();
//        var memory = new MemoryStream();
//        ReadStream(stream, memory,
//            () =>
//            {
//                memory.Dispose();
//                stream.Dispose();
//                response.Dispose();
//                Console.WriteLine("Done");
//            },
//            ex =>
//            {
//                memory.Dispose();
//                stream.Dispose();
//                response.Dispose();
//                Console.WriteLine("ERROR");
//            });
//    }, null);
//}

await Parallel.ForAsync(0, 100, async (i, _) => await DoRequest(i));

Console.ReadLine();

void ReadStream(Stream stream, Stream destination, Action onDone, Action<Exception> onError)
{
    var buffer = new byte[16 * 1024];
    stream.BeginRead(buffer, 0, buffer.Length, ar =>
    {
        int read;
        try
        {
            read = stream.EndRead(ar);
        }
        catch (IOException ex)
        {
            onError(ex);
            return;
        }
        if (read == 0)
        {
            onDone();
            return;
        }
        destination.BeginWrite(buffer, 0, read, ar =>
        {
            try
            {
                destination.EndWrite(ar);
            }
            catch (IOException ex)
            {
                onError(ex);
                return;
            }
            ReadStream(stream, destination, onDone, onError);
        }, null);
    }, null);
}

async Task DoRequest(int i)
{
    var request = WebRequest.CreateHttp("https://bing.com");
    using var response = await request.GetResponseAsync();
    using var stream = response.GetResponseStream();
    using var memory = new MemoryStream();
    var buffer = new byte[16 * 1024];
    while (true)
    {
        try
        {
            var read = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
                break;
            await memory.WriteAsync(buffer, 0, read);
        }
        catch (IOException ex)
        {
            Console.WriteLine("ERROR");
        }
    }
    Console.WriteLine($"Done {i}");
}