namespace HBLibrary.Wpf.Commands;
public class AsyncRelayCommand : AsyncCommandBase {
    private readonly Func<object?, Task> callback;
    private readonly Predicate<object?> canExecute;
    public AsyncRelayCommand(Func<object?, Task> callback, Predicate<object?> canExecute, Action<Exception> onException) : base(onException) {
        this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        this.canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter) {
        return canExecute != null ? canExecute(parameter) && base.CanExecute(parameter) : base.CanExecute(parameter);
    }

    protected override async Task ExecuteAsync(object? parameter) => await callback(parameter);
}

public class AsyncRelayCommand<TParameter> : AsyncCommandBase {
    private readonly Func<TParameter, Task> callback;
    private readonly Predicate<TParameter> canExecute;

    public AsyncRelayCommand(Func<TParameter, Task> callback, Predicate<TParameter> canExecute, Action<Exception> onException) : base(onException) {
        this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        this.canExecute = canExecute;
    }

    public AsyncRelayCommand(Func<TParameter, Task> callback, bool canExecute, Action<Exception> onException) : this(callback, o => canExecute, onException) { }

    public override bool CanExecute(object? parameter) {

        if (parameter is TParameter tParameter) {
            return canExecute != null
           ? canExecute(tParameter) && base.CanExecute(tParameter)
           : base.CanExecute(tParameter);
        }

        return false;
    }


    protected override Task ExecuteAsync(object? parameter) {
        if (parameter is TParameter tParameter) {
            return callback(tParameter);
        }

        throw new InvalidOperationException("Parameter does not match command type argument");
    }
}
