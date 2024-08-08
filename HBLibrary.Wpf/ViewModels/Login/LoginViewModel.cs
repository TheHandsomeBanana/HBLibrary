using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.ViewModels.Login;
public class LoginViewModel : ViewModelBase<LoginModel> {
    public event Func<LoginResult?, Task>? LoginCompleted;

    public string Username {
        get => Model.Username;
        set {
            Model.Username = value;
            NotifyPropertyChanged();

            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    public SecureString SecurePassword {
        get => Model.SecurePassword;
        set {
            Model.SecurePassword = value;
            NotifyPropertyChanged();

            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    public AsyncRelayCommand<UserControl> LoginCommand { get; set; }
    public AsyncRelayCommand<UserControl> LoginWithMicrosoftCommand { get; set; }

    public LoginViewModel() {
        Model = new LoginModel();
        LoginCommand = new AsyncRelayCommand<UserControl>(LoginAsync, w => IsLoginInputValid(), OnLoginException);
        LoginWithMicrosoftCommand = new AsyncRelayCommand<UserControl>(LoginWithMicrosoftAsync, w => true, OnLoginException);
    }

    private async Task LoginWithMicrosoftAsync(UserControl obj) {
        MicrosoftLoginResult? result = new MicrosoftLoginResult {
            Username = Model.Username,
        };

        if (LoginCompleted is not null) {
            await LoginCompleted.Invoke(result);
        }

        (obj.Parent as Window)?.Close();
    }

    private async Task LoginAsync(UserControl obj) {
        LocalLoginResult? result = new LocalLoginResult {
            Username = Model.Username,
            SecurePassword = Model.SecurePassword
        };

        Window parentWindow = Window.GetWindow(obj);
        parentWindow.Visibility = Visibility.Hidden;

        if (LoginCompleted is not null) {
            await LoginCompleted.Invoke(result);
        }




        parentWindow.Close();
    }

    private void OnLoginException(Exception exception) {
        // There are no exceptions!
    }

    private bool IsLoginInputValid() {
        return !string.IsNullOrEmpty(Model.Username)
            && !string.IsNullOrEmpty(SStringConverter.SecureStringToString(Model.SecurePassword));
    }
}