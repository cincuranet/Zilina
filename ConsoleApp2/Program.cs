using System.Net;

for (var i = 0; i < 1000; i++)
{
    var request = WebRequest.CreateHttp("https://bing.com");
    request.BeginGetResponse(ar =>
    {
        var response = request.EndGetResponse(ar);
        var stream = response.GetResponseStream();
        var memory = new MemoryStream();
        ReadStream(stream, memory,
            () =>
            {
                memory.Dispose();
                stream.Dispose();
                response.Dispose();
                Console.WriteLine("Done");
            },
            ex =>
            {
                memory.Dispose();
                stream.Dispose();
                response.Dispose();
                Console.WriteLine("ERROR");
            });
    }, null);
}

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
