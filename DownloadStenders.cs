using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace DownloadStenders
{
    public class DownloadStenders
    {
        private readonly Arguments args;
        public DownloadStenders(Arguments args)
        {
            this.args = args;
        }
        public void Run()
        {
            if(args.DisplayUsage)
            {
                Console.WriteLine(args.Usage);
                return;
            }
            if( args.Poll ) 
            {
                while (true)
                {
                    Check();	
                    Log($"Will check again at '{DateTime.Now.AddHours(1):dd-MM-yyyy HH:mm}'");
                    Thread.Sleep(TimeSpan.FromHours(1));
                }
            }
            else
            {
                Check();
            }
        }

        private void Check()
        {
            try
            {
                if (!Directory.Exists(args.Folder))
                    Directory.CreateDirectory(args.Folder);
                ReadMp3Urls()
                    .Take(args.All ? int.MaxValue : 1)
                    .Select(mp3 => new
                    {
                        mp3,
                        localfile = new FileInfo($"{args.Folder}\\{new FileInfo(new Uri(mp3).AbsolutePath).Name}").FullName
                    })
                    .Where(x => !File.Exists(x.localfile))
                    .ToList()
                    .ForEach(x =>
                    {
                        DownloadMp3(x.mp3, x.localfile);
                    });
            }
            catch(Exception e)
            {
                Log($"Error: {e.Message}");
                Log($"StackTrace: {e.StackTrace}");
            }
        }
	
        private void DownloadMp3(string url, string localfile)
        {
            using (var wc = new WebClient())
            {
                Log($"Downloading '{url}' to '{localfile}'...");
                wc.DownloadFile(url, localfile);
                Log($"... done");
            }
        }

        private string[] ReadMp3Urls()
        {
            using (var wc = new WebClient())
            {
                var url = @"https://www.robstenders.nl/podcast/11/index";
                Log($"Reading '{url}'");
                return new Regex("\"(?<url>[^\"]+mp3)\"").Matches(wc.DownloadString(url))
                    .Cast<Match>()
                    .Select(e => e.Groups.Cast<Group>().FirstOrDefault(g => g.Name == "url")?.Value)
                    .Where(v => v != null)
                    .Distinct()
                    .ToArray();
            }
        }
	
        private static void Log(string m)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss,fff} {m}");
        }
    }
}