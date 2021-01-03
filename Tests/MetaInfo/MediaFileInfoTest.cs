using NUnit.Framework;
using System;
using System.IO;
using FFMpegWrapper;

[TestFixture]
public class MediaFileInfoTest
{
    [TestCase]
    public void TestThing()
    {
        var Wrapper = new Wrapper();
        Wrapper.Input(new FileInfo("./Resources/bunny.webm"));
    }
}