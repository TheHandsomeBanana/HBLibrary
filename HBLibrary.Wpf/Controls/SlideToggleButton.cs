using System;
using System.Collections.Generic;
using System.Linq;
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
public class SlideToggleButton : ToggleButton {
    static SlideToggleButton() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SlideToggleButton), new FrameworkPropertyMetadata(typeof(SlideToggleButton)));
    }

    public Effect BorderEffect {
        get { return (Effect)GetValue(BorderEffectProperty); }
        set { SetValue(BorderEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for BorderEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BorderEffectProperty =
        DependencyProperty.Register("BorderEffect", typeof(Effect), typeof(SlideToggleButton), new PropertyMetadata());

    public SolidColorBrush BackgroundChecked {
        get { return (SolidColorBrush)GetValue(BackgroundCheckedProperty); }
        set { SetValue(BackgroundCheckedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for BackgroundChecked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BackgroundCheckedProperty =
        DependencyProperty.Register("BackgroundChecked", typeof(SolidColorBrush), typeof(SlideToggleButton), new PropertyMetadata(Brushes.MediumSeaGreen));



    public SolidColorBrush BackgroundUnchecked {
        get { return (SolidColorBrush)GetValue(BackgroundUncheckedProperty); }
        set { SetValue(BackgroundUncheckedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for BackgroundUnchecked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BackgroundUncheckedProperty =
        DependencyProperty.Register("BackgroundUnchecked", typeof(SolidColorBrush), typeof(SlideToggleButton), new PropertyMetadata(Brushes.IndianRed));




    public CornerRadius CornerRadius {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SlideToggleButton), new PropertyMetadata(new CornerRadius(10)));





    public double SliderWidth {
        get { return (double)GetValue(SliderWidthProperty); }
        set { SetValue(SliderWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SliderWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderWidthProperty =
        DependencyProperty.Register("SliderWidth", typeof(double), typeof(SlideToggleButton), new PropertyMetadata(17.0));

    public double SliderHeight {
        get { return (double)GetValue(SliderHeightProperty); }
        set { SetValue(SliderHeightProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SliderHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderHeightProperty =
        DependencyProperty.Register("SliderHeight", typeof(double), typeof(SlideToggleButton), new PropertyMetadata(double.NaN));


    public Thickness SliderMargin {
        get { return (Thickness)GetValue(SliderMarginProperty); }
        set { SetValue(SliderMarginProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SliderMargin.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderMarginProperty =
        DependencyProperty.Register("SliderMargin", typeof(Thickness), typeof(SlideToggleButton), new PropertyMetadata(new Thickness(2)));



   

    public CornerRadius SliderCornerRadius {
        get { return (CornerRadius)GetValue(SliderCornerRadiusProperty); }
        set { SetValue(SliderCornerRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SliderCornerRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderCornerRadiusProperty =
        DependencyProperty.Register("SliderCornerRadius", typeof(CornerRadius), typeof(SlideToggleButton), new PropertyMetadata(new CornerRadius(10)));


    public SolidColorBrush SliderFill {
        get { return (SolidColorBrush)GetValue(SliderFillProperty); }
        set { SetValue(SliderFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EllipseFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderFillProperty =
        DependencyProperty.Register("SliderFill", typeof(SolidColorBrush), typeof(SlideToggleButton), new PropertyMetadata(Brushes.White));

    public SolidColorBrush SliderStroke {
        get { return (SolidColorBrush)GetValue(SliderStrokeProperty); }
        set { SetValue(SliderStrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EllipseStroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderStrokeProperty =
        DependencyProperty.Register("SliderStroke", typeof(SolidColorBrush), typeof(SlideToggleButton), new PropertyMetadata(Brushes.White));


    public Thickness SliderStrokeThickness {
        get { return (Thickness)GetValue(SliderStrokeThicknessProperty); }
        set { SetValue(SliderStrokeThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EllipseStrokeThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderStrokeThicknessProperty =
        DependencyProperty.Register("SliderStrokeThickness", typeof(Thickness), typeof(SlideToggleButton), new PropertyMetadata(new Thickness(0.2)));


    public Effect SliderEffect {
        get { return (Effect)GetValue(SliderEffectProperty); }
        set { SetValue(SliderEffectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EllipseEffect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SliderEffectProperty =
        DependencyProperty.Register("SliderEffect", typeof(Effect), typeof(SlideToggleButton), new PropertyMetadata());



    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        Border? border = GetTemplateChild("Border") as Border;
        Border? slider = GetTemplateChild("Slider") as Border;

        if (border is null || slider is null)
            return;

        UpdateVisualState(border, slider, IsChecked.GetValueOrDefault());

        this.Checked += (_, _) => AnimateToggle(border, slider, true);
        this.Unchecked += (_, _) => AnimateToggle(border, slider, false);
    }

    private void AnimateToggle(Border border, Border slider, bool isChecked) {
        ColorAnimation colorAnimation = new ColorAnimation {
            To = isChecked ? BackgroundChecked.Color : BackgroundUnchecked.Color,
            Duration = TimeSpan.FromSeconds(0.2)
        };

        border.Background = new SolidColorBrush(((SolidColorBrush)border.Background).Color); // Unfreeze background brush
        border.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

        // Create a new ThicknessAnimation to move the slider
        var thicknessAnimation = new ThicknessAnimation {
            To = isChecked 
                ? new Thickness(Width - (SliderWidth + SliderMargin.Left) - (BorderThickness.Left + BorderThickness.Right), SliderMargin.Top, SliderMargin.Right, SliderMargin.Bottom) 
                : SliderMargin,
            
            Duration = TimeSpan.FromSeconds(0.2)
        };

        slider.BeginAnimation(Border.MarginProperty, thicknessAnimation);
    }

    // Required for custom colors to load when template is applied
    private void UpdateVisualState(Border border, Border slider, bool isChecked) {
        border.Background = isChecked
            ? BackgroundChecked
            : BackgroundUnchecked;

        slider.Margin = isChecked
            ? new Thickness(Width - (SliderWidth + SliderMargin.Left) - (BorderThickness.Left + BorderThickness.Right), SliderMargin.Top, SliderMargin.Right, SliderMargin.Bottom)
            : SliderMargin;
    }
}
