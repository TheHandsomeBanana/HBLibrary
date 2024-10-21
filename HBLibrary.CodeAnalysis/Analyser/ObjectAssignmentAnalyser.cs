using HBLibrary.Interface.CodeAnalysis;
using HBLibrary.Interface.CodeAnalysis.Results;
using Microsoft.CodeAnalysis;

namespace HBLibrary.Code.Analysis.Analyser;
public class ObjectAssignmentAnalyser : IObjectAssignmentAnalyser {
    public IAnalyserRegistry Registry => throw new NotImplementedException();

    public Task<ObjectAssignmentResult> RunAsync(SyntaxNode snapshot) {
        throw new NotImplementedException();
    }

    Task<object> ICodeAnalyser.RunAsync(SyntaxNode syntaxNode) {
        throw new NotImplementedException();
    }
}
