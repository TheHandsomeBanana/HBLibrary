using HBLibrary.NetFramework.Code.Analysis.Analyser;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public enum AnalyserFactoryScope {
        Solution,
        Project
    }

    public interface IAnalyserFactory {
        AnalyserFactoryScope Scope { get; }
        SemanticModelCache SemanticModelCache {get;}
        Solution Solution { get; }
        Project ScopedProject { get; }
        IImmutableSet<Document> Documents { get; }
        Task Init(Solution solution);
        Task Init(Project project);
        IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser();
    }
}
