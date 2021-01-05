using System;
using System.Text.RegularExpressions;

using VCodec = FFMpegWrapper.Options.VideoCodec;
using ACodec = FFMpegWrapper.Options.AudioCodec;

namespace FFMpegWrapper.Models
{
    public sealed class MediaFileInfo
    {
        private string StdOut;

        public DateTime? CreationTime { get; private set; }

        public TimeSpan? Duration { get; private set; }

        public string Bitrate { get; private set; }

        public VCodec? VideoCodec { get; private set; }

        public ACodec? AudioCodec { get; private set; }

        public string Resolution { get; private set; }

        public double? Fps { get; private set; }

        public MediaFileInfo(string stdOut)
        {
            StdOut = stdOut;
            ParseStdOut();
        }

        private void ParseStdOut()
        {
            CreationTime = ParseCreationDate();
            Duration = ParseDuration();
            Bitrate = ParseBitrate();
            VideoCodec = ParseVideoCodec();
            AudioCodec = ParseAudioCodec();
            Resolution = ParseResolution();
            Fps = ParseFps();
        }

        private DateTime? ParseCreationDate()
        {
            var creationDateRegex = new Regex(@"(?m)(?i)creation_time .*: (.*)$");
            var creationMatch = creationDateRegex.Match(StdOut);
            var match = creationMatch.Groups[1].Value;
            if (DateTime.TryParse(match, out DateTime result))
            {
                return result;
            }
            return null;
        }

        private TimeSpan? ParseDuration()
        {
            var durationRegex = new Regex(@"(?m)(?i) duration: (.*), start");
            var durationMatch = durationRegex.Match(StdOut);
            var match = durationMatch.Groups[1].Value;
            if (TimeSpan.TryParse(match, out TimeSpan result))
            {
                return result;
            }
            return null;
        }

        private string ParseBitrate()
        {
            var bitrateRegex = new Regex(@"(?m)(?i) bitrate: (.*)$");
            var bitrateMatch = bitrateRegex.Match(StdOut);
            var match = bitrateMatch.Groups[1].Value;
            return match;
        }

        private VCodec? ParseVideoCodec()
        {
            var videoCodecRegex = new Regex(@"(?m)(?i): Video: (\w+)");
            var videoCodecMatch = videoCodecRegex.Match(StdOut);
            var match = videoCodecMatch.Groups[1].Value;
            if (Enum.TryParse(typeof(VCodec), match, true, out object result))
            {
                return (VCodec)result;
            }
            return null;
        }

        private ACodec? ParseAudioCodec()
        {
            var audioCodecRegex = new Regex(@"(?m)(?i): Audio: (\w+)");
            var audioCodecMatch = audioCodecRegex.Match(StdOut);
            var match = audioCodecMatch.Groups[1].Value;
            if (Enum.TryParse(typeof(ACodec), match, true, out object result))
            {
                return (ACodec)result;
            }
            return null;
        }

        private string ParseResolution()
        {
            var resolutionRegex = new Regex(@"(?m)(?i), (\d{3,5}x\d{3,5}),");
            var resolutionMatch = resolutionRegex.Match(StdOut);
            var match = resolutionMatch.Groups[1].Value;
            return match;
        }

        private double? ParseFps()
        {
            var fpsRegex = new Regex(@"(?m)(?i), (\d{1,}\.?\d{1,3}?) fps");
            var fpsMatch = fpsRegex.Match(StdOut);
            var match = fpsMatch.Groups[1].Value;
            if (double.TryParse(match, out double result))
            {
                return result;
            }
            return null;
        }
    }
}