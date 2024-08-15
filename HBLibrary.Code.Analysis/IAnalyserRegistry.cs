using HBLibrary.Code.Analysis.Analyser;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace HBLibrary.Code.Analysis;
public enum AnalysisScope {
    Solution,
    Project
}

public interface IAnalyserRegistry {
    IReadOnlyDictionary<string, ICodeAnalyser> RegisteredAnalyser { get; }
    AnalysisScope Scope { get; }
    SemanticModelCache SemanticModelCache { get; }
    Solution Solution { get; }
    Project ScopedProject { get; }
    IImmutableSet<Document> Documents { get; }
    Task InitAsync(Solution solution);
    Task InitAsync(Project project);
    void Register<TAnalyser>(TAnalyser analyser) where TAnalyser : ICodeAnalyser;
    TAnalyser Get<TAnalyser>() where TAnalyser : ICodeAnalyser;

}
