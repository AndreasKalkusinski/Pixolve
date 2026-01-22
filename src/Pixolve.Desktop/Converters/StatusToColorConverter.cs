using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Pixolve.Desktop.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            // Success statuses - Green
            if (status.Contains("Erfolg", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Success", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Konvertiert", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Converted", StringComparison.OrdinalIgnoreCase))
            {
                return new SolidColorBrush(Color.Parse("#4CAF50")); // Green
            }

            // Error statuses - Red
            if (status.Contains("Fehler", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Fehlgeschlagen", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Failed", StringComparison.OrdinalIgnoreCase))
            {
                return new SolidColorBrush(Color.Parse("#F44336")); // Red
            }

            // In Progress statuses - Orange
            if (status.Contains("Bearbeitung", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Converting", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Wird konvertiert", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("Processing", StringComparison.OrdinalIgnoreCase))
            {
                return new SolidColorBrush(Color.Parse("#FF9800")); // Orange
            }
        }

        // Default - Gray for waiting/empty
        return new SolidColorBrush(Color.Parse("#9E9E9E"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
