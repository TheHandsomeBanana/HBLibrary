using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace HBLibrary.Wpf.Behaviors;
public class TreeViewSelectedItemBehavior : Behavior<System.Windows.Controls.TreeView>
{
    public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TreeViewSelectedItemBehavior), new PropertyMetadata(null));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectedItemChanged += OnSelectedItemChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectedItemChanged -= OnSelectedItemChanged;
    }

    private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (Command != null && Command.CanExecute(e.NewValue))
        {
            Command.Execute(e.NewValue);
        }
    }
}
