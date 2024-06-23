using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;
public class TreeViewItem : ViewModelBase {
    private string? name;
    public string? Name {
        get { 
            return name; 
        }
        set {
            name = value;
            NotifyPropertyChanged();
        }
    }

    private Uri? navLink;
    public Uri? NavLink {
        get => navLink;
        set {
            navLink = value;
            NotifyPropertyChanged();
        }
    }

    private ObservableCollection<TreeViewItem> children = [];
    public ObservableCollection<TreeViewItem> Children {
        get => children;
        set {
            children = value;
            NotifyPropertyChanged();
        }
    }

}
