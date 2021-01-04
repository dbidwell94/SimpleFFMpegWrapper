using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;
using System;
using FFMpegWrapper;
using FFMpegWrapper.Options;

[TestFixture]
internal class MediaFileInfoTest
{
    [TestCase]
    public async Task TestFileInfoProcessing()
    {
        var wrapper = new Wrapper();
        wrapper.Input(new FileInfo("./Resources/bunny.webm"));
        var info = await wrapper.GetInputInfo();
        Assert.AreEqual(VideoCodec.vp8, info.VideoCodec);
        Assert.AreEqual(AudioCodec.vorbis, info.AudioCodec);
        Assert.AreEqual(25, info.Fps);
        Assert.AreEqual("640x360", info.Resolution);
        Assert.AreEqual(TimeSpan.Parse("00:00:32.48"), info.Duration);
        Assert.AreEqual(DateTime.Parse("2010-05-20T08:21:12.000000Z"), info.CreationTime);
        Assert.AreEqual("533 kb/s", info.Bitrate);
    }
}