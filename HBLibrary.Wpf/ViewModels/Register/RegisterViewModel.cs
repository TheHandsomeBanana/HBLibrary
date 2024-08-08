using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.ViewModels.Register;

public class RegisterViewModel : ViewModelBase<RegistrationModel> {
    public event Func<RegistrationResult?, Task>? RegistrationCompleted;

    public string Username {
        get => Model.Username;
        set {
            Model.Username = value;
            NotifyPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }

    public SecureString SecurePassword {
        get => Model.SecurePassword;
        set {
            Model.SecurePassword = value;
            NotifyPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }
    
    public SecureString ConfirmSecurePassword {
        get => Model.ConfirmSecurePassword;
        set {
            Model.ConfirmSecurePassword = value;
            NotifyPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }

    public AsyncRelayCommand<UserControl> RegisterCommand { get; set; }
    public AsyncRelayCommand<UserControl> RegisterWithMicrosoftCommand { get; set; }

    public RegisterViewModel(IAccountService accountService) {
        Model = new RegistrationModel();
        RegisterCommand = new AsyncRelayCommand<UserControl>(RegisterAsync, w => IsRegisterInputValid(), OnLoginException);
        RegisterWithMicrosoftCommand = new AsyncRelayCommand<UserControl>(RegisterWithMicrosoftAsync, w => true, OnLoginException);
    }

    private async Task RegisterWithMicrosoftAsync(UserControl obj) {
        MicrosoftRegistrationResult result = new MicrosoftRegistrationResult {
            Username = Model.Username,
        };

        Window parentWindow = Window.GetWindow(obj);
        parentWindow.Visibility = Visibility.Hidden;

        if (RegistrationCompleted is not null) {
            await RegistrationCompleted.Invoke(result);
        }

        parentWindow.Close();
    }

    private async Task RegisterAsync(UserControl obj) {
        LocalRegistrationResult result = new LocalRegistrationResult {
            Username = Model.Username,
            SecurePassword = Model.SecurePassword
        };

        Window parentWindow = Window.GetWindow(obj);
        parentWindow.Visibility = Visibility.Hidden;


        if (RegistrationCompleted is not null) {
            await RegistrationCompleted.Invoke(result);
        }

        parentWindow.Close();
    }

    private void OnLoginException(Exception exception) {
        // There are no exceptions!
    }

    private bool IsRegisterInputValid() {
        string securePassword = SStringConverter.SecureStringToString(Model.SecurePassword)!;
        string confirmSecurePassword = SStringConverter.SecureStringToString(Model.ConfirmSecurePassword)!;

        return !string.IsNullOrEmpty(Model.Username)
            && !string.IsNullOrEmpty(securePassword)
            && !string.IsNullOrEmpty(confirmSecurePassword)
            && securePassword == confirmSecurePassword;
    }
}
