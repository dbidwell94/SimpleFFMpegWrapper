namespace FFMpegWrapper.Options
{
    /// <summary>
    /// This enum maps to the -b:v parameter of FFMpeg
    /// </summary>
    public enum Bitrate
    {
        ///<summary>400Kbps</summary>
        K400,

        /// <summary>800Kbps</summary>
        K800,
        
        /// <summary>1Mbps</summary>
        M1,

        /// <summary>1.5Mbps</summary>
        M1_5,

        /// <summary>2Mpbs</summary>
        M2,

        /// <summary>2.5Mbps</summary>
        M2_5,

        /// <summary>5Mbps</summary>
        M5,

        /// <summary>7.5Mbps</summary>
        M7_5,

        /// <summary>8Mpbs</summary>
        M8,

        /// <summary>12Mbps</summary>
        M12,

        /// <summary>16Mbps</summary>
        M16,

        /// <summary>24Mbps</summary>
        M24,

        /// <summary>35Mbps</summary>
        M35,

        /// <summary>40Mbps</summary>
        M40
    }

    public static class BitrateEnumExtensions
    {
        public static string ToNormalizedString(this Bitrate bRate)
        {
            string bitrateString = bRate.ToString();
            char size = bitrateString[0];
            bitrateString = bitrateString.Replace('_', '.');
            bitrateString = bitrateString.Remove(0, 1);
            bitrateString += size;
            return bitrateString;
        }
    }
}