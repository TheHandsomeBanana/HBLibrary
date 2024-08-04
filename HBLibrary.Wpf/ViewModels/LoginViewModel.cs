using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels;
public class LoginViewModel : ViewModelBase {
    public event EventHandler<LoginResult?>? LoginCompleted;

    public bool CanExecuteLogin { get; set; } = false;

    public ViewModelBase currentLoginViewModel;
    public ViewModelBase CurrentLoginViewModel {
        get => currentLoginViewModel;
        set {
            currentLoginViewModel = value;
            NotifyPropertyChanged();
        }
    }

    private AccountType selectedAccountType;
    public AccountType SelectedAccountType {
        get => selectedAccountType;
        set {
            selectedAccountType = value;
            NotifyPropertyChanged();

            if(CurrentLoginViewModel is LocalLoginViewModel localLoginViewModel) {
                localLoginViewModel.ValidationPropertyChanged -= LocalLoginViewModel_ValidationPropertyChanged;
            }

            switch (selectedAccountType) {
                case AccountType.Local:
                    localLoginViewModel = new LocalLoginViewModel();
                    localLoginViewModel.ValidationPropertyChanged += LocalLoginViewModel_ValidationPropertyChanged;
                    localLoginViewModel.NotifyValidationPropertyChanged();
                    CurrentLoginViewModel = localLoginViewModel;

                    break;
                case AccountType.Microsoft:
                    CurrentLoginViewModel = new MicrosoftLoginViewModel();
                    break;
            }

        }
    }

    private void LocalLoginViewModel_ValidationPropertyChanged(object? sender, bool e) {
        CanExecuteLogin = e;
        LoginCommand.NotifyCanExecuteChanged();
    }

    public RelayCommand<Window> CancelCommand { get; set; }
    public RelayCommand<Window> LoginCommand { get; set; }

    public LoginViewModel() {
        LocalLoginViewModel localLoginViewModel = new LocalLoginViewModel();
        localLoginViewModel.ValidationPropertyChanged += LocalLoginViewModel_ValidationPropertyChanged;
        currentLoginViewModel = localLoginViewModel;
        
        

        LoginCommand = new RelayCommand<Window>(Login, w => CanExecuteLogin);
        CancelCommand = new RelayCommand<Window>(Cancel, true);
    }

    private void Cancel(Window obj) {
        obj.Close();
    }

    private void Login(Window obj) {
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

        LoginCompleted?.Invoke(this, result);
        obj.Close();
    }
}