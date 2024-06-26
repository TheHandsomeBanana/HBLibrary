﻿using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public interface ISingleNavigationService<TViewModel> where TViewModel : ViewModelBase
{
    public void Navigate();
}