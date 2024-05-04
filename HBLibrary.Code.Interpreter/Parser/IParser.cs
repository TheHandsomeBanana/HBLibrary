using HBLibrary.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HBLibrary.Code.Interpreter.Parser;

public interface IParser<TSyntaxTree, TToken> where TSyntaxTree : ISyntaxTree where TToken : ISyntaxToken {
    TSyntaxTree Parse(ImmutableArray<TToken> tokens);
    ImmutableArray<SimpleError> GetSyntaxErrors(string content);
}
