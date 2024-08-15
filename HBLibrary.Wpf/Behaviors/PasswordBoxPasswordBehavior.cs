using Microsoft.Xaml.Behaviors;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Behaviors;

public class PasswordBoxPasswordBehavior : Behavior<PasswordBox> {
    public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordBoxPasswordBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

    public SecureString Password {
        get { return (SecureString)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is PasswordBoxPasswordBehavior behavior && behavior.AssociatedObject != null) {
            var passwordBox = behavior.AssociatedObject;
            if (e.NewValue is SecureString newPassword) {
                passwordBox.PasswordChanged -= behavior.PasswordBox_PasswordChanged;

                if (newPassword != null) {
                    string? newPasswordString = ConvertToUnsecureString(newPassword);
                    if (passwordBox.Password != newPasswordString) {
                        passwordBox.Password = newPasswordString;
                    }
                }

                passwordBox.PasswordChanged += behavior.PasswordBox_PasswordChanged;
            }
        }
    }

    protected override void OnAttached() {
        base.OnAttached();
        AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
    }

    protected override void OnDetaching() {
        base.OnDetaching();
        AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
        PasswordBox passwordBox = (PasswordBox)sender;
        Password = ConvertToSecureString(passwordBox.Password);
    }

    private static SecureString ConvertToSecureString(string password) {
        var secureString = new SecureString();
        foreach (char c in password) {
            secureString.AppendChar(c);
        }
        secureString.MakeReadOnly();
        return secureString;
    }

    private static string? ConvertToUnsecureString(SecureString secureString) {
        if (secureString == null)
            return string.Empty;

        var unmanagedString = IntPtr.Zero;
        try {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally {
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }
}
