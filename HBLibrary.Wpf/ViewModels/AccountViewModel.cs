using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels;
public class AccountViewModel : ViewModelBase<AccountModel> {
    public RelayCommand<Window> SwitchUserCommand { get; set; }


    public AccountViewModel() {
        SwitchUserCommand = new RelayCommand<Window>(SwitchUser, true);
    }

    private void SwitchUser(Window obj) {
        HBDarkLoginWindow loginWindow = new HBDarkLoginWindow(obj);
        LocalLoginViewModel dataContext = (LocalLoginViewModel)loginWindow.DataContext;
        dataContext.LoginCompleted += LocalLoginCompleted;
        loginWindow.ShowDialog();
    }

    private void LocalLoginCompleted(object? sender, LoginResult e) {
        throw new NotImplementedException();
    }
}
