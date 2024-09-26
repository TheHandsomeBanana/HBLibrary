using HBLibrary.Common.Initializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels;
public abstract class AsyncInitializerViewModelBase : ViewModelBase, IAsyncInitializer {

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



    public async Task InitializeAsync() {
        try {
            IsLoading = true;
            await InitializeViewModelAsync();
        }
        catch (Exception ex) {
            IsFaulted = true;
            OnException(ex);
        }
        finally {
            IsLoading = false;
        }
    }

    protected abstract Task InitializeViewModelAsync();
    protected abstract void OnException(Exception exception);
}

public abstract class AsyncInitializerViewModelBase<TModel> : ViewModelBase<TModel>, IAsyncInitializer {

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


    public AsyncInitializerViewModelBase(TModel model) : base(model) {

    }

    public async Task InitializeAsync() {
        try {
            IsLoading = true;
            await InitializeViewModelAsync();
        }
        catch (Exception ex) {
            IsFaulted = true;
            OnException(ex);
        }
        finally {
            IsLoading = false;
        }
    }

    protected abstract Task InitializeViewModelAsync();
    protected abstract void OnException(Exception exception);
}
