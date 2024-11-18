using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HBLibrary.Wpf.Controls;
public class LogListBox : ListBox {
    static LogListBox() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LogListBox), new FrameworkPropertyMetadata(typeof(LogListBox)));
    }

    // Properties to toggle timestamp and explicit level display
    public bool ShowTimestamp {
        get { return (bool)GetValue(ShowTimestampProperty); }
        set { SetValue(ShowTimestampProperty, value); }
    }

    public static readonly DependencyProperty ShowTimestampProperty =
        DependencyProperty.Register("ShowTimestamp", typeof(bool), typeof(LogListBox), new PropertyMetadata(false));

    public bool ShowExplicitLevel {
        get { return (bool)GetValue(ShowExplicitLevelProperty); }
        set { SetValue(ShowExplicitLevelProperty, value); }
    }

    public static readonly DependencyProperty ShowExplicitLevelProperty =
        DependencyProperty.Register("ShowExplicitLevel", typeof(bool), typeof(LogListBox), new PropertyMetadata(false));



    public bool ShowCategory {
        get { return (bool)GetValue(ShowCategoryProperty); }
        set { SetValue(ShowCategoryProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShowCategory.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowCategoryProperty =
        DependencyProperty.Register("ShowCategory", typeof(bool), typeof(LogListBox), new PropertyMetadata(false));




    public bool CanClearLogs {
        get { return (bool)GetValue(CanClearLogsProperty); }
        set { SetValue(CanClearLogsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CanClearLogs.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CanClearLogsProperty =
        DependencyProperty.Register("CanClearLogs", typeof(bool), typeof(LogListBox), new PropertyMetadata(false, OnCanClearLogsChanged));

    private static void OnCanClearLogsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if(d is LogListBox) {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public ICommand ClearLogsCommand { get; }

    public LogListBox() {
        ClearLogsCommand = new RelayCommand(ClearLogs, o => CanClearLogs);
    }

    private void ClearLogs(object? obj) {
        (ItemsSource as ObservableCollection<ListBoxLog>)?.Clear();
    }

    private INotifyCollectionChanged? _observableItemsSource;

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue) {
        base.OnItemsSourceChanged(oldValue, newValue);

        if (_observableItemsSource != null) {
            _observableItemsSource.CollectionChanged -= OnItemsSourceCollectionChanged;
        }

        if (newValue is INotifyCollectionChanged newObservableCollection) {
            _observableItemsSource = newObservableCollection;
            _observableItemsSource.CollectionChanged += OnItemsSourceCollectionChanged;
        }
        else {
            _observableItemsSource = null;
        }

        // Initial line number assignment
        UpdateLineNumbers();
    }

    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        switch (e.Action) {
            case NotifyCollectionChangedAction.Add:
                UpdateLineNumbersFromIndex(e.NewStartingIndex);
                break;
        }
    }

    private void UpdateLineNumbers() {
        for (int i = 0; i < Items.Count; i++) {
            if (Items[i] is ListBoxLog log) {
                log.LineNumber = i + 1;
            }
        }
    }

    private void UpdateLineNumbersFromIndex(int startIndex) {
        for (int i = startIndex; i < Items.Count; i++) {
            if (Items[i] is ListBoxLog log) {
                log.LineNumber = i + 1;
            }
        }
    }
}
