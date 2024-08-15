using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR;
using HBLibrary.Services.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.Services.IO.Tests;

[TestClass]
public class WinRARAddCommandTests {
    private const string assets = "../../../assets/";

    [TestMethod]
    public void CompressFile_ReturnsCorrectResult() {
        WinRARAddCommand addCommand = new WinRARAddCommand() {
            TargetArchive = assets + "compressedFile.rar",
            Targets = [assets + "compressableFile.txt"]
        };


        WinRARArchiver archiver = new WinRARArchiver();

        archiver.OnProcessExit += CorrectResult_OnProcessExit;
        archiver.OnOutputDataReceived += CorrectResult_OnOutputDataReceived;
        archiver.OnErrorDataReceived += CorrectResult_OnErrorDataReceived;
        archiver.Execute(addCommand);
    }

    [TestMethod]
    public void ExtractFile_ReturnsCorrectResult() {
    }

    [TestMethod]
    public void CompressDirectory_ReturnsCorrectResult() {
    }

    [TestMethod]
    public void ExtractDirectory_ReturnsCorrectResult() {
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
}