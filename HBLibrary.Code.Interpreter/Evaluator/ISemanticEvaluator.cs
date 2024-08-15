using HBLibrary.Code.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HBLibrary.Code.Interpreter.Evaluator;
public interface ISemanticEvaluator<TSyntaxTree> where TSyntaxTree : ISyntaxTree {
    public ImmutableArray<SimpleError> Evaluate(TSyntaxTree syntaxTree, string content);
}
