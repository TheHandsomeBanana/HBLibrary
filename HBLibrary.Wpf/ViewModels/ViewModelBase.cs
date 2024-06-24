using HBLibrary.Wpf.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HBLibrary.Wpf.ViewModels;
public abstract class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void SaveState(IViewModelCache viewModelCache) {
        viewModelCache.AddOrUpdate(this);
    }
}

public abstract class ViewModelBase<TModel> : ViewModelBase where TModel : new() {
    public TModel Model { get; }

    public ViewModelBase() {
        Model = new TModel();
    }

    protected ViewModelBase(TModel model) {
        Model = model;
    }
}
