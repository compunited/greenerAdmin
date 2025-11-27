using System;
using System.Windows;
using System.Windows.Data;

namespace GreenerConfigurator.Helper
{
  public class NegateBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return !(bool)value;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return !(bool)value;
    }
  }

  public class NegateVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Visibility visProp = Visibility.Visible;
      switch ((Visibility)value)
      {
        case Visibility.Visible:
          visProp = Visibility.Hidden;
          break;
        case Visibility.Hidden:
          visProp = Visibility.Visible;
          break;
      }

      return visProp;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return !(bool)value;
    }
  }
}
