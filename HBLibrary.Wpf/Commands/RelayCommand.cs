﻿namespace HBLibrary.Wpf.Commands;
public class RelayCommand : CommandBase {
    private readonly Action<object?> callback;
    private readonly Predicate<object?> canExecute;

    public RelayCommand(Action<object?> callback, Predicate<object?> canExecute) {
        this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        this.canExecute = canExecute;
    }

    public RelayCommand(Action<object?> callback) : this(callback, _ => true) { }

    public override bool CanExecute(object? parameter) {
        return canExecute != null ? canExecute(parameter) && base.CanExecute(parameter) : base.CanExecute(parameter);
    }

    public override void Execute(object? parameter) {
        callback(parameter);
    }
}

public class RelayCommand<TParameter> : CommandBase {
    private readonly Action<TParameter> callback;
    private readonly Predicate<TParameter> canExecute;

    public RelayCommand(Action<TParameter> callback, Predicate<TParameter?> canExecute) {
        this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        this.canExecute = canExecute;
    }

    public RelayCommand(Action<TParameter> callback) : this(callback, _ => true) { }

    public override bool CanExecute(object? parameter) {

        if (parameter is TParameter tParameter) {
            return canExecute != null
           ? canExecute(tParameter) && base.CanExecute(tParameter)
           : base.CanExecute(tParameter);
        }

        return false;
    }

    public override void Execute(object? parameter) {
        if (parameter is TParameter tParameter) {
            callback(tParameter);
        }
    }
}
