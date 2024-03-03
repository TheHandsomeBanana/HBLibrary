using HBLibrary.Code.Interpreter.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Evaluator;
public interface ISemanticEvaluator<TSyntaxTree> where TSyntaxTree : ISyntaxTree {
    public ImmutableArray<SemanticError> Evaluate(TSyntaxTree syntaxTree, string content);
}
