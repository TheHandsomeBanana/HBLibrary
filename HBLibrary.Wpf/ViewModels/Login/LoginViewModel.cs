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
    public event Func<LoginTriggerData?, Task>? LoginTriggered;

    public string Username {
        get => Model.Username;
        set {
            Model.Username = value;
            NotifyPropertyChanged();

            LoginCommand.NotifyCanExecuteChanged();
            if (ErrorMessage is not null) {
                ErrorMessage = null;
            }
        }
    }

    public SecureString SecurePassword {
        get => Model.SecurePassword;
        set {
            Model.SecurePassword = value;
            NotifyPropertyChanged();

            LoginCommand.NotifyCanExecuteChanged();
            if (ErrorMessage is not null) {
                ErrorMessage = null;
            }
        }
    }

    private string? errorMessage;

    public string? ErrorMessage {
        get { return errorMessage; }
        set {
            errorMessage = value;
            NotifyPropertyChanged();
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
        ErrorMessage = null;

        MicrosoftLoginTriggerData? result = new MicrosoftLoginTriggerData {
            ControlContext = obj,
        };

        if (LoginTriggered is not null) {
            await LoginTriggered.Invoke(result);
        }
    }

    private async Task LoginAsync(UserControl obj) {
        ErrorMessage = null;

        LocalLoginTriggerData? result = new LocalLoginTriggerData {
            ControlContext = obj,
            Username = Model.Username,
            SecurePassword = Model.SecurePassword
        };

        if (LoginTriggered is not null) {
            await LoginTriggered.Invoke(result);
        }
    }

    private void OnLoginException(Exception exception) {
        ErrorMessage = exception.Message;
    }

    private bool IsLoginInputValid() {
        return !string.IsNullOrEmpty(Model.Username)
            && !string.IsNullOrEmpty(SStringConverter.SecureStringToString(Model.SecurePassword));
    }
}