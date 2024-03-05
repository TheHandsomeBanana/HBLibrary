using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving;
using HBLibrary.Services.IO.Archiving.WinRAR;
using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using HBLibrary.Services.IO.Compression.WinRAR;

namespace HBLibrary.Services.IO.Tests;

[TestClass]
public class WinRARCompressionTests {
    private const string assets = "../../../assets/";

    [TestMethod]
    public void CompressFile_ReturnsCorrectResult() {
        IWinRARCompressor service = new WinRARCompressor();
        service.OnProcessExit += CorrectResult_OnProcessExit;
        service.OnOutputDataReceived += CorrectResult_OnOutputDataReceived;
        service.OnErrorDataReceived += CorrectResult_OnErrorDataReceived;
        Archive archive = new Archive(assets + "compressedFile.rar");
        archive.Files.Add(FileSnapshot.Create(assets + "compressableFile.txt", true));
        service.Compress(archive, WinRARCompressionOptions.Default);
    }

    [TestMethod]
    public void ExtractFile_ReturnsCorrectResult() {
        IWinRARCompressor service = new WinRARCompressor();
    }

    [TestMethod]
    public void CompressDirectory_ReturnsCorrectResult() {
        IWinRARCompressor service = new WinRARCompressor();
    }

    [TestMethod]
    public void ExtractDirectory_ReturnsCorrectResult() {
        IWinRARCompressor service = new WinRARCompressor();
    }

    private void CorrectResult_OnErrorDataReceived(object? sender, ProcessStdStreamEventArgs e) {
        Console.WriteLine(e.Data);
    }

    private void CorrectResult_OnOutputDataReceived(object? sender, ProcessStdStreamEventArgs e) {
        Console.WriteLine(e.Data);
    }

    private void CorrectResult_OnProcessExit(object? sender, Common.Process.ProcessExitEventArgs e) {
        Assert.AreEqual(0, e.ExitCode);
        Console.WriteLine("Exit Code:" + e.ExitCode + "; Duration: " + e.Duration);
    }


    //[TestMethod]
    //public void Compress_InvalidSource_ShouldTriggerError() {
    //    string errorOutput = "";
    //    IWinRARCompressor service = new WinRARCompressor();
    //    service.OnErrorDataReceived += (sender, e) => {
    //        if (!string.IsNullOrEmpty(e.Data)) {
    //            errorOutput += e.Data + Environment.NewLine;
    //        }
    //    };

    //    service.OnProcessExit += (sender, e) => {
    //        Assert.AreNotEqual(0, e.ExitCode);
    //    };

    //    string invalidSource = @"C:\path\to\nonexistent\file.txt";
    //    string destinationArchive = @"C:\path\to\output\archive.rar";

    //    Archive archive = new Archive(destinationArchive);
    //    archive.Files.Add(FileSnapshot.Create(invalidSource));

    //    service.Compress(archive, WinRARCompressionSettings.Default);

    //    Assert.IsFalse(string.IsNullOrEmpty(errorOutput), "No error message was captured.");
    //}
}