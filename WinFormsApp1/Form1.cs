namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //await Task.Delay(5000);
            await ReadFile();
            button1.Text = "OK";
        }

        async Task ReadFile()
        {
            var data = await Read().ConfigureAwait(false);
            await Write(data).ConfigureAwait(false);
        }
        async Task<byte[]> Read()
        {
            await Task.Delay(1000);
            return Array.Empty<byte>();
        }
        Task Write(byte[] data) => Task.CompletedTask;

        static void Do()
        {
            Thread.Sleep(5000);
            throw new InvalidCastException();
        }

        //static void await(Func<Task> f)
        //{
        //    Task.Run(f).Wait();
        //}
    }
}
