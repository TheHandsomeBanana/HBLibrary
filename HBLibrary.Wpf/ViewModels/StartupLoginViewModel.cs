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
using System.Windows;
using System.Windows.Interop;

namespace HBLibrary.Wpf.ViewModels;

public class StartupLoginViewModel : ViewModelBase {
    private readonly IAccountService accountService;
    private readonly CommonAppSettings appSettings;

    public event Action<bool>? StartupCompleted;

    private ViewModelBase? appLoginContent;
    public ViewModelBase? AppLoginContent {
        get => appLoginContent;
        set {
            appLoginContent = value;
            NotifyPropertyChanged();
        }
    }

    public RelayCommand<Window> CloseWindowCommand { get; set; }
    public RelayCommand LoginToggleCommand { get; set; }
    public RelayCommand RegisterToggleCommand { get; set; }

    public StartupLoginViewModel(IAccountService accountService, CommonAppSettings appSettings) {
        this.accountService = accountService;
        this.appSettings = appSettings;

        CloseWindowCommand = new RelayCommand<Window>(CloseWindow, true);
        LoginToggleCommand = new RelayCommand(LoginToggle, true);
        RegisterToggleCommand = new RelayCommand(RegisterToggle, true);

        LoginToggle(null);
    }

    private void CloseWindow(Window window) {
        StartupCompleted?.Invoke(false);
    }

    private void LoginToggle(object? obj) {
        if (AppLoginContent is RegisterViewModel registerViewModel) {
            registerViewModel.RegistrationTriggered -= RegisterViewModel_RegistrationCompleted;
        }

        LoginViewModel loginViewModel = new LoginViewModel();
        AccountInfo? lastAccount = accountService.GetLastAccount(appSettings.ApplicationName);

        if(lastAccount is not null && lastAccount.AccountType == AccountType.Local) {
            loginViewModel.Username = lastAccount.Username;
        }


        loginViewModel.LoginTriggered += LoginViewModel_LoginCompleted;

        AppLoginContent = loginViewModel;
    }

    private void RegisterToggle(object? obj) {
        if (AppLoginContent is LoginViewModel loginViewModel) {
            loginViewModel.LoginTriggered -= LoginViewModel_LoginCompleted;
        }

        RegisterViewModel registerViewModel = new RegisterViewModel(accountService);
        registerViewModel.RegistrationTriggered += RegisterViewModel_RegistrationCompleted;
        AppLoginContent = registerViewModel;
    }

    private async Task LoginViewModel_LoginCompleted(LoginTriggerData? arg) {
        switch (arg) {
            case LocalLoginTriggerData localLogin:
                await accountService.LoginAsync(new LocalAuthCredentials(localLogin.Username, localLogin.SecurePassword),
                    appSettings.ApplicationName);
                break;
            case MicrosoftLoginTriggerData microsoftLogin:
                Window window = Window.GetWindow(arg.ControlContext);
                IntPtr windowHandle = new WindowInteropHelper(window).Handle;

                MSAuthCredentials mSAuthCredentials = MSAuthCredentials.CreateInteractive([MsalScopes.UserRead], b => {
                    b.WithUseEmbeddedWebView(true);
                    b.WithParentActivityOrWindow(windowHandle);
                });

                await accountService.LoginAsync(mSAuthCredentials, appSettings.ApplicationName);
                break;
        }


        Window parentWindow = Window.GetWindow(arg!.ControlContext);
        parentWindow.Close();    
        StartupCompleted?.Invoke(true);
    }

    private async Task RegisterViewModel_RegistrationCompleted(RegistrationTriggerData? arg) {
        switch (arg) {
            case LocalRegistrationTriggerData localRegistration:
                await accountService.RegisterAsync(new LocalAuthCredentials(localRegistration.Username, localRegistration.SecurePassword),
                    appSettings.ApplicationName);
                break;
            case MicrosoftRegistrationTriggerData microsoftRegistration:
                Window window = Window.GetWindow(arg.ControlContext);
                IntPtr windowHandle = new WindowInteropHelper(window).Handle;

                MSAuthCredentials mSAuthCredentials = MSAuthCredentials.CreateInteractive([MsalScopes.UserRead], b => {
                    b.WithUseEmbeddedWebView(true);
                    b.WithParentActivityOrWindow(windowHandle);
                });

                await accountService.LoginAsync(mSAuthCredentials, appSettings.ApplicationName);
                break;
        }

        Window parentWindow = Window.GetWindow(arg!.ControlContext);
        parentWindow.Visibility = Visibility.Hidden;
        parentWindow.Close();

        StartupCompleted?.Invoke(true);
    }
}
