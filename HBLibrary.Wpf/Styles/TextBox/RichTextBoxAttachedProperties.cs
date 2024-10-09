using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace HBLibrary.Wpf.Styles.TextBox;
public static class RichTextBoxAttachedProperties {
    public static readonly DependencyProperty BindableDocumentProperty =
        DependencyProperty.RegisterAttached(
            "BindableDocument",
            typeof(FlowDocument),
            typeof(RichTextBoxAttachedProperties),
            new PropertyMetadata(null, OnBindableDocumentChanged));

    public static FlowDocument GetBindableDocument(DependencyObject obj) {
        return (FlowDocument)obj.GetValue(BindableDocumentProperty);
    }

    public static void SetBindableDocument(DependencyObject obj, FlowDocument value) {
        obj.SetValue(BindableDocumentProperty, value);
    }

    private static void OnBindableDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is RichTextBox richTextBox && e.NewValue is FlowDocument newDocument) {
            try {
                richTextBox.Document = newDocument;
            }
            catch {
                // Swallow
            }
        }
    }
}
