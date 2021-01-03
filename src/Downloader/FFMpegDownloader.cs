using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using System.IO.Compression;

namespace FFMpegWrapper.Downloader
{
    internal static class FFMpegDownloader
    {
        internal static string FFMpegLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFMpeg");

        public static async Task DownloadAndVerifyAsync()
        {
            string[] existingFiles;
            string[] existingFolders;
            await Task.Run(() =>
            {
                var httpClient = new WebClient();
                string downloadUrl;
                string fileName;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    downloadUrl = UrlSources.Linux;
                    fileName = UrlSources.LinuxFileName;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    downloadUrl = UrlSources.Windows;
                    fileName = UrlSources.WindowsFileName;
                }
                else
                {
                    throw new PlatformNotSupportedException("Unable to find a subtible version of FFMpeg for your system");
                }

                if (!Directory.Exists(FFMpegLocation))
                {
                    Directory.CreateDirectory(FFMpegLocation);
                }
                if (File.Exists(Path.Combine(FFMpegLocation, "readme.txt")))
                {
                    return;
                }

                httpClient.DownloadFile(downloadUrl, Path.Combine(FFMpegLocation, fileName));
                ZipFile.ExtractToDirectory(Path.Combine(FFMpegLocation, fileName), Path.Combine(FFMpegLocation, "FFMpeg"));
                existingFiles = Directory.GetFiles(FFMpegLocation);
                existingFolders = Directory.GetDirectories(FFMpegLocation);
                string cd = Path.Combine(FFMpegLocation, "FFMpeg");
                while (true)
                {
                    System.Console.WriteLine($"Directories: {Directory.GetDirectories(cd).Length}, Files: {Directory.GetFiles(cd).Length}");
                    if (Directory.GetDirectories(cd).Length == 0 && Directory.GetFiles(cd).Length == 0)
                    {
                        throw new Exception("Unable to find FFMpeg data");
                    }
                    if (SourceCodeExistsHere(cd))
                    {
                        break;
                    }
                    else
                    {
                        cd = Path.Combine(cd, Directory.GetDirectories(cd)[0]);
                    }
                }
                ExtractSourceCodeIntoRootDirectory(cd);
                foreach (var dir in existingFolders)
                {
                    Directory.Delete(dir, true);
                }
                foreach (var file in existingFiles)
                {
                    File.Delete(file);
                }
            });
        }

        private static void ExtractSourceCodeIntoRootDirectory(string currentSourcePath)
        {
            foreach (var file in Directory.GetFiles(currentSourcePath))
            {
                var info = new FileInfo(file);
                File.Move(file, Path.Combine(FFMpegLocation, info.Name));
            }
            foreach (var dir in Directory.GetDirectories(currentSourcePath))
            {
                var info = new DirectoryInfo(dir);
                Directory.Move(dir, Path.Combine(FFMpegLocation, info.Name));
            }
        }

        private static bool SourceCodeExistsHere(string path)
        {
            if (File.Exists(Path.Combine(path, "readme.txt")))
            {
                return true;
            }
            return false;
        }
    }
}
