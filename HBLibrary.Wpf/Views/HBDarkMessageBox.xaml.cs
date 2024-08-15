using System.Media;
using System.Windows;
using System.Windows.Input;

namespace HBLibrary.Wpf.Views;
/// <summary>
/// Interaction logic for HBDarkMessageBox.xaml
/// </summary>
public partial class HBDarkMessageBox : Window {
    private MessageBoxResult result = MessageBoxResult.None;

    public HBDarkMessageBox(string title, string message) {
        InitializeComponent();
        txbTitle.Text = title;
        txbMessage.Text = message;
        SystemSounds.Exclamation.Play();
    }

    public HBDarkMessageBox(string title, string message, MessageBoxButton messageBoxButton) : this(title, message) {
        ConfigureButtons(messageBoxButton);
    }

    public HBDarkMessageBox(string title, string message, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage) : this(title, message, messageBoxButton) {
        ConfigureIcon(messageBoxImage);
    }

    public static bool Show(string title, string message) {
        HBDarkMessageBox messageBox = new HBDarkMessageBox(title, message);
        messageBox.btnOk.Visibility = Visibility.Visible;
        messageBox.infoIcon.Visibility = Visibility.Visible;

        return messageBox.ShowDialog() ?? false;
    }


    public static MessageBoxResult Show(string title, string message, MessageBoxButton messageBoxButton) {
        HBDarkMessageBox messageBox = new HBDarkMessageBox(title, message, messageBoxButton);
        messageBox.ShowDialog();
        return messageBox.result;
    }

    public static MessageBoxResult Show(string title, string message, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage) {
        HBDarkMessageBox messageBox = new HBDarkMessageBox(title, message, messageBoxButton, messageBoxImage);
        messageBox.ShowDialog();
        return messageBox.result;
    }

    private void ConfigureButtons(MessageBoxButton buttons) {
        switch (buttons) {
            case MessageBoxButton.OK:
                btnOk.Visibility = Visibility.Visible;
                break;
            case MessageBoxButton.OKCancel:
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                break;
            case MessageBoxButton.YesNo:
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
                break;
            case MessageBoxButton.YesNoCancel:
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                break;
            default:
                btnOk.Visibility = Visibility.Visible;
                break;
        }
    }

    private void ConfigureIcon(MessageBoxImage icon) {
        switch (icon) {
            case MessageBoxImage.Information:
                infoIcon.Visibility = Visibility.Visible;
                break;
            case MessageBoxImage.Question:
                questionIcon.Visibility = Visibility.Visible;
                break;
            case MessageBoxImage.Warning:
                warningIcon.Visibility = Visibility.Visible;
                break;
            case MessageBoxImage.Error:
                errorIcon.Visibility = Visibility.Visible;
                break;
        }
    }

    // Can execute
    private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
        e.CanExecute = true;
    }


    // Close
    private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e) {
        SystemCommands.CloseWindow(this);
    }

    protected override void OnContentRendered(EventArgs e) {
        base.OnContentRendered(e);

        // Content of window may be black in case of SizeToContent is set. 
        // This eliminates the problem. 
        // Do not use InvalidateVisual because it may implicitly break your markup.
        InvalidateMeasure();
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e) {
        result = MessageBoxResult.OK;
        SystemCommands.CloseWindow(this);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
        result = MessageBoxResult.Cancel;
        SystemCommands.CloseWindow(this);
    }

    private void btnNo_Click(object sender, RoutedEventArgs e) {
        result = MessageBoxResult.No;
        SystemCommands.CloseWindow(this);
    }

    private void btnYes_Click(object sender, RoutedEventArgs e) {
        result = MessageBoxResult.Yes;
        SystemCommands.CloseWindow(this);
    }
}

