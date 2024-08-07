using HBLibrary.Common;
using HBLibrary.Common.Account;
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
    private readonly CommonAppSettings commonAppSettings;

    private ViewModelBase appLoginContent;
    public ViewModelBase AppLoginContent {
        get => appLoginContent;
        set {
            appLoginContent = value;
            NotifyPropertyChanged();
        }
    }

    public AsyncRelayCommand LoginToggleCommand { get; set; }
    public RelayCommand RegisterToggleCommand { get; set; }

    public StartupLoginViewModel(IAccountService accountService, CommonAppSettings commonAppSettings) {
        this.accountService = accountService;
        this.commonAppSettings = commonAppSettings;

        LoginToggleCommand = new AsyncRelayCommand(LoginToggle, o => true, OnLoginToggleException);
        RegisterToggleCommand = new RelayCommand(RegisterToggle, true);
    }

    private void OnLoginToggleException(Exception exception) {
        // Don't spit, swallow
    }

    private async Task LoginToggle(object? obj) {
        AccountInfo? accountInfo = await accountService.GetLastAccountAsync(commonAppSettings.ApplicationName);
        AccountType accountType = accountInfo?.AccountType ?? AccountType.Local;
        
        switch(accountType) {
            case AccountType.Local:
                AppLoginContent = new LocalLoginViewModel();
                break;
            case AccountType.Microsoft:
                AppLoginContent = new MicrosoftLoginViewModel();
                break;
        }
    }

    private void RegisterToggle(object? obj) {
        AppLoginContent = new RegisterViewModel();
    }
}
