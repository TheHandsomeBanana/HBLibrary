using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using System.Security;
using System.Windows.Controls;

namespace HBLibrary.Wpf.ViewModels.Register;

public class RegisterViewModel : ViewModelBase<RegistrationModel> {
    public event Func<RegistrationTriggerData?, Task>? RegistrationTriggered;

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

            if (ErrorMessage is not null) {
                ErrorMessage = null;
            }
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

    private string? errorMessage;
    public string? ErrorMessage {
        get { return errorMessage; }
        set {
            errorMessage = value;
            NotifyPropertyChanged();
        }
    }


    public AsyncRelayCommand<UserControl> RegisterCommand { get; set; }
    public AsyncRelayCommand<UserControl> RegisterWithMicrosoftCommand { get; set; }

    public RegisterViewModel(IAccountService accountService) {
        Model = new RegistrationModel();
        RegisterCommand = new AsyncRelayCommand<UserControl>(RegisterAsync, w => IsRegisterInputValid(), OnRegisterException);
        RegisterWithMicrosoftCommand = new AsyncRelayCommand<UserControl>(RegisterWithMicrosoftAsync, w => true, OnRegisterException);
    }

    private async Task RegisterWithMicrosoftAsync(UserControl obj) {
        MicrosoftRegistrationTriggerData result = new MicrosoftRegistrationTriggerData {
            ControlContext = obj,
            Username = Model.Username,
        };

        if (RegistrationTriggered is not null) {
            await RegistrationTriggered.Invoke(result);
        }
    }

    private async Task RegisterAsync(UserControl obj) {
        LocalRegistrationTriggerData result = new LocalRegistrationTriggerData {
            ControlContext = obj,
            Username = Model.Username,
            SecurePassword = Model.SecurePassword
        };

        if (RegistrationTriggered is not null) {
            await RegistrationTriggered.Invoke(result);
        }
    }

    private void OnRegisterException(Exception exception) {
        ErrorMessage = exception.Message;
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
