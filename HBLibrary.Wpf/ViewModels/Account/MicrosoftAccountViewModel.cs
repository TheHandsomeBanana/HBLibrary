using HBLibrary.Core;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.Views;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels.Account;

public class MicrosoftAccountViewModel : ViewModelBase<MicrosoftAccountModel> {
    private readonly IAccountService accountService;
    private readonly Window parentOwner;
    private readonly CommonAppSettings appSettings;
    private readonly Action<bool>? userSwitchCallback;
    private readonly Action? preventShutdownCallback;

    public string Username {
        get { return Model.Username; }
        set {
            Model.Username = value;
            NotifyPropertyChanged();
        }
    }

    public string DisplayName {
        get { return Model.DisplayName; }
        set {
            Model.DisplayName = value;
            NotifyPropertyChanged();
        }
    }

    public AsyncRelayCommand SignOutCommand { get; set; }

    public MicrosoftAccountViewModel(IAccountService accountService, Window parentOwner, MicrosoftAccountModel microsoftAccount, CommonAppSettings appSettings,
        Action<bool>? userSwitchCallback = null, Action? preventShutdownCallback = null) : base(microsoftAccount) {

        SignOutCommand = new AsyncRelayCommand(SignOut, _ => true, OnException);

        this.accountService = accountService;
        this.parentOwner = parentOwner;
        this.appSettings = appSettings;
        this.userSwitchCallback = userSwitchCallback;
        this.preventShutdownCallback = preventShutdownCallback;
    }

    private void OnException(Exception exception) {
        // No exceptions!
    }

    private async Task SignOut(object? arg) {
        MessageBoxResult result = HBDarkMessageBox
           .Show("Sign out",
               "Are you sure you want to sign out?",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes) {
            preventShutdownCallback?.Invoke();

            await accountService.LogoutAsync();
            parentOwner.Close();

            StartupLoginViewModel dataContext = new StartupLoginViewModel(accountService, appSettings);

            StartupLoginWindow loginWindow = new StartupLoginWindow();
            loginWindow.DataContext = dataContext;

            dataContext.StartupCompleted += userSwitchCallback;
            loginWindow.Show();
        }
    }
}
