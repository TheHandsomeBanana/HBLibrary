using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
