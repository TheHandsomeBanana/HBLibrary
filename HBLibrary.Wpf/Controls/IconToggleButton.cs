using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HBLibrary.Wpf.Controls;

public class IconToggleButton : ToggleButton {
    static IconToggleButton() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IconToggleButton), new FrameworkPropertyMetadata(typeof(IconToggleButton)));
    }

    ~IconToggleButton() {
        IsEnabledChanged -= IconToggleButton_IsEnabledChanged;
    }

    public Geometry Data {
        get { return (Geometry)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(Geometry), typeof(IconToggleButton), new PropertyMetadata(null));

    public SolidColorBrush Fill {
        get { return (SolidColorBrush)GetValue(FillProperty); }
        set { SetValue(FillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(Brushes.White));


    public SolidColorBrush Stroke {
        get { return (SolidColorBrush)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register("Stroke", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(Brushes.White));

    public double StrokeThickness {
        get { return (double)GetValue(StrokeThicknessProperty); }
        set { SetValue(StrokeThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register("StrokeThickness", typeof(double), typeof(IconToggleButton), new PropertyMetadata(0.0));

    public Stretch Stretch {
        get { return (Stretch)GetValue(StretchProperty); }
        set { SetValue(StretchProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register("Stretch", typeof(Stretch), typeof(IconToggleButton), new PropertyMetadata(Stretch.Uniform));


    public double IconWidth {
        get { return (double)GetValue(IconWidthProperty); }
        set { SetValue(IconWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconWidthProperty =
        DependencyProperty.Register("IconWidth", typeof(double), typeof(IconToggleButton), new PropertyMetadata(15.0));


    public double IconHeight {
        get { return (double)GetValue(IconHeightProperty); }
        set { SetValue(IconHeightProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconHeightProperty =
        DependencyProperty.Register("IconHeight", typeof(double), typeof(IconToggleButton), new PropertyMetadata(15.0));


    public SolidColorBrush? FromFill {
        get { return (SolidColorBrush?)GetValue(FromFillProperty); }
        set { SetValue(FromFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromFillProperty =
        DependencyProperty.Register("FromFill", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));


    public SolidColorBrush? ToFill {
        get { return (SolidColorBrush?)GetValue(ToFillProperty); }
        set { SetValue(ToFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToFillProperty =
        DependencyProperty.Register("ToFill", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));

    public SolidColorBrush? FromStroke {
        get { return (SolidColorBrush?)GetValue(FromStrokeProperty); }
        set { SetValue(FromStrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromStroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromStrokeProperty =
        DependencyProperty.Register("FromStroke", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));


    public SolidColorBrush? ToStroke {
        get { return (SolidColorBrush?)GetValue(ToStrokeProperty); }
        set { SetValue(ToStrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToStroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToStrokeProperty =
        DependencyProperty.Register("ToStroke", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));


    public Duration AnimationDuration {
        get { return (Duration)GetValue(AnimationDurationProperty); }
        set { SetValue(AnimationDurationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnimationDurationProperty =
        DependencyProperty.Register("AnimationDuration", typeof(Duration), typeof(IconToggleButton), new PropertyMetadata((Duration)TimeSpan.FromSeconds(0.2)));

    public DropShadowEffect ShadowEffect {
        get { return (DropShadowEffect)GetValue(ShadowEffectProperty); }
        set { SetValue(ShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShadowEffectProperty =
        DependencyProperty.Register("ShadowEffect", typeof(DropShadowEffect), typeof(IconToggleButton), new PropertyMetadata(null));




    public SolidColorBrush? FromShadowEffect {
        get { return (SolidColorBrush?)GetValue(FromShadowEffectProperty); }
        set { SetValue(FromShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromShadowEffectProperty =
        DependencyProperty.Register("FromShadowEffect", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));



    public SolidColorBrush ToShadowEffect {
        get { return (SolidColorBrush)GetValue(ToShadowEffectProperty); }
        set { SetValue(ToShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToShadowEffectProperty =
        DependencyProperty.Register("ToShadowEffect", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(null));




    public SolidColorBrush AnimatedShadowBrush {
        get { return (SolidColorBrush)GetValue(AnimatedShadowBrushProperty); }
        set { SetValue(AnimatedShadowBrushProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AnimatedShadowBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnimatedShadowBrushProperty =
        DependencyProperty.Register("AnimatedShadowBrush", typeof(SolidColorBrush), typeof(IconToggleButton), new PropertyMetadata(Brushes.Transparent, OnAnimatedShadowBrushChange));

    // Required to change the color of the shadow
    private static void OnAnimatedShadowBrushChange(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        IconToggleButton control = (IconToggleButton)d;
        SolidColorBrush? brush = e.NewValue as SolidColorBrush;

        if (control.ShadowEffect is not null && brush is not null) {
            control.ShadowEffect.Color = brush.Color;
        }
    }

    private Path? icon;
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        IsEnabledChanged += IconToggleButton_IsEnabledChanged;

        icon = GetTemplateChild("Icon") as Path;

        if (icon is null) {
            return;
        }

        
        this.Checked += (_, _) => AnimateIcon(icon, true);
        this.Unchecked += (_, _) => AnimateIcon(icon, false);
    }

    private void IconToggleButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
        if (IsEnabled && icon is not null) {
            UpdateVisualState(icon, IsChecked.GetValueOrDefault());
        }
    }

    private void AnimateIcon(Path icon, bool state) {
        if (!IsEnabled) {
            return;
        }

        if (FromFill is not null && ToFill is not null) {

            ColorAnimation colorAnimation = new ColorAnimation {
                To = state ? FromFill.Color : ToFill.Color,
                Duration = AnimationDuration
            };

            icon.Fill = new SolidColorBrush(((SolidColorBrush)icon.Fill).Color); // Unfreeze fill brush
            icon.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        if (FromStroke is not null && ToStroke is not null) {
            ColorAnimation colorAnimation = new ColorAnimation {
                To = state ? FromStroke.Color : ToStroke.Color,
                Duration = AnimationDuration
            };

            icon.Stroke = new SolidColorBrush(((SolidColorBrush)icon.Stroke).Color); // Unfreeze stroke brush
            icon.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        if (FromShadowEffect is not null && ToShadowEffect is not null) {
            ColorAnimation colorAnimation = new ColorAnimation {
                To = state ? FromShadowEffect.Color : ToShadowEffect.Color,
                Duration = AnimationDuration
            };

            AnimatedShadowBrush = new SolidColorBrush(AnimatedShadowBrush.Color);
            AnimatedShadowBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
    }

    // Required for custom colors to load when template is applied
    private void UpdateVisualState(Path icon, bool state) {
        if (FromFill is not null && ToFill is not null) {
            icon.Fill = state
                ? FromFill
                : ToFill;
        }

        if (FromStroke is not null && ToStroke is not null) {
            icon.Stroke = state
                ? FromStroke
                : ToStroke;
        }

        if (FromShadowEffect is not null && ToShadowEffect is not null) {
            ShadowEffect.Color = state
                ? FromShadowEffect.Color
                : ToShadowEffect.Color;
        }

    }
}
