using NUnit.Framework;
using FFMpegWrapper;
using System.Threading.Tasks;
using System.IO;

[TestFixture]
internal class DownloadTest
{

    [TestCase]
    [Order(0)]
    [Ignore("Under development; don't need to test download every time")]
    public async Task TestDownloadWorksCorrectly()
    {
        if (Directory.Exists(Wrapper.FFMpegLocation))
        {
            Directory.Delete(Wrapper.FFMpegLocation, true);
        }
        await Wrapper.VerifyOrDownload();
        Assert.That(File.Exists(Path.Combine(Wrapper.FFMpegLocation, "readme.txt")), "FFMpeg did not download / extract correctly");
    }

    [TestCase]
    [Order(1)]
    public async Task TestDownloadDoesntHappenIfFileExists()
    {
        if(!File.Exists(Path.Combine(Wrapper.FFMpegLocation, "readme.txt")))
        {
            Assert.Fail("Files do not exist, test cannot fun");
        }
        FileInfo beforeInfo = new FileInfo(Path.Combine(Wrapper.FFMpegLocation, "readme.txt"));
        await Wrapper.VerifyOrDownload();
        if(!File.Exists(Path.Combine(Wrapper.FFMpegLocation, "readme.txt")))
        {
            Assert.Fail("File does not exist, test has failed");
        }
        FileInfo afterInfo = new FileInfo(Path.Combine(Wrapper.FFMpegLocation, "readme.txt"));
        Assert.AreEqual(beforeInfo.CreationTime, afterInfo.CreationTime);
    }
}