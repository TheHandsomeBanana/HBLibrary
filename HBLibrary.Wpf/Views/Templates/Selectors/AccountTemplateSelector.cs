﻿using HBLibrary.Wpf.ViewModels;
using HBLibrary.Wpf.ViewModels.Account;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Views.Templates.Selectors {
    public class AccountTemplateSelector : DataTemplateSelector {
        public DataTemplate? LocalTemplate { get; set; }
        public DataTemplate? MicrosoftTemplate { get; set; }
        public DataTemplate? EmptyAccountTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container) {
            if (item is ViewModelBase accountViewModel) {
                switch (accountViewModel) {
                    case LocalAccountViewModel:
                        return LocalTemplate;
                    case MicrosoftAccountViewModel:
                        return MicrosoftTemplate;
                }
            }

            return EmptyAccountTemplate;
        }
    }
}
