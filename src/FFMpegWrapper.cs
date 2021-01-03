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

        public FileInfo InputFile { get; private set; }

        public FileInfo OutputFile { get; private set; }

        public VideoCodec OutputVideoCodec { get; private set; } = VideoCodec.none;

        public AudioCodec OutputAudioCodec { get; private set; } = AudioCodec.none;

        public CpuUsed ConverionCpusUsed { get; private set; } = CpuUsed.One;

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
            OutputFile = outputFile;
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

        public async Task<MediaFileInfo> GetInputInfo()
        {
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
    }
}