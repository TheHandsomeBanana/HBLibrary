using HBLibrary.Common;
using HBLibrary.Common.Account;
using HBLibrary.Common.Authentication;
using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Common.DI.Unity;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.ViewModels.Account;
using HBLibrary.Wpf.ViewModels.Login;
using HBLibrary.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace HBLibrary.Wpf.ViewModels;
public class AccountViewModel : ViewModelBase {
    private readonly IAccountService accountService;
    private readonly CommonAppSettings appSettings;
    private readonly Window owner;
    private readonly Action<bool>? userSwitchCallback;
    private readonly Action? preventShutdownCallback;

    public RelayCommand<Window> SwitchUserCommand { get; set; }

    private ViewModelBase? accountDetailViewModel;
    public ViewModelBase? AccountDetailViewModel { 
        get => accountDetailViewModel; 
        set {
            accountDetailViewModel = value;
            NotifyPropertyChanged();
        }
    }

    private string? accountTypeName;
    public string? AccountTypeName {
        get { return accountTypeName; }
        set { 
            accountTypeName = value;
            NotifyPropertyChanged();
        }
    }


    public AccountViewModel(Window owner, IAccountService accountService, CommonAppSettings appSettings, 
        Action<bool>? userSwitchCallback = null, Action? preventShutdownCallback = null) {

        this.accountService = accountService;
        this.appSettings = appSettings;
        this.owner = owner;
        this.userSwitchCallback = userSwitchCallback;
        this.preventShutdownCallback = preventShutdownCallback;

        SwitchUserCommand = new RelayCommand<Window>(SwitchUser, true);

        switch(accountService.Account) {
            case LocalAccount localAccount:
                AccountDetailViewModel = new LocalAccountViewModel(new LocalAccountModel {
                    Username = localAccount.Username,
                });
                AccountTypeName = "Local Account";
                break;
            case MicrosoftAccount microsoftAccount:
                AccountDetailViewModel = new MicrosoftAccountViewModel(new MicrosoftAccountModel {
                    Username = microsoftAccount.Username,
                    DisplayName = microsoftAccount.DisplayName,
                });

                AccountTypeName = "Microsoft Account";
                break;
        }
    }

    private void SwitchUser(Window obj) {
        preventShutdownCallback?.Invoke();

        obj.Close();
        this.owner.Close();

        AccountInfo lastAccount = accountService.Account!.GetAccountInfo();
        StartupLoginViewModel dataContext = new StartupLoginViewModel(accountService, appSettings);

        if (lastAccount.AccountType == AccountType.Local && dataContext.AppLoginContent is LoginViewModel loginViewModel) {
            loginViewModel.Username = lastAccount.Username;
        }

        StartupLoginWindow loginWindow = new StartupLoginWindow();
        loginWindow.DataContext = dataContext;

        dataContext.StartupCompleted += userSwitchCallback;
        loginWindow.Show();
    }
}
