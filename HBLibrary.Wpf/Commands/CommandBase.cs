using System;
using System.Windows.Input;

namespace HBLibrary.Wpf.Commands; 
public abstract class CommandBase : ICommand {
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter) {
        return true;
    }

    public abstract void Execute(object? parameter);
    public void NotifyCanExecuteChanged() {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
