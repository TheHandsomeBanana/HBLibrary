using HBLibrary.Services.IO.Compression.WinRAR;

namespace HBLibrary.Services.IO.Tests;

[TestClass]
public class WinRARCompressionTests {
    [TestMethod]
    public void CompressFile_ReturnsCorrectResult() {
        IWinRARCompressionService service = new WinRARCompressionService();
        service.OnProcessExit += CorrectResult_OnProcessExit;
        service.OnOutputDataReceived += CorrectResult_OnOutputDataReceived;
        service.OnErrorDataReceived += CorrectResult_OnErrorDataReceived;
        
    }

    private void CorrectResult_OnErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e) {
        Assert.Fail();
    }

    private void CorrectResult_OnOutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e) {
        throw new NotImplementedException();
    }

    private void CorrectResult_OnProcessExit(object? sender, Common.Process.ProcessExitEventArgs e) {
        throw new NotImplementedException();
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
}