using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels.Login;
public class LoginViewModel : ViewModelBase {
    public event Func<LoginResult?, Task>? LoginCompleted;

    public bool CanExecuteLogin { get; set; } = false;

    public ViewModelBase currentLoginViewModel;
    public ViewModelBase CurrentLoginViewModel {
        get => currentLoginViewModel;
        set {
            currentLoginViewModel = value;
            NotifyPropertyChanged();
        }
    }

    private void LocalLoginViewModel_ValidationPropertyChanged(object? sender, bool e) {
        CanExecuteLogin = e;
        LoginCommand.NotifyCanExecuteChanged();
    }

    public RelayCommand<Window> CancelCommand { get; set; }
    public AsyncRelayCommand<Window> LoginCommand { get; set; }

    public LoginViewModel(IAccountService accountService) {
        LocalLoginViewModel localLoginViewModel = new LocalLoginViewModel();
        localLoginViewModel.ValidationPropertyChanged += LocalLoginViewModel_ValidationPropertyChanged;
        currentLoginViewModel = localLoginViewModel;

        LoginCommand = new AsyncRelayCommand<Window>(LoginAsync, w => CanExecuteLogin, OnLoginException);
        CancelCommand = new RelayCommand<Window>(Cancel, true);
    }

    private void Cancel(Window obj) {
        obj.Close();
    }

    private async Task LoginAsync(Window obj) {
        LoginResult? result = null;

        switch (currentLoginViewModel) {
            case LocalLoginViewModel localLogin:
                result = new LocalLoginResult {
                    Username = localLogin.Username,
                    SecurePassword = localLogin.SecurePassword
                };
                break;
            case MicrosoftLoginViewModel microsoftLogin:
                result = new MicrosoftLoginResult {
                    
                };
                break;
        }

        if (LoginCompleted is not null) {
            await LoginCompleted.Invoke(result);
        }

        obj.Close();
    }

    private void OnLoginException(Exception exception) {
        // There are no exceptions
    }
}