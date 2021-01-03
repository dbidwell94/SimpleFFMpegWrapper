using FFMpegWrapper.Downloader;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FFMpegWrapper
{
    public class Wrapper
    {
        public static string FFMpegLocation = FFMpegDownloader.FFMpegLocation;
        public Wrapper()
        {

        }

        public async Task VerifyOrDownload()
        {
            await FFMpegDownloader.DownloadAndVerifyAsync();
        }
    }
}