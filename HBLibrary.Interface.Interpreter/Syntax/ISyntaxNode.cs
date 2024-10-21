namespace HBLibrary.Interface.Interpreter.Syntax;
public interface ISyntaxNode { // Flag for constraint
    public TextSpan Span { get; }
    public LineSpan LineSpan { get; }
}

public interface ISyntaxNode<TNode, TToken> : ISyntaxNode where TNode : ISyntaxNode where TToken : ISyntaxToken {
    public TNode? Parent { get; }
    public IReadOnlyList<TNode> ChildNodes { get; }
    public IReadOnlyList<TToken> ChildTokens { get; }
    public void AddChildNode(TNode node);
    public void AddChildToken(TToken token);

    public TNode[] GetDescendantNodes();
}