using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Account;

public class MicrosoftAccountViewModel : ViewModelBase<MicrosoftAccountModel> {

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

    public MicrosoftAccountViewModel(MicrosoftAccountModel microsoftAccount) : base(microsoftAccount) {

    }

}
