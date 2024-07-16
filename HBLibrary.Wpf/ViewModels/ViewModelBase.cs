using HBLibrary.Wpf.Services;
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

    // Use for TModel instantiation
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ViewModelBase() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected ViewModelBase(TModel model) {
        Model = model;
    }
}
