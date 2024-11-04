using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HBLibrary.Wpf.Controls;
public class ListBoxButtonV2 : ListBoxItem
{

    static ListBoxButtonV2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxButtonV2), new FrameworkPropertyMetadata(typeof(ListBoxButtonV2)));
    }

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(ListBoxButtonV2), new PropertyMetadata(null));

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(object), typeof(ListBoxButtonV2), new PropertyMetadata(null));


    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (Command != null && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
        }
    }


    public double ImageHeight
    {
        get { return (double)GetValue(ImageHeightProperty); }
        set { SetValue(ImageHeightProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(double), typeof(ListBoxButtonV2), new PropertyMetadata(0.0));


    public double ImageWidth
    {
        get { return (double)GetValue(ImageWidthProperty); }
        set { SetValue(ImageWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register("ImageWidth", typeof(double), typeof(ListBoxButtonV2), new PropertyMetadata(0.0));

    public Stretch ImageStretch
    {
        get { return (Stretch)GetValue(ImageStretchProperty); }
        set { SetValue(ImageStretchProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconStretch.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImageStretchProperty =
        DependencyProperty.Register("ImageStretch", typeof(Stretch), typeof(ListBoxButtonV2), new PropertyMetadata(Stretch.Uniform));


    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ListBoxButtonV2), new PropertyMetadata(null));


    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(ListBoxButtonV2), new PropertyMetadata(null));
}
