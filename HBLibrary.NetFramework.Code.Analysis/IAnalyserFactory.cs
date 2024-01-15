using HBLibrary.NetFramework.Code.Analysis.Analyser;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
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
        Task<IAnalyserFactory> FromSolution(Solution solution);
        Task<IAnalyserFactory> FromProject(Project project);
        IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser();
    }
}
