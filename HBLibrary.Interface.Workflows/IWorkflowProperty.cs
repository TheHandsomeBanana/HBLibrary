using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Workflows;
public interface IWorkflowProperty {
    public object Get(IWorkflowContext context);
}
