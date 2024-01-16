using HBLibrary.NetFramework.Code.Analysis.Analyser;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public interface IAnalyserFactory {
        IAnalyserRegistry Registry { get; }
        TAnalyser CreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser;
        TAnalyser CreateAndRegisterAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser;
        IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser();
    }
}
