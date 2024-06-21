using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HBLibrary.Wpf.Navigation;
public class NavButton : ListBoxItem {
    static NavButton() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));
    }

    public Uri NavLink {
        get { return (Uri)GetValue(NavLinkProperty); }
        set { SetValue(NavLinkProperty, value); }
    }

    // Using a DependencyProperty as the backing store for NavLink.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NavLinkProperty =
        DependencyProperty.Register("NavLink", typeof(Uri), typeof(NavButton), new PropertyMetadata(null));



    public double IconHeight {
        get { return (double)GetValue(IconHeightProperty); }
        set { SetValue(IconHeightProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconHeightProperty =
        DependencyProperty.Register("IconHeight", typeof(double), typeof(NavButton), new PropertyMetadata(0.0));


    public double IconWidth {
        get { return (double)GetValue(IconWidthProperty); }
        set { SetValue(IconWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconWidthProperty =
        DependencyProperty.Register("IconWidth", typeof(double), typeof(NavButton), new PropertyMetadata(0.0));



    public SolidColorBrush IconFill {
        get { return (SolidColorBrush)GetValue(IconFillProperty); }
        set { SetValue(IconFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconFillProperty =
        DependencyProperty.Register("IconFill", typeof(SolidColorBrush), typeof(NavButton), new PropertyMetadata(Brushes.White));



    public Geometry Icon {
        get { return (Geometry)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavButton), new PropertyMetadata(null));



    public string Text {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(NavButton), new PropertyMetadata(null));




    public SolidColorBrush SelectedBackground {
        get { return (SolidColorBrush)GetValue(SelectedBackgroundProperty); }
        set { SetValue(SelectedBackgroundProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedBackground.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedBackgroundProperty =
        DependencyProperty.Register("SelectedBackground", typeof(SolidColorBrush), typeof(NavButton), new PropertyMetadata(null));


}
