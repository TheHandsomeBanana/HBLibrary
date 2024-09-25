using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Behaviors;
public interface IDragDropTarget {
    public void MoveItem(object source, object target);
}
