using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis.Analyser {
    public interface ICodeAnalyser {
        SyntaxNode GetSyntaxNode(SnapshotPoint snapshot);
        Task<object> Run(SyntaxNode syntaxNode);
    }

    public interface ICodeAnalyser<TResult> : ICodeAnalyser {
        /// <summary>
        /// Runs analysis for only 1 file given through provided <see cref="SyntaxNode"/>.<br></br>
        /// Analyser specific -> returns <typeparamref name="TResult"/> closest to snapshot in most cases. 
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        Task<TResult> Run(SnapshotPoint snapshot);
    }
}
