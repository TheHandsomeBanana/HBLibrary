using HBLibrary.Common.Security;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels;
public class LocalLoginViewModel : ViewModelBase<LocalLoginModel> { 
    public event EventHandler<LoginResult>? LoginCompleted;

    public string Username {
        get => Model.Username;
        set {
            Model.Username = value;
            NotifyPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    
    public SecureString SecurePassword {
        get => Model.SecurePassword;
        set {
            Model.SecurePassword = value;
            NotifyPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand<Window> CancelCommand { get; set; }
    public RelayCommand<Window> LoginCommand { get; set; }

    public LocalLoginViewModel() {
        Model = new LocalLoginModel();
        LoginCommand = new RelayCommand<Window>(Login, _ => !string.IsNullOrWhiteSpace(SStringConverter.SecureStringToString(SecurePassword)) && !string.IsNullOrWhiteSpace(Username));
        CancelCommand = new RelayCommand<Window>(Cancel, true);
    }

    private void Cancel(Window obj) {
        obj.Close();
    }

    private void Login(Window obj) {
        LoginResult result = new LoginResult {
            Username = Model.Username,
            SecurePassword = Model.SecurePassword,
        };

        LoginCompleted?.Invoke(this, result);
        obj.Close();
    }
}

public class LoginResult {
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();
}