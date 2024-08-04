using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Login {
    public class MicrosoftLoginViewModel : ViewModelBase<MicrosoftLoginModel> {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
