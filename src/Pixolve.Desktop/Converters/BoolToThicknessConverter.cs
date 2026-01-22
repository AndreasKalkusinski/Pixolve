using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pixolve.Desktop.Converters;

public class BoolToThicknessConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            return new Thickness(3); // 3px border when dragging
        }
        return new Thickness(0); // No border when not dragging
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
