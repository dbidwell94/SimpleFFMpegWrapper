using System;
using System.IO;

namespace FFMpegWrapper
{
    public sealed class ConversionEventArgs
    {
#nullable enable
        public Exception? ThrownException { get; private set; }
        public FileInfo InputFile { get; private set; }
        public FileInfo? OutputFile { get; private set; }
        public TimeSpan? ConversionDuration { get; private set; }
        public StreamReader? PipedOutput { get; private set; }
#nullable disable

        internal ConversionEventArgs(FileInfo input, FileInfo output, TimeSpan duration)
        {
            ThrownException = null;
            InputFile = input;
            OutputFile = output;
            ConversionDuration = duration;
        }

        internal ConversionEventArgs(FileInfo input, FileInfo output)
        {
            InputFile = input;
            OutputFile = output;
        }

        

    }
}