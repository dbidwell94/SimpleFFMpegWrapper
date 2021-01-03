using System.Threading.Tasks;
using System.IO;
using System;
using FFMpegWrapper.Models;

namespace FFMpegWrapper.Utils
{
    internal static class StreamToMediaFileInfo
    {
        public static async Task<MediaFileInfo> ParseStream(StreamReader input)
        {
            return await Task.Run(async () =>
            {
                return new MediaFileInfo(await input.ReadToEndAsync());
            });
        }
    }
}