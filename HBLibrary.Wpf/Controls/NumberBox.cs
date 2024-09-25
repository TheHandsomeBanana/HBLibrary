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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HBLibrary.Wpf.Controls;

public class NumberBox : Control {
    static NumberBox() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
    }

    public int Value {
        get { return (int)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(int), typeof(NumberBox), new FrameworkPropertyMetadata(0, OnValueChanged));

   
    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        NumberBox numberBox = (NumberBox)d;

        if(!int.TryParse(e.OldValue.ToString(), out int outValue)) {
            return;
        }

        if(!int.TryParse(e.NewValue.ToString(), out int newValue)) { 
            return;
        }
    }

  

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        "Minimum", typeof(int), typeof(NumberBox), new PropertyMetadata(int.MinValue));

    public int Minimum {
        get { return (int)GetValue(MinimumProperty); }
        set { SetValue(MinimumProperty, value); }
    }

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        "Maximum", typeof(int), typeof(NumberBox), new PropertyMetadata(int.MaxValue));

    public int Maximum {
        get { return (int)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }




    public SolidColorBrush UpArrowColor {
        get { return (SolidColorBrush)GetValue(UpArrowColorProperty); }
        set { SetValue(UpArrowColorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for UpArrowColor.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UpArrowColorProperty =
        DependencyProperty.Register("UpArrowColor", typeof(SolidColorBrush), typeof(NumberBox), new PropertyMetadata(Brushes.LightGray));



    public SolidColorBrush DownArrowColor {
        get { return (SolidColorBrush)GetValue(DownArrowColorProperty); }
        set { SetValue(DownArrowColorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DownArrowColor.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DownArrowColorProperty =
        DependencyProperty.Register("DownArrowColor", typeof(SolidColorBrush), typeof(NumberBox), new PropertyMetadata(Brushes.LightGray));




    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        Button incrementButton = (Button)GetTemplateChild("PART_IncrementButton");
        Button decrementButton = (Button)GetTemplateChild("PART_DecrementButton");
        TextBox textBox = (TextBox)GetTemplateChild("PART_TextBox");

        if (incrementButton is not null) {
            incrementButton.Click += IncrementButton_Click;
        }

        if (decrementButton is not null) {
            decrementButton.Click += DecrementButton_Click;
        }

        if (textBox is not null) {
            textBox.TextChanged += TextBox_TextChanged;
        }
    }

    private void IncrementButton_Click(object sender, RoutedEventArgs e) {
        if (Value < Maximum) {
            Value++;
        }
    }

    private void DecrementButton_Click(object sender, RoutedEventArgs e) {
        if (Value > Minimum) {
            Value--;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is TextBox textBox && int.TryParse(textBox?.Text ?? "0", out int result)) {
            if (result >= Minimum && result <= Maximum) {
                Value = result;
            }
        }
    }
}
