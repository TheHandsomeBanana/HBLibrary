using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HBLibrary.Wpf.Controls;
public class LogListBoxItem : ListBoxItem {
    public static readonly DependencyProperty LogLevelProperty =
       DependencyProperty.Register("LogLevel", typeof(string), typeof(LogListBoxItem), new PropertyMetadata(string.Empty));

    public string LogLevel {
        get { return (string)GetValue(LogLevelProperty); }
        set { SetValue(LogLevelProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register("Message", typeof(string), typeof(LogListBoxItem), new PropertyMetadata(string.Empty));

    public string Message {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly DependencyProperty TimestampProperty =
        DependencyProperty.Register("Timestamp", typeof(DateTime), typeof(LogListBoxItem), new PropertyMetadata(DateTime.Now));

    public DateTime Timestamp {
        get { return (DateTime)GetValue(TimestampProperty); }
        set { SetValue(TimestampProperty, value); }
    }

    public static readonly DependencyProperty ForegroundColorProperty =
        DependencyProperty.Register("ForegroundColor", typeof(Brush), typeof(LogListBoxItem), new PropertyMetadata(Brushes.Black));

    public Brush ForegroundColor {
        get { return (Brush)GetValue(ForegroundColorProperty); }
        set { SetValue(ForegroundColorProperty, value); }
    }

    public static readonly DependencyProperty LineNumberProperty =
       DependencyProperty.Register("LineNumber", typeof(int), typeof(LogListBoxItem), new PropertyMetadata(0));

    public int LineNumber {
        get { return (int)GetValue(LineNumberProperty); }
        set { SetValue(LineNumberProperty, value); }
    }
}
