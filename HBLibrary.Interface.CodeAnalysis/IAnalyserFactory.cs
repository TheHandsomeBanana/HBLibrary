namespace HBLibrary.Interface.CodeAnalysis;
public interface IAnalyserFactory {
    IAnalyserRegistry Registry { get; }
    TAnalyser CreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser;
    TAnalyser GetOrCreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser;
    IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser();
    IObjectAssignmentAnalyser GetOrCreateObjectAssignmentAnalyser();
}
