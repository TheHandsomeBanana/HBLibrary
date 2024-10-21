using HBLibrary.Interface.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HBLibrary.Interface.Interpreter.Lexer;
public interface ILexer<TToken> where TToken : ISyntaxToken {
    public ImmutableArray<TToken> Lex(string input);
    public ImmutableArray<SimpleError> GetSyntaxErrors();
}
