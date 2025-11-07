using Avalonia;
using Avalonia.Styling;
using Pixolve.Core.Models;

namespace Pixolve.Desktop.Services;

/// <summary>
/// Service for managing application theme
/// </summary>
public class ThemeService
{
    /// <summary>
    /// Apply the specified theme to the application
    /// </summary>
    public static void ApplyTheme(AppTheme theme)
    {
        if (Application.Current == null)
            return;

        var themeVariant = theme switch
        {
            AppTheme.Light => ThemeVariant.Light,
            AppTheme.Dark => ThemeVariant.Dark,
            AppTheme.System => DetectSystemTheme(),
            _ => ThemeVariant.Default
        };

        Application.Current.RequestedThemeVariant = themeVariant;
    }

    /// <summary>
    /// Detect the system theme preference
    /// </summary>
    private static ThemeVariant DetectSystemTheme()
    {
        // Avalonia automatically detects system theme when using ThemeVariant.Default
        // But we can also explicitly check platform APIs if needed
        return ThemeVariant.Default;
    }
}
