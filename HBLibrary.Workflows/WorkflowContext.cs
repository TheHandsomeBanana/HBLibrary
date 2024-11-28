using HBLibrary.Interface.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Workflows;
public class WorkflowContext : IWorkflowContext {
    public T Resolve<T>(string qualifiedName) {
        throw new NotImplementedException();
    }
}
