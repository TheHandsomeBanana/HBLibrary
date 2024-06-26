using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;
public class TreeViewItem {
    public string Name { get; set; }
    public Type ViewModelType { get; set; }
    public ObservableCollection<TreeViewItem> Children { get; set; } = [];

    public TreeViewItem(string name, Type viewModelType) {
        Name = name;
        ViewModelType = viewModelType;
    }
}
