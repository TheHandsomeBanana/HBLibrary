using HBLibrary.Common;
using HBLibrary.Common.Account;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.ViewModels.Account;
using HBLibrary.Wpf.ViewModels.Login;
using HBLibrary.Wpf.Views;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels;
public class AccountViewModel : ViewModelBase {
    private readonly IAccountService accountService;
    private readonly CommonAppSettings appSettings;
    private readonly Window owner;
    private readonly Action<bool>? onAccountSwitched;
    private readonly Action? onAccountSwitching;

    public RelayCommand<Window> SwitchUserCommand { get; set; }

    private ViewModelBase? accountDetailViewModel = null;
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
        Action<bool>? onAccountSwitched = null, Action? onAccountSwitching = null) {

        this.accountService = accountService;
        this.appSettings = appSettings;
        this.owner = owner;
        this.onAccountSwitched = onAccountSwitched;
        this.onAccountSwitching = onAccountSwitching;

        SwitchUserCommand = new RelayCommand<Window>(SwitchUser, true);

        switch (accountService.Account) {
            case LocalAccount localAccount:
                AccountDetailViewModel = new LocalAccountViewModel(accountService, owner, new LocalAccountModel {
                    Username = localAccount.Username,
                }, appSettings, onAccountSwitched, onAccountSwitching);


                AccountTypeName = "Local Account";
                break;
            case MicrosoftAccount microsoftAccount:
                AccountDetailViewModel = new MicrosoftAccountViewModel(accountService, owner, new MicrosoftAccountModel {
                    Username = microsoftAccount.Username,
                    DisplayName = microsoftAccount.DisplayName,
                }, appSettings, onAccountSwitched, onAccountSwitching);

                AccountTypeName = "Microsoft Account";
                break;
        }
    }

    private void SwitchUser(Window obj) {
        onAccountSwitching?.Invoke();

        obj.Close();
        this.owner.Close();

        StartupLoginViewModel dataContext = new StartupLoginViewModel(accountService, appSettings);

        AccountInfo? lastAccount = accountService.AccountStorage.GetLatestAccount(accountService.Account!.Application);

        if (lastAccount?.AccountType == AccountType.Local && dataContext.AppLoginContent is LoginViewModel loginViewModel) {
            loginViewModel.Username = lastAccount.Username;
        }

        StartupLoginWindow loginWindow = new StartupLoginWindow();
        loginWindow.DataContext = dataContext;

        dataContext.StartupCompleted += onAccountSwitched;
        loginWindow.Show();
    }
}
