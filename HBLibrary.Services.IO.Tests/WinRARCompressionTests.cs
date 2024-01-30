using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Compression.WinRAR;

namespace HBLibrary.Services.IO.Tests;

[TestClass]
public class WinRARCompressionTests {
    private const string assets = "../../../assets/";

    [TestMethod]
    public void CompressFile_ReturnsCorrectResult() {
        IWinRARCompressionService service = new WinRARCompressionService();
        service.OnProcessExit += CorrectResult_OnProcessExit;
        service.OnOutputDataReceived += CorrectResult_OnOutputDataReceived;
        service.OnErrorDataReceived += CorrectResult_OnErrorDataReceived;

        service.Compress(assets + "compressableFile.txt", assets + "compressedFile.rar", WinRARCompressionSettings.Default);
    }

    [TestMethod]
    public void ExtractFile_ReturnsCorrectResult() {
        IWinRARCompressionService service = new WinRARCompressionService();
    }

    [TestMethod]
    public void CompressDirectory_ReturnsCorrectResult() {
        IWinRARCompressionService service = new WinRARCompressionService();
    }

    [TestMethod]
    public void ExtractDirectory_ReturnsCorrectResult() {
        IWinRARCompressionService service = new WinRARCompressionService();
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


    [TestMethod]
    public void Compress_InvalidSource_ShouldTriggerError() {
        string errorOutput = "";
        IWinRARCompressionService service = new WinRARCompressionService();
        service.OnErrorDataReceived += (sender, e) => {
            if (!string.IsNullOrEmpty(e.Data)) {
                errorOutput += e.Data + Environment.NewLine;
            }
        };

        service.OnProcessExit += (sender, e) => {
            Assert.AreNotEqual(0, e.ExitCode);
        };

        string invalidSource = @"C:\path\to\nonexistent\file.txt";
        string destinationArchive = @"C:\path\to\output\archive.rar";

        service.Compress(invalidSource, destinationArchive, WinRARCompressionSettings.Default);

        Assert.IsFalse(string.IsNullOrEmpty(errorOutput), "No error message was captured.");
    }
}