using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Quoridor.Converters
{
    public class WidthAsWindowPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Window.Current.Bounds.Width* System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}