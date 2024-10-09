using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HBLibrary.Wpf.ViewModels;
public abstract class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public abstract class ViewModelBase<TModel> : ViewModelBase {
    public TModel Model { get; protected set; }

    protected ViewModelBase(TModel model) {
        Model = model;
    }
}
