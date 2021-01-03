using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

using VCodec = FFMpegWrapper.Options.VideoCodec;
using ACodec = FFMpegWrapper.Options.AudioCodec;

namespace FFMpegWrapper.Models
{
    public sealed class MediaFileInfo
    {
        private string StdOut;

        public DateTime CreationTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string Bitrate { get; private set; }

        public VCodec VideoCodec { get; private set; }

        public ACodec AudioCodec { get; private set; }

        public string Resolution { get; private set; }

        public int Fps { get; private set; }

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

        private DateTime ParseCreationDate()
        {
            var creationDateRegex = new Regex(@"(?m)(?i)creation_time .*: (.*)$");
            var creationMatch = creationDateRegex.Match(StdOut);
            var match = creationMatch.Groups[1].Value;
            return DateTime.Parse(match);
        }

        private TimeSpan ParseDuration()
        {
            var durationRegex = new Regex(@"(?m)(?i) duration: (.*), start");
            var durationMatch = durationRegex.Match(StdOut);
            var match = durationMatch.Groups[1].Value;
            return TimeSpan.Parse(match);
        }

        private string ParseBitrate()
        {
            var bitrateRegex = new Regex(@"(?m)(?i) bitrate: (.*)$");
            var bitrateMatch = bitrateRegex.Match(StdOut);
            var match = bitrateMatch.Groups[1].Value;
            return match;
        }

        private VCodec ParseVideoCodec()
        {
            var videoCodecRegex = new Regex(@"(?m)(?i): Video: (\w+)");
            var videoCodecMatch = videoCodecRegex.Match(StdOut);
            var match = videoCodecMatch.Groups[1].Value;
            return (VCodec)Enum.Parse(typeof(VCodec), match);
        }

        private ACodec ParseAudioCodec()
        {
            var audioCodecRegex = new Regex(@"(?m)(?i): Audio: (\w+)");
            var audioCodecMatch = audioCodecRegex.Match(StdOut);
            var match = audioCodecMatch.Groups[1].Value;
            return (ACodec)Enum.Parse(typeof(ACodec), match);
        }

        private string ParseResolution()
        {
            var resolutionRegex = new Regex(@"(?m)(?i), (\d{3,5}x\d{3,5}),");
            var resolutionMatch = resolutionRegex.Match(StdOut);
            var match = resolutionMatch.Groups[1].Value;
            return match;
        }

        private int ParseFps()
        {
            var fpsRegex = new Regex(@"(?m)(?i), (\d{1,}) fps");
            var fpsMatch = fpsRegex.Match(StdOut);
            var match = fpsMatch.Groups[1].Value;
            return int.Parse(match);
        }
    }
}