using HBLibrary.Common.Initializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;
public abstract class InitializerViewModelBase : ViewModelBase, IInitializer {
    private bool isInitialized;
    public bool IsInitialized {
        get => isInitialized;
        private set {
            isInitialized = value;
            NotifyPropertyChanged();
        }
    }

    private bool isLoading;

    public bool IsLoading {
        get { return isLoading; }
        set {
            isLoading = value;
            NotifyPropertyChanged();
        }
    }

    private bool isFaulted;

    public bool IsFaulted {
        get { return isFaulted; }
        set { isFaulted = value; }
    }

    public InitializerViewModelBase() {

    }

    public void Initialize() {
        try {
            IsLoading = true;
            InitializeViewModel();
            IsInitialized = true;
        }
        finally {
            IsLoading = false;
        }
    }

    protected abstract void InitializeViewModel();
}
public abstract class InitializerViewModelBase<TModel> : ViewModelBase<TModel>, IInitializer {
    private bool isInitialized;
    public bool IsInitialized {
        get => isInitialized;
        private set {
            isInitialized = value;
            NotifyPropertyChanged();
        }
    }

    private bool isLoading;

    public bool IsLoading {
        get { return isLoading; }
        set {
            isLoading = value;
            NotifyPropertyChanged();
        }
    }

    private bool isFaulted;

    public bool IsFaulted {
        get { return isFaulted; }
        set { isFaulted = value; }
    }

    public InitializerViewModelBase(TModel model) : base(model) {

    }

    public void Initialize() {
        try {
            IsLoading = true;
            InitializeViewModel();
            IsInitialized = true;
        }
        finally {
            IsLoading = false;
        }
    }

    protected abstract void InitializeViewModel();
}
