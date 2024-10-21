using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Interpreter;
public interface IInterpreter {
    public void Run(string input);
    public void RunFromFile(string filePath);
    public ImmutableArray<string> GetErrors();
}
