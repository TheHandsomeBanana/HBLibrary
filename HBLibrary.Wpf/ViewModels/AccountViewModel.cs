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

namespace HBLibrary.Wpf.ViewModels;
public class AccountViewModel : ViewModelBase {
    public RelayCommand<Window> SwitchUserCommand { get; set; }

    private ViewModelBase accountDetailViewModel;
    public ViewModelBase AccountDetailViewModel { 
        get => accountDetailViewModel; 
        set {
            accountDetailViewModel = value;
            NotifyPropertyChanged();
        }
    }

    public AccountViewModel() {
        SwitchUserCommand = new RelayCommand<Window>(SwitchUser, true);

        AccountDetailViewModel = new LocalLoginViewModel();
    }

    private void SwitchUser(Window obj) {
        HBDarkLoginWindow loginWindow = new HBDarkLoginWindow(obj);
        LoginViewModel dataContext = (LoginViewModel)loginWindow.DataContext;
        dataContext.LoginCompleted += LoginCompleted;
        loginWindow.ShowDialog();

    }

    private void LoginCompleted(object? sender, LoginResult? e) {
        switch (e) {
            case LocalLoginResult localLogin:
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
