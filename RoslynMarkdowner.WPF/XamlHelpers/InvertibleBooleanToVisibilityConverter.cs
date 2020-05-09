using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RoslynMarkdowner.WPF.XamlHelpers
{
    public class InvertibleBooleanToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool visible && visible != Invert ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility visibility && (visibility == Visibility.Visible) != Invert;
    }
}
