using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
public class AnimatedPath : Control {
    static AnimatedPath() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedPath), new FrameworkPropertyMetadata(typeof(AnimatedPath)));
    }


    public AnimatedPath() {
    }

    ~AnimatedPath() {
        IsEnabledChanged -= AnimatedPath_IsEnabledChanged;
    }

    public bool State {
        get { return (bool)GetValue(StateProperty); }
        set { SetValue(StateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StateProperty =
        DependencyProperty.Register("State", typeof(bool), typeof(AnimatedPath), new PropertyMetadata(false, StatePropertyChanged));



    public Geometry Data {
        get { return (Geometry)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(Geometry), typeof(AnimatedPath), new PropertyMetadata(null));

    public SolidColorBrush Fill {
        get { return (SolidColorBrush)GetValue(FillProperty); }
        set { SetValue(FillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(Brushes.White));


    public SolidColorBrush Stroke {
        get { return (SolidColorBrush)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register("Stroke", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(Brushes.White));

    public double StrokeThickness {
        get { return (double)GetValue(StrokeThicknessProperty); }
        set { SetValue(StrokeThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register("StrokeThickness", typeof(double), typeof(AnimatedPath), new PropertyMetadata(0.0));

    public Stretch Stretch {
        get { return (Stretch)GetValue(StretchProperty); }
        set { SetValue(StretchProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AnimatedPath), new PropertyMetadata(Stretch.Uniform));


    public SolidColorBrush? FromFill {
        get { return (SolidColorBrush?)GetValue(FromFillProperty); }
        set { SetValue(FromFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromFillProperty =
        DependencyProperty.Register("FromFill", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));


    public SolidColorBrush? ToFill {
        get { return (SolidColorBrush?)GetValue(ToFillProperty); }
        set { SetValue(ToFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToFillProperty =
        DependencyProperty.Register("ToFill", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));

    public SolidColorBrush? FromStroke {
        get { return (SolidColorBrush?)GetValue(FromStrokeProperty); }
        set { SetValue(FromStrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromStroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromStrokeProperty =
        DependencyProperty.Register("FromStroke", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));


    public SolidColorBrush? ToStroke {
        get { return (SolidColorBrush?)GetValue(ToStrokeProperty); }
        set { SetValue(ToStrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToStroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToStrokeProperty =
        DependencyProperty.Register("ToStroke", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));


    public Duration AnimationDuration {
        get { return (Duration)GetValue(AnimationDurationProperty); }
        set { SetValue(AnimationDurationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnimationDurationProperty =
        DependencyProperty.Register("AnimationDuration", typeof(Duration), typeof(AnimatedPath), new PropertyMetadata((Duration)TimeSpan.FromSeconds(0.2)));

    public DropShadowEffect ShadowEffect {
        get { return (DropShadowEffect)GetValue(ShadowEffectProperty); }
        set { SetValue(ShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShadowEffectProperty =
        DependencyProperty.Register("ShadowEffect", typeof(DropShadowEffect), typeof(AnimatedPath), new PropertyMetadata(null));




    public SolidColorBrush? FromShadowEffect {
        get { return (SolidColorBrush?)GetValue(FromShadowEffectProperty); }
        set { SetValue(FromShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromShadowEffectProperty =
        DependencyProperty.Register("FromShadowEffect", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));



    public SolidColorBrush ToShadowEffect {
        get { return (SolidColorBrush)GetValue(ToShadowEffectProperty); }
        set { SetValue(ToShadowEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToShadowEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToShadowEffectProperty =
        DependencyProperty.Register("ToShadowEffect", typeof(SolidColorBrush), typeof(AnimatedPath), new PropertyMetadata(null));




    public double FromShadowBlurRadius {
        get { return (double)GetValue(FromShadowBlurRadiusProperty); }
        set { SetValue(FromShadowBlurRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FromShadowBlurRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FromShadowBlurRadiusProperty =
        DependencyProperty.Register("FromShadowBlurRadius", typeof(double), typeof(AnimatedPath), new PropertyMetadata(0.0));


    public double ToShadowBlurRadius {
        get { return (double)GetValue(ToShadowBlurRadiusProperty); }
        set { SetValue(ToShadowBlurRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ToShadowBlurRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToShadowBlurRadiusProperty =
        DependencyProperty.Register("ToShadowBlurRadius", typeof(double), typeof(AnimatedPath), new PropertyMetadata(0.0));



    private Path? icon;
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        IsEnabledChanged += AnimatedPath_IsEnabledChanged;

        icon = (Path)GetTemplateChild("Icon");

        if (IsEnabled) {
            UpdateVisualState(icon, State);
        }
    }

    private void AnimateIcon(bool stateTrue) {
        if (FromFill is not null && ToFill is not null) {

            ColorAnimation colorAnimation = new ColorAnimation {
                To = stateTrue ? FromFill.Color : ToFill.Color,
                Duration = AnimationDuration
            };

            icon!.Fill = new SolidColorBrush(((SolidColorBrush)icon.Fill).Color); // Unfreeze fill brush
            icon.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        if (FromStroke is not null && ToStroke is not null) {
            ColorAnimation colorAnimation = new ColorAnimation {
                To = stateTrue ? FromStroke.Color : ToStroke.Color,
                Duration = AnimationDuration
            };

            icon!.Stroke = new SolidColorBrush(((SolidColorBrush)icon.Stroke).Color); // Unfreeze stroke brush
            icon.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }


        if (FromShadowEffect is not null || ToShadowEffect is not null) {
            ColorAnimation colorAnimation = new ColorAnimation {
                To = stateTrue
                ? (FromShadowEffect?.Color ?? Fill.Color)
                : (ToShadowEffect?.Color ?? Fill.Color),

                Duration = AnimationDuration
            };

            ShadowEffect.BeginAnimation(DropShadowEffect.ColorProperty, colorAnimation);
        }



        DoubleAnimation doubleAnimation = new DoubleAnimation {
            To = stateTrue ? FromShadowBlurRadius : ToShadowBlurRadius,
            Duration = AnimationDuration
        };

        ShadowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, doubleAnimation);
    }

    private static void StatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        AnimatedPath path = (AnimatedPath)d;
        if (path.icon is not null) {
            path.AnimateIcon((bool)e.NewValue);
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
            DropShadowEffect temp = ShadowEffect;
            ShadowEffect = new DropShadowEffect {
                Color = state ? FromShadowEffect.Color : ToShadowEffect.Color,
                BlurRadius = state ? FromShadowBlurRadius : ToShadowBlurRadius,
                Direction = temp.Direction,
                ShadowDepth = temp.ShadowDepth,
                Opacity = temp.Opacity,
            };
        }
    }

    private void AnimatedPath_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
        if (IsEnabled && icon != null) {
            UpdateVisualState(icon, State);
        }
    }
}
