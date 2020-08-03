using System.Linq;

namespace DownloadStenders
{
    public class Arguments
    {
        public Arguments(params string[] args)
        {
            Folder = args.Length>0?args[0]:null;
            Poll = args.Contains("-poll");
            All = args.Contains("-all");
            DisplayUsage = args.Length == 0 || args.Contains("-help") || args.Contains("?");
        }
        public string Folder { get; set; }
        public bool Poll { get; set; }
        public bool All { get; set; }
        public bool DisplayUsage { get; set; }
        public string Usage => @"Usage:
	DownloadStenders.exe <output-folder> [-poll] [-all]

	output-folder   folder to write mp3's to (mandatory)
	-poll           check for new mp3's every hour (optional)
	-all            download all mp3's (optional)";
    }
}