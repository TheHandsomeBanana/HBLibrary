using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Account;

public class LocalAccountViewModel : ViewModelBase<LocalAccountModel> {

    public string Username {
        get { return Model.Username; }
        set {
            Model.Username = value;
            NotifyPropertyChanged();
        }
    }

    public LocalAccountViewModel(LocalAccountModel localAccount) : base(localAccount) {

    }
}
