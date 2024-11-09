using HBLibrary.Wpf.Commands;
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

    public bool ShowTimestamp {
        get { return (bool)GetValue(ShowTimestampProperty); }
        set { SetValue(ShowTimestampProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShowTimestamp.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowTimestampProperty =
        DependencyProperty.Register("ShowTimestamp", typeof(bool), typeof(LogListBox), new PropertyMetadata(false));

    public bool ShowExplicitLevel {
        get { return (bool)GetValue(ShowExplicitLevelProperty); }
        set { SetValue(ShowExplicitLevelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShowExplicitLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowExplicitLevelProperty =
        DependencyProperty.Register("ShowExplicitLevel", typeof(bool), typeof(LogListBox), new PropertyMetadata(false));


    public RelayCommand ClearLogsCommand { get; set; }
    public RelayCommand ShowTimestampCommand { get; set; }
    public RelayCommand ShowExplicitLevelCommand { get; set; }


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

    protected override DependencyObject GetContainerForItemOverride() => new LogListBoxItem();
    protected override bool IsItemItsOwnContainerOverride(object item) => item is LogListBoxItem;



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

        // Initial assignment of line numbers
        UpdateLineNumbers();
    }

    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        switch (e.Action) {
            case NotifyCollectionChangedAction.Add:
                // Assign line numbers for new items and shift items below them
                UpdateLineNumbersFromIndex(e.NewStartingIndex);
                break;
        }
    }

    private void UpdateLineNumbers() {
        for (int i = 0; i < Items.Count; i++) {
            if (Items[i] is LogListBoxItem item) {
                item.LineNumber = i + 1;
            }
        }
    }

    private void UpdateLineNumbersFromIndex(int startIndex) {
        for (int i = startIndex; i < Items.Count; i++) {
            if (Items[i] is LogListBoxItem item) {
                item.LineNumber = i + 1;
            }
        }
    }
}
