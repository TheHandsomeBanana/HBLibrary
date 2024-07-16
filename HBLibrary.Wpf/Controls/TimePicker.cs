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
public class TimePicker : Control {
    static TimePicker() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
    }

    public TimeOnly? SelectedTime {
        get { return (TimeOnly?)GetValue(SelectedTimeProperty); }
        set { SetValue(SelectedTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedTimeProperty =
        DependencyProperty.Register("SelectedTime", typeof(TimeOnly?), typeof(TimePicker), new PropertyMetadata(null, OnSelectedTimeChanged));


    public Style HourStyle {
        get { return (Style)GetValue(HourStyleProperty); }
        set { SetValue(HourStyleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for HourStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HourStyleProperty =
        DependencyProperty.Register("HourStyle", typeof(Style), typeof(TimePicker), new PropertyMetadata(null));




    public Style SeperatorStyle {
        get { return (Style)GetValue(SeperatorStyleProperty); }
        set { SetValue(SeperatorStyleProperty, value); }
    }
    

    // Using a DependencyProperty as the backing store for SeperatorStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SeperatorStyleProperty =
        DependencyProperty.Register("SeperatorStyle", typeof(Style), typeof(TimePicker), new PropertyMetadata(null));




    public Style MinuteStyle {
        get { return (Style)GetValue(MinuteStyleProperty); }
        set { SetValue(MinuteStyleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MinuteStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MinuteStyleProperty =
        DependencyProperty.Register("MinuteStyle", typeof(Style), typeof(TimePicker), new PropertyMetadata(null));


    public DataTemplate HourItemTemplate {
        get { return (DataTemplate)GetValue(HourItemTemplateProperty); }
        set { SetValue(HourItemTemplateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for HourItemTemplate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HourItemTemplateProperty =
        DependencyProperty.Register("HourItemTemplate", typeof(DataTemplate), typeof(TimePicker), new PropertyMetadata(null));


    public DataTemplate MinuteItemTemplate {
        get { return (DataTemplate)GetValue(MinuteItemTemplateProperty); }
        set { SetValue(MinuteItemTemplateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MinuteItemTemplate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MinuteItemTemplateProperty =
        DependencyProperty.Register("MinuteItemTemplate", typeof(DataTemplate), typeof(TimePicker), new PropertyMetadata(null));

    public Path Icon {
        get { return (Path)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Path), typeof(TimePicker), new PropertyMetadata(null));



    private bool isUpdating;
    private ComboBox? hourComboBox;
    private ComboBox? minuteComboBox;
    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        hourComboBox = GetTemplateChild("PART_HourComboBox") as ComboBox;
        minuteComboBox = GetTemplateChild("PART_MinuteComboBox") as ComboBox;

        if (hourComboBox != null) {
            hourComboBox.SelectionChanged += (s, e) => UpdateSelectedTime();
            PopulateComboBox(hourComboBox, 24);
            
        }

        if (minuteComboBox != null) {
            minuteComboBox.SelectionChanged += (s, e) => UpdateSelectedTime();
            PopulateComboBox(minuteComboBox, 60);
        }
    }
    private void UpdateSelectedTime() {
        if (isUpdating) {
            return;
        }

        if (hourComboBox != null && minuteComboBox != null && hourComboBox.SelectedItem != null && minuteComboBox.SelectedItem != null) {
            int hour = hourComboBox.SelectedIndex;
            int minute = minuteComboBox.SelectedIndex;
            isUpdating = true;
            SelectedTime = new TimeOnly(hour, minute);
            isUpdating = false;
        }
    }

    private void PopulateComboBox(ComboBox comboBox, int maxItems) {
        for (int i = 0; i < maxItems; i++) {
            comboBox.Items.Add(new ComboBoxItem { Content = i.ToString("D2") });
        }
    }

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is TimePicker timePicker) {
            timePicker.UpdateComboBoxes();
        }
    }

    private void UpdateComboBoxes() {
        if(isUpdating) {
            return;
        }

        if (hourComboBox != null && minuteComboBox != null) {
            isUpdating = true;
            hourComboBox.SelectedIndex = SelectedTime.HasValue 
                ? SelectedTime.Value.Hour
                : -1;

            minuteComboBox.SelectedIndex = SelectedTime.HasValue
                ? SelectedTime.Value.Minute
                : -1;

            isUpdating = false;
        }
    }
}
