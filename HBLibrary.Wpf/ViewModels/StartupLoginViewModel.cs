using HBLibrary.Common;
using HBLibrary.Common.Account;
using HBLibrary.Common.Authentication;
using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels.Login;
using HBLibrary.Wpf.ViewModels.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;

public class StartupLoginViewModel : ViewModelBase {
    private readonly IAccountService accountService;
    private readonly CommonAppSettings appSettings;

    public event Func<LoginResult?, Task>? LoginCompleted;
    public event Func<RegistrationResult?, Task>? RegistrationCompleted;

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

    public StartupLoginViewModel(IAccountService accountService, CommonAppSettings commonAppSettings) {
        this.accountService = accountService;
        this.appSettings = commonAppSettings;

        LoginToggleCommand = new RelayCommand(LoginToggle, true);
        RegisterToggleCommand = new RelayCommand(RegisterToggle, true);

        LoginToggle(null);
    }

    private void LoginToggle(object? obj) {
        LoginViewModel loginViewModel = new LoginViewModel(accountService);
        AppLoginContent = loginViewModel;
    }

    private void RegisterToggle(object? obj) {
        AppLoginContent = new RegisterViewModel();
    }
}
