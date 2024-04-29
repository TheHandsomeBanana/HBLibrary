using HBLibrary.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HBLibrary.Code.Interpreter.Lexer;
public interface ILexer<TToken> where TToken : ISyntaxToken {
    public ImmutableArray<TToken> Lex(string input);
    public ImmutableArray<SimpleError> GetSyntaxErrors();
}
