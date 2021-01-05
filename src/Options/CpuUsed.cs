using System;
namespace FFMpegWrapper.Options
{
    /// <summary>
    /// This enum maps to the -cpu-used parameter of FFMpeg
    /// </summary>
    public enum CpuUsed
    {
        One,
        Half,
        AllAvailable
    }

    public static class CpuUsedEnumExtensions
    {
        public static int GetCpuParam(this CpuUsed cpu)
        {
            switch (cpu)
            {
                case CpuUsed.One:
                    return 1;
                case CpuUsed.Half:
                    return Environment.ProcessorCount / 2;
                case CpuUsed.AllAvailable:
                    return Environment.ProcessorCount;
                default:
                    return 1;
            }
        }
    }
}