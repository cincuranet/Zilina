namespace WinFormsApp1;

public partial class Form1 : Form
{
    static HttpClient Http = new HttpClient();

    public Form1()
    {
        InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        button1.Text = "Loading";
        flowLayoutPanel1.SuspendLayout();
        flowLayoutPanel1.Controls.Clear();
        flowLayoutPanel1.Controls.AddRange(Enumerable.Range(0, 200)
            .Select(_ => new PictureBox() { Size = new Size(200, 200) })
            .ToArray());
        flowLayoutPanel1.ResumeLayout();
        await LoadImages(200);
        button1.Text = "Done";
    }

    async Task LoadImages(int count)
    {
        var tasks = new Task[count];
        for (var i = 0; i < count; i++)
        {
            tasks[i] = Helper(i);
        }
        await Task.WhenAll(tasks);

        async Task Helper(int i)
        {
            var data = await LoadImage($"https://dummyimage.com/200x200/00f/0f0&text=Test{i}");
            var pb = (PictureBox)flowLayoutPanel1.Controls[i];
            pb.Image = Image.FromStream(new MemoryStream(data));
        }
    }

    async Task<byte[]> LoadImage(string uri)
    {
        await Task.Delay(Random.Shared.Next(1000, 5000)).ConfigureAwait(false);
        return await Http.GetByteArrayAsync(uri).ConfigureAwait(false);
    }
}
