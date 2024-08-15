using HBLibrary.Code.Analysis.Analyser;
using HBLibrary.Code.Analysis.Exceptions;

namespace HBLibrary.Code.Analysis;
public class AnalyserFactory : IAnalyserFactory {
    public IAnalyserRegistry Registry { get; }
    public TAnalyser CreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser => Activator.CreateInstance<TAnalyser>();

    public TAnalyser GetOrCreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser {
        string typeName = typeof(TAnalyser).FullName;
        if (Registry.RegisteredAnalyser.ContainsKey(typeName))
            throw AnalyserException.AnalyserRegistered(typeName);

        TAnalyser analyser = CreateAnalyser<TAnalyser>();
        Registry.Register(analyser);
        return analyser;
    }

    public IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser() {
        return new ObjectAssignmentAnalyser();
    }

    public IObjectAssignmentAnalyser GetOrCreateObjectAssignmentAnalyser() {
        if (Registry.RegisteredAnalyser.ContainsKey(nameof(IObjectAssignmentAnalyser)))
            throw AnalyserException.AnalyserRegistered(nameof(IObjectAssignmentAnalyser));

        ObjectAssignmentAnalyser analyser = new ObjectAssignmentAnalyser();
        Registry.Register(analyser);
        return analyser;
    }
}

