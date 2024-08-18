using HBLibrary.Wpf.Models;
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
public class TimeSpanPicker : Control {
    static TimeSpanPicker() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSpanPicker), new FrameworkPropertyMetadata(typeof(TimeSpanPicker)));
    }

    public string Text {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(TimeSpanPicker), new PropertyMetadata(null, OnTextValueChanged));

    private static void OnTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        OnAnyChanged(d, e);
    }


    public TimeSpan ParsedText {
        get { return (TimeSpan)GetValue(ParsedTextProperty); }
        set { SetValue(ParsedTextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ParsedText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ParsedTextProperty =
        DependencyProperty.Register("ParsedText", typeof(TimeSpan), typeof(TimeSpanPicker), new PropertyMetadata(TimeSpan.FromSeconds(0)));

    public TimeUnit TimeUnit {
        get { return (TimeUnit)GetValue(TimeUnitProperty); }
        set { SetValue(TimeUnitProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TimeUnit.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimeUnitProperty =
        DependencyProperty.Register("TimeUnit", typeof(TimeUnit), typeof(TimeSpanPicker), new PropertyMetadata(TimeUnit.Seconds, OnTimeUnitChanged));

    private static void OnTimeUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        OnAnyChanged(d, e);
    }

    private static void OnAnyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = (TimeSpanPicker)d;

        if (int.TryParse(control.Text, out int parsed)) {
            switch (control.TimeUnit) {
                case TimeUnit.Seconds:
                    control.ParsedText = TimeSpan.FromSeconds(parsed);
                    break;

                case TimeUnit.Minutes:
                    control.ParsedText = TimeSpan.FromMinutes(parsed);
                    break;

                case TimeUnit.Hours:
                    control.ParsedText = TimeSpan.FromHours(parsed);
                    break;

                case TimeUnit.Days:
                    control.ParsedText = TimeSpan.FromDays(parsed);
                    break;

                case TimeUnit.Weeks:
                    control.ParsedText = TimeSpan.FromDays(parsed * 7);
                    break;

                case TimeUnit.Years:
                    control.ParsedText = TimeSpan.FromDays(parsed * 365);
                    break;
            }
        }
        else {
            control.ParsedText = TimeSpan.FromSeconds(0); // Default value or handle the error as needed
        }
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        TextBox? textBox = GetTemplateChild("PART_TextBox") as TextBox;

        if (textBox != null) {
            textBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            textBox.PreviewKeyDown += NumericTextBox_PreviewKeyDown;
        }
    }

    private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
        e.Handled = !IsTextNumeric(e.Text);
    }

    private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
        if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Escape || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Down || e.Key == Key.Up) {
            return;
        }

        if (!IsKeyNumeric(e.Key)) {
            e.Handled = true;
        }
    }

    private bool IsTextNumeric(string input) {
        return int.TryParse(input, out _);
    }

    private bool IsKeyNumeric(Key key) {
        return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9);
    }

}
