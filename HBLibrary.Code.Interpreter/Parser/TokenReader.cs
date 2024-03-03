using HBLibrary.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Parser;
public class TokenReader<TToken, TSyntaxKind> where TToken : ISyntaxToken
{
    public int CurrentIndex { get; private set; }
    public TToken? CurrentToken => CurrentIndex < tokens.Length ? tokens[CurrentIndex] : default;
    private ImmutableArray<TToken> tokens = [];
    public void Init(ImmutableArray<TToken> tokens)
    {
        this.tokens = tokens;
        CurrentIndex = 0;
    }

    public void MoveNext()
    {
        if (CanMoveNext())
            CurrentIndex++;
    }

    public bool CanMoveNext() => CurrentIndex < tokens.Length - 1;

    public TToken? GetNextToken()
    {
        if (CanMoveNext())
            MoveNext();

        return CurrentToken;
    }

    private TToken? GetTokenAt(int index) => CurrentIndex < tokens.Length ? tokens[index] : default;

    public TextSpan? GetCurrentFullSpan() => CurrentToken?.FullSpan;
    public LineSpan? GetCurrentLineSpan() => CurrentToken?.LineSpan;

    public TToken GetLastValidToken()
    {
        int index = CurrentIndex;
        while (GetTokenAt(index) == null && index > 0)
            index--;

        return GetTokenAt(index)!;
    }

    public bool CanMoveBack() => CurrentIndex - 1 > 0;
    public void MoveBack()
    {
        if (CanMoveBack())
            CurrentIndex--;
    }

    public TToken? GetPreviousToken()
    {
        CurrentIndex--;
        return CurrentToken;
    }

    public TToken? PeekNextToken() => GetTokenAt(CurrentIndex + 1);
}
