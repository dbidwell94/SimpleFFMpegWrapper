namespace FFMpegWrapper.Options
{
    /// <summary>
    /// This enum maps to the -s parameter of FFMpeg
    /// </summary>
    public enum Resolution
    {
        /// <summary>240p</summary>
        X426x240,

        /// <summary>360p</summary>
        X640x360,

        /// <summary>480p</summary>
        X854x480,

        /// <summary>720p</summary>
        X1280x720,

        /// <summary>1080p</summary>
        X1920x1080,

        /// <summary>1440p</summary>
        X2560x1440,

        /// <summary>2160p</summary>
        X3840x2160
    }

    public static class ResolutionEnumExtensions
    {
        /// <summary>
        /// Return the normalized string representation of this enum
        /// </summary>
        /// <param name="resolution">The resolution to extract the string from</param>
        /// <returns></returns>
        public static string ToNormalizedString(this Resolution resolution)
        {
            string enumString = resolution.ToString();
            return enumString.Remove(0, 1);
        }
    }
}