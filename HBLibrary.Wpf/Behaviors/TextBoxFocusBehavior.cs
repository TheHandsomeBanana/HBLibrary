using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HBLibrary.Wpf.Behaviors;
public class TextBoxFocusBehavior : Behavior<TextBox> {
    protected override void OnAttached() {
        base.OnAttached();
        AssociatedObject.Loaded += AssociatedObject_Loaded;
    }

    private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e) {
        AssociatedObject.Focus();
        Keyboard.Focus(AssociatedObject);
    }

    protected override void OnDetaching() {
        base.OnDetaching();
        AssociatedObject.Loaded -= AssociatedObject_Loaded;
    }
}
