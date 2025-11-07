using System.Text.Json;

namespace Pixolve.Core.Models;

/// <summary>
/// User settings that persist between sessions
/// </summary>
public class UserSettings
{
    public int Quality { get; set; } = 85;
    public int MaxPixelSize { get; set; } = 2048;
    public bool EnableResizing { get; set; } = true;
    public bool OverwriteExisting { get; set; } = true;
    public bool PreserveTimestamp { get; set; } = false;
    public bool IncludeSubfolders { get; set; } = false;
    public string OutputDirectory { get; set; } = string.Empty;
    public ImageFormat SelectedTargetFormat { get; set; } = ImageFormat.WebP;
    public AppTheme Theme { get; set; } = AppTheme.System;
    public AppLanguage Language { get; set; } = AppLanguage.German;

    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Pixolve",
        "settings.json");

    /// <summary>
    /// Load settings from disk or return default settings
    /// </summary>
    public static UserSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                var settings = JsonSerializer.Deserialize<UserSettings>(json);
                return settings ?? new UserSettings();
            }
        }
        catch
        {
            // If loading fails, return default settings
        }

        return new UserSettings();
    }

    /// <summary>
    /// Save settings to disk
    /// </summary>
    public void Save()
    {
        try
        {
            var directory = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(SettingsPath, json);
        }
        catch
        {
            // Silently fail if save fails
        }
    }
}
