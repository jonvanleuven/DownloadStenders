namespace DownloadStenders
{
    class Program
    {
        static void Main(string[] args)
        {
            new DownloadStenders(new Arguments(args)).Run();
        }
    }
}
