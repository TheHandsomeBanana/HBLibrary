using System.Windows;
using System.Windows.Media;

namespace HBLibrary.Wpf.Controls;
public class IconButton : System.Windows.Controls.Button {
    static IconButton() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
    }

    public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register(nameof(IconData), typeof(Geometry), typeof(IconButton), new PropertyMetadata(null));

    public Geometry IconData {
        get => (Geometry)GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public static readonly DependencyProperty IconHeightProperty =
        DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(IconButton), new PropertyMetadata(24.0));

    public double IconHeight {
        get => (double)GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }

    public static readonly DependencyProperty IconWidthProperty =
        DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(IconButton), new PropertyMetadata(24.0));

    public double IconWidth {
        get => (double)GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }

    public static readonly DependencyProperty IconFillProperty =
        DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(IconButton), new PropertyMetadata(Brushes.Black));

    public Brush IconFill {
        get => (Brush)GetValue(IconFillProperty);
        set => SetValue(IconFillProperty, value);
    }

    public static readonly DependencyProperty IconStrokeProperty =
        DependencyProperty.Register(nameof(IconStroke), typeof(Brush), typeof(IconButton), new PropertyMetadata(null));

    public Brush IconStroke {
        get => (Brush)GetValue(IconStrokeProperty);
        set => SetValue(IconStrokeProperty, value);
    }

    public static readonly DependencyProperty IconStrokeThicknessProperty =
        DependencyProperty.Register(nameof(IconStrokeThickness), typeof(double), typeof(IconButton), new PropertyMetadata(0.0));

    public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.Register(nameof(IconStretch), typeof(Stretch), typeof(IconButton), new PropertyMetadata(Stretch.None));

    public Stretch IconStretch {
        get => (Stretch)GetValue(IconStretchProperty);
        set => SetValue(IconStretchProperty, value);
    }

    public double IconStrokeThickness {
        get => (double)GetValue(IconStrokeThicknessProperty);
        set => SetValue(IconStrokeThicknessProperty, value);
    }
}
