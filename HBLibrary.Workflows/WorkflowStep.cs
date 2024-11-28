using HBLibrary.Interface.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Workflows;
public class WorkflowStep {
    public required string Name { get; set; }
    public List<IWorkflowProperty> AvailableProperties { get; } = [];
}
