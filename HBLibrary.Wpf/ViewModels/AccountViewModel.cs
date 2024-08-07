using HBLibrary.Common;
using HBLibrary.Common.Account;
using HBLibrary.Common.Authentication;
using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Common.DI.Unity;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
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
    private readonly CommonAppSettings commonAppSettings;

    public RelayCommand<Window> SwitchUserCommand { get; set; }

    private ViewModelBase accountDetailViewModel;
    public ViewModelBase AccountDetailViewModel { 
        get => accountDetailViewModel; 
        set {
            accountDetailViewModel = value;
            NotifyPropertyChanged();
        }
    }

    public AccountViewModel(IAccountService accountService, CommonAppSettings commonAppSettings) {
        this.accountService = accountService;
        this.commonAppSettings = commonAppSettings;

        SwitchUserCommand = new RelayCommand<Window>(SwitchUser, true);
        accountDetailViewModel = new LocalLoginViewModel();
    }

    private void SwitchUser(Window obj) {
        HBDarkLoginWindow loginWindow = new HBDarkLoginWindow(obj);
        LoginViewModel dataContext = (LoginViewModel)loginWindow.DataContext;
        dataContext.LoginCompleted += LoginCompleted;
        loginWindow.ShowDialog();
    }

    private async Task LoginCompleted(LoginResult? e) {
        switch (e) {
            case LocalLoginResult localLogin:
                await accountService.LoginAsync(new LocalAuthCredentials(localLogin.Username, localLogin.SecurePassword),
                    commonAppSettings.ApplicationName);

                AccountDetailViewModel = new LocalLoginViewModel(new LocalLoginModel {
                    Username = localLogin.Username
                });
                break;
            case MicrosoftLoginResult microsoftLogin:


                AccountDetailViewModel = new MicrosoftLoginViewModel();
                break;
        }
    }
}
