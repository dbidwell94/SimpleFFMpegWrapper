using System.Runtime.InteropServices;

namespace FFMpegWrapper
{
    public static class FFMpegFilenames
    {
        public static string FFMpeg
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "ffmpeg";
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "ffmpeg.exe";
                }
                else return "ffmpeg";
            }
        }

        public static string FFProbe
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "ffprobe";
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "ffprobe.exe";
                }
                else return "ffprobe";
            }
        }
    }
}