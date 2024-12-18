﻿using Microsoft.CodeAnalysis;

namespace HBLibrary.Interface.CodeAnalysis;
public interface ICodeAnalyser {
    IAnalyserRegistry Registry { get; }
    Task<object> RunAsync(SyntaxNode syntaxNode);
}

public interface ICodeAnalyser<TResult> : ICodeAnalyser {
    /// <summary>
    /// Runs analysis for only 1 file given through provided <see cref="SyntaxNode"/>.<br></br>
    /// Analyser specific -> returns <typeparamref name="TResult"/> closest to snapshot in most cases. 
    /// </summary>
    /// <param name="syntaxNode"></param>
    /// <returns></returns>
    new Task<TResult> RunAsync(SyntaxNode snapshot);
}
