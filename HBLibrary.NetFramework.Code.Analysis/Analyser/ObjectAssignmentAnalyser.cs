using HBLibrary.NetFramework.Code.Analysis.Analyser.Results;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis.Analyser {
    public class ObjectAssignmentAnalyser : IObjectAssignmentAnalyser {
        public IAnalyserRegistry Registry => throw new NotImplementedException();

        public Task<ObjectAssignmentResult> RunAsync(SyntaxNode snapshot) {
            throw new NotImplementedException();
        }

        Task<object> ICodeAnalyser.RunAsync(SyntaxNode syntaxNode) {
            throw new NotImplementedException();
        }
    }
}
