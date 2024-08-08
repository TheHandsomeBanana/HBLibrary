using HBLibrary.Common;
using HBLibrary.Common.Account;
using HBLibrary.Common.Authentication;
using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels.Login;
using HBLibrary.Wpf.ViewModels.Register;
using Microsoft.Graph.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;

public class StartupLoginViewModel : ViewModelBase {
    private readonly IAccountService accountService;
    private readonly CommonAppSettings appSettings;

    public event Action? StartupCompleted;

    private ViewModelBase? appLoginContent;
    public ViewModelBase? AppLoginContent {
        get => appLoginContent;
        set {
            appLoginContent = value;
            NotifyPropertyChanged();
        }
    }

    public RelayCommand LoginToggleCommand { get; set; }
    public RelayCommand RegisterToggleCommand { get; set; }

    public StartupLoginViewModel(IAccountService accountService, CommonAppSettings appSettings) {
        this.accountService = accountService;
        this.appSettings = appSettings;

        LoginToggleCommand = new RelayCommand(LoginToggle, true);
        RegisterToggleCommand = new RelayCommand(RegisterToggle, true);

        LoginToggle(null);
    }

    private void LoginToggle(object? obj) {
        if (AppLoginContent is RegisterViewModel registerViewModel) {
            registerViewModel.RegistrationCompleted -= RegisterViewModel_RegistrationCompleted;
        }

        LoginViewModel loginViewModel = new LoginViewModel(accountService);
        loginViewModel.LoginCompleted += LoginViewModel_LoginCompleted;

        AppLoginContent = loginViewModel;
    }

    private void RegisterToggle(object? obj) {
        if (AppLoginContent is LoginViewModel loginViewModel) {
            loginViewModel.LoginCompleted -= LoginViewModel_LoginCompleted;
        }

        RegisterViewModel registerViewModel = new RegisterViewModel(accountService);
        registerViewModel.RegistrationCompleted += RegisterViewModel_RegistrationCompleted;
        AppLoginContent = registerViewModel;
    }

    private async Task LoginViewModel_LoginCompleted(LoginResult? arg) {
        switch (arg) {
            case LocalLoginResult localLogin:
                await accountService.LoginAsync(new LocalAuthCredentials(localLogin.Username, localLogin.SecurePassword),
                    appSettings.ApplicationName);
                break;
            case MicrosoftLoginResult microsoftLogin:
                break;
        }

        StartupCompleted?.Invoke();
    }

    private async Task RegisterViewModel_RegistrationCompleted(RegistrationResult? arg) {
        switch (arg) {
            case LocalRegistrationResult localRegistration:
                await accountService.LoginAsync(new LocalAuthCredentials(localRegistration.Username, localRegistration.SecurePassword),
                    appSettings.ApplicationName);
                break;
            case MicrosoftRegistrationResult microsoftRegistration:
                break;
        }

        StartupCompleted?.Invoke();
    }
}
