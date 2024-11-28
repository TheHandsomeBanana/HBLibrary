using HBLibrary.Interface.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Workflows;
public class WorkflowProperty<TProperty> : IWorkflowProperty {
    public required string PropertyDisplayValue { get; set; }

    public TProperty Get(IWorkflowContext context) {
        WFPropertyAttribute wfAttribute = typeof(TProperty).GetCustomAttribute<WFPropertyAttribute>()
            ?? throw new InvalidOperationException("Cannot get property without attribute");

        return context.Resolve<TProperty>(wfAttribute.QualifiedName);
    }

    object IWorkflowProperty.Get(IWorkflowContext context) {
        return Get(context)!;
    }
}
