using HBLibrary.Core;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Models;
using HBLibrary.Wpf.Views;
using System.Windows;

namespace HBLibrary.Wpf.ViewModels.Account;

public class LocalAccountViewModel : ViewModelBase<LocalAccountModel> {
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

    public AsyncRelayCommand DeleteLocalAccountCommand { get; set; }

    public LocalAccountViewModel(IAccountService accountService, Window parentOwner, LocalAccountModel localAccount, CommonAppSettings appSettings,
        Action<bool>? userSwitchCallback = null, Action? preventShutdownCallback = null) : base(localAccount) {


        DeleteLocalAccountCommand = new AsyncRelayCommand(DeleteAccount, o => true, OnDeleteException);
        this.accountService = accountService;
        this.parentOwner = parentOwner;
        this.appSettings = appSettings;
        this.userSwitchCallback = userSwitchCallback;
        this.preventShutdownCallback = preventShutdownCallback;
    }

    private void OnDeleteException(Exception exception) {
        HBDarkMessageBox.Show("Clear failed", exception.Message, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private async Task DeleteAccount(object? obj) {
        MessageBoxResult result = HBDarkMessageBox
            .Show("Delete local account",
                "Are you sure you want to delete this local account?",
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
