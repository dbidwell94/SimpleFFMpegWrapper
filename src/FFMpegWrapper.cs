using System;
using System.Diagnostics;
using FFMpegWrapper.Downloader;
using FFMpegWrapper.Options;
using FFMpegWrapper.Models;
using FFMpegWrapper.Utils;
using System.Threading.Tasks;
using System.IO;

namespace FFMpegWrapper
{
    public class Wrapper
    {
        public static string FFMpegLocation = FFMpegDownloader.FFMpegLocation;

        private FileInfo InputFile;

        private FileInfo OutputFile;

        private bool OverwriteOutputFile = false;

        private VideoCodec OutputVideoCodec = VideoCodec.copy;

        private AudioCodec OutputAudioCodec = AudioCodec.copy;

        private CpuUsed ConverionCpusUsed = CpuUsed.One;

        private FrameRate OutputFrameRate = FrameRate.DEFAULT;

        private Bitrate? OutputBitrate = null;

        private Resolution? OutputResolution = null;

        private CpuUsed NumCpuUsed = CpuUsed.One;

        private bool PipedOutput = false;

        #region Event System
        public delegate void ConversionEventHandler(object sender, ConversionEventArgs e);
        public event ConversionEventHandler OnConversionDidFinish;
        public event ConversionEventHandler OnConversionDidFail;
        public event ConversionEventHandler OnConversionDidStart;
        #endregion

        public Wrapper()
        {

        }

        public Wrapper Input(FileInfo inputFile)
        {
            if (inputFile.Exists)
            {
                InputFile = inputFile;
            }
            else
            {
                throw new FileNotFoundException($"{inputFile.Name} does not exist");
            }
            return this;
        }

        public Wrapper Output(FileInfo outputFile, bool overwrite = false)
        {
            if (overwrite && outputFile.Exists)
            {
                throw new FileLoadException($"{outputFile.Name} already exists and overwrite was not set to true");
            }
            PipedOutput = false;
            OutputFile = outputFile;
            OverwriteOutputFile = overwrite;
            return this;
        }

        public Wrapper OutputVideoFormat(VideoCodec videoCodec)
        {
            OutputVideoCodec = videoCodec;
            return this;
        }

        public Wrapper OutputAudioFormat(AudioCodec audioCodec)
        {
            OutputAudioCodec = audioCodec;
            return this;
        }

        public Wrapper UseCpus(CpuUsed cpus)
        {
            ConverionCpusUsed = cpus;
            return this;
        }

        public Wrapper OutputFramerate(FrameRate rate)
        {
            OutputFrameRate = rate;
            return this;
        }

        /// <summary>
        /// This method will ignore the video output and pipe the FFMpeg output directly to a StreamReader instead
        /// </summary>
        /// <returns>Realtime binary data from the conversion</returns>
        public StreamReader WithPipedOutput()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method will run in the background. Please subscribe to <see cref="OnConversionDidStart" />,
        /// <see cref="OnConversionDidFail" />, and <see cref="OnConversionDidFinish" /> events to get status updates and final
        /// processed data
        /// </summary>
        public void BuildAndRun()
        {
            Stopwatch watch = new Stopwatch();
            Task.Run(() =>
            {
                var processInfo = new ProcessStartInfo(Path.Combine(FFMpegLocation, FFMpegFilenames.FFMpeg));
                string builtArgs = "-i ";
                if (InputFile == null)
                {
                    throw new Exception("InputFile cannot be null");
                }
                if (OutputFile == null)
                {
                    throw new Exception("Output file cannot be null for BuildAndRun");
                }
                builtArgs += InputFile.FullName;
                if (OverwriteOutputFile)
                {
                    builtArgs += " -y";
                }
                builtArgs += $" -c:v {OutputVideoCodec.ToString()}";
                builtArgs += $" -c:a {OutputAudioCodec.ToString()}";
                if (OutputBitrate.HasValue)
                {
                    builtArgs += $" -b:v {OutputBitrate.Value.ToNormalizedString()}";
                }
                if (OutputFrameRate != FrameRate.DEFAULT)
                {
                    builtArgs += $" -r {(int)OutputFrameRate}";
                }
                builtArgs += $" -cpu-used {NumCpuUsed.GetCpuParam()}";
                if (OutputResolution.HasValue)
                {
                    builtArgs += $" -s {OutputResolution.Value.ToNormalizedString()}";
                }
                builtArgs += $" {OutputFile.FullName}";
                processInfo.Arguments = builtArgs;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardInput = true;
                processInfo.RedirectStandardOutput = true;
                var process = new Process();
                process.StartInfo = processInfo;
                watch.Start();
                process.Start();
                System.Console.WriteLine("Process started -- firing event");
                OnConversionDidStart?.Invoke(this, new ConversionEventArgs(InputFile, OutputFile));

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    OnConversionDidFail?.Invoke(this, new ConversionEventArgs(InputFile, OutputFile));
                };

                process.WaitForExit();
                watch.Stop();
            }).ContinueWith((res) =>
            {
                OnConversionDidFinish?.Invoke(this, new ConversionEventArgs(InputFile, OutputFile, watch.Elapsed));
            });
        }

        public async Task<MediaFileInfo> GetInputInfo()
        {
            if (!File.Exists(Path.Combine(FFMpegLocation, FFMpegFilenames.FFProbe)))
            {
                throw new FileNotFoundException("You do not have FFMpeg downloaded. Did you forget to call VerifyOrDownload()?");
            }
            if (InputFile == null)
            {
                throw new FileNotFoundException("InputFile not specified. To set call wrapper.Input(file)");
            }
            var startInfo = new ProcessStartInfo(Path.Combine(FFMpegLocation, FFMpegFilenames.FFProbe), InputFile.FullName);
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            Process infoProcess = new Process();
            infoProcess.StartInfo = startInfo;
            infoProcess.Start();
            return await StreamToMediaFileInfo.ParseStream(infoProcess.StandardError);
        }

        public static async Task VerifyOrDownload()
        {
            await FFMpegDownloader.DownloadAndVerifyAsync();
        }


        private static void LogEvent(object sender, ConversionEventArgs args)
        {
            System.Console.WriteLine("event fired");
        }
    }
}