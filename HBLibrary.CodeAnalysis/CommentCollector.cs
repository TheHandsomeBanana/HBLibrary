using Microsoft.CodeAnalysis;

/* Unmerged change from project 'HBLibrary.Code.Analysis (net8.0)'
Before:
using Microsoft.CodeAnalysis;
After:
using Microsoft.CodeAnalysis.CSharp;
*/
using Microsoft.CodeAnalysis.CSharp;

namespace HBLibrary.Code.Analysis;
public class CommentCollector : CSharpSyntaxWalker {
    public List<string> Comments { get; } = [];

    public override void VisitTrivia(SyntaxTrivia trivia) {
        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            Comments.Add(trivia.ToString());

        // Continue traversing the syntax tree
        base.VisitTrivia(trivia);
    }
}
