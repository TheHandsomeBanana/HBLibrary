using HBLibrary.Common.Security;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Login {
    public class LocalLoginViewModel : ViewModelBase<LocalLoginModel> {
        public event EventHandler<bool>? ValidationPropertyChanged;

        public string Username {
            get => Model.Username;
            set {
                Model.Username = value;
                NotifyPropertyChanged();

                ValidationPropertyChanged?.Invoke(this, IsLoginInputValid());
            }
        }


        public SecureString SecurePassword {
            get => Model.SecurePassword;
            set {
                Model.SecurePassword = value;
                NotifyPropertyChanged();

                ValidationPropertyChanged?.Invoke(this, IsLoginInputValid());
            }
        }

        public LocalLoginViewModel() {
            Model = new LocalLoginModel();
        }

        public LocalLoginViewModel(LocalLoginModel model) : base(model) { }


        public void NotifyValidationPropertyChanged() {
            ValidationPropertyChanged?.Invoke(this, IsLoginInputValid());
        }


        private bool IsLoginInputValid() {
            return !string.IsNullOrEmpty(Model.Username) 
                && !string.IsNullOrEmpty(SStringConverter.SecureStringToString(Model.SecurePassword));
        }
    }
}
