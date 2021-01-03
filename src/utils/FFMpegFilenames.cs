using System.Runtime.InteropServices;
using System;

namespace FFMpegWrapper
{
    class FFMpegFilenames
    {
        public string FFMpeg
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

        public string FFProbe
        {
            get
            {
                return "";
            }
        }
    }
}