using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using FFMpegWrapper;
using FFMpegWrapper.Options;
using System.IO;

[TestFixture]
internal class ConverterTests
{

    FileInfo InputFile;
    FileInfo OutputFile;

    AutoResetEvent ConversionStartedEvent;
    AutoResetEvent ConversionFailedEvent;
    AutoResetEvent ConversionCompletedEvent;

    private async Task WaitForEvent(AutoResetEvent e, string eventName, int secondTimeout = 30)
    {
        await Task.Run(() =>
        {
            while (!e.WaitOne(secondTimeout * 1000, false))
            {
                Assert.Fail($"{eventName} did not get invoked");
            }
        });
    }

    private void HandleConversionFailedEvent(object sender, ConversionEventArgs args)
    {
        ConversionFailedEvent.Set();
    }

    private void HandleConversionStartedEvent(object sender, ConversionEventArgs args)
    {
        ConversionStartedEvent.Set();
    }

    private void HandleConversionCompletedEvent(object sender, ConversionEventArgs args)
    {
        ConversionCompletedEvent.Set();
    }

    [SetUp]
    public void Setup()
    {
        ConversionFailedEvent = new AutoResetEvent(false);
        ConversionCompletedEvent = new AutoResetEvent(false);
        ConversionStartedEvent = new AutoResetEvent(false);
        OutputFile = new FileInfo(Path.Combine("./Resources/convertedBunny.webm"));
        InputFile = new FileInfo(Path.Combine("./Resources/bunny.webm"));
    }

    [TearDown]
    public void TearDown()
    {
        if (OutputFile.Exists)
        {
            File.Delete(OutputFile.FullName);
        }
    }

    [TestCase]
    [Order(0)]
    public async Task TestConversionVP8()
    {
        var wrapper = new Wrapper();
        wrapper.Input(new FileInfo("./Resources/bunny.webm"))
            .Output(new FileInfo("./Resources/convertedBunny.webm"))
            .UseCpus(CpuUsed.AllAvailable)
            .OutputAudioFormat(AudioCodec.copy)
            .OutputVideoFormat(VideoCodec.vp8)
            .OutputFramerate(FrameRate.FPS_10);

        // Get metainfo about the input file
        var info = await wrapper.GetInputInfo();

        Assert.That(!OutputFile.Exists, "The output file already exists.");
        Assert.That(InputFile.Exists, "There is no input file for the test");

        wrapper.OnConversionDidStart += HandleConversionStartedEvent;
        wrapper.OnConversionDidFinish += HandleConversionCompletedEvent;
        wrapper.OnConversionDidFail += HandleConversionFailedEvent;
        wrapper.BuildAndRun();

        await WaitForEvent(ConversionStartedEvent, "ConversionStartedEvent", 2);
        await WaitForEvent(ConversionCompletedEvent, "Conversion Completed");

        OutputFile = new FileInfo(OutputFile.FullName);

        Assert.That(OutputFile.Exists, "The output file did not get created..");
        var infoWrapper = new Wrapper();
        infoWrapper.Input(OutputFile);

        // Get metainfo about newly created output file
        var outputInfo = await infoWrapper.GetInputInfo();

        // Check the media codecs are what we expect
        Assert.AreEqual(VideoCodec.vp8, outputInfo.VideoCodec);
        Assert.AreEqual(info.AudioCodec, outputInfo.AudioCodec);

        // Check the framerate got encoded correctly
        Assert.AreEqual((double)FrameRate.FPS_10, outputInfo.Fps);
    }
}
