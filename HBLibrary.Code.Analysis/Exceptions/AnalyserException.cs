namespace HBLibrary.Code.Analysis.Exceptions;
public class AnalyserException : Exception {
    public AnalyserException(string message) : base(message) {
    }

    public static AnalyserException AnalyserRegistered(string name)
        => new AnalyserException($"{name} already registered.");
}
