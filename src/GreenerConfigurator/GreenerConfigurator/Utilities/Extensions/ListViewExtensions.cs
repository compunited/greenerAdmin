using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace GreenerConfigurator.Utilities.Extensions
{
    public static class ListViewExtensions
    {
        public static readonly DependencyProperty ScrollIntoViewOnChangeProperty =
            DependencyProperty.RegisterAttached("ScrollIntoViewOnChange", typeof(bool), typeof(ListViewExtensions), new PropertyMetadata(false, OnScrollIntoViewOnChangeChanged));

        public static bool GetScrollIntoViewOnChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollIntoViewOnChangeProperty);
        }

        public static void SetScrollIntoViewOnChange(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollIntoViewOnChangeProperty, value);
        }

        private static void OnScrollIntoViewOnChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                listView.SelectionChanged -= ListView_SelectionChanged;
                if ((bool)e.NewValue)
                {
                    listView.SelectionChanged += ListView_SelectionChanged;
                }
            }
        }

        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                listView.Dispatcher.InvokeAsync(() =>
                {
                    listView.UpdateLayout();
                    listView.ScrollIntoView(listView.SelectedItem);
                });
            }
        }
    }

}
