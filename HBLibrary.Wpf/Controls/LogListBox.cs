using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
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

    // Commands for the UI actions
    public ICommand ClearLogsCommand { get; }
    public ICommand ShowTimestampCommand { get; }
    public ICommand ShowExplicitLevelCommand { get; }

    public LogListBox() {
        ClearLogsCommand = new RelayCommand(ClearLogs);
        ShowTimestampCommand = new RelayCommand(ToggleTimestamp);
        ShowExplicitLevelCommand = new RelayCommand(ToggleExplicitLevel);
    }

    private void ToggleExplicitLevel(object? obj) {
        ShowExplicitLevel = !ShowExplicitLevel;
    }

    private void ToggleTimestamp(object? obj) {
        ShowTimestamp = !ShowTimestamp;
    }

    private void ClearLogs(object? obj) {
        Items.Clear();
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
