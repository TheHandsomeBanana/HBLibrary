using HBLibrary.Interface.Interpreter.Syntax;
using System.Collections.Immutable;

namespace HBLibrary.Interface.Interpreter.Evaluator;
public interface ISemanticEvaluator<TSyntaxTree> where TSyntaxTree : ISyntaxTree {
    public ImmutableArray<SimpleError> Evaluate(TSyntaxTree syntaxTree, string content);
}
