using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pixolve.Core.Interfaces;
using Pixolve.Core.Models;
using Pixolve.Desktop.Resources;
using Pixolve.Desktop.Services;
using SkiaSharp;

namespace Pixolve.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IImageConverter _imageConverter;
    private CancellationTokenSource? _cancellationTokenSource;
    private IStorageProvider? _storageProvider;
    private bool _isLoadingSettings;

    public string AppVersion { get; }= GetAppVersion();

    private static string GetAppVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return $"v{version?.Major}.{version?.Minor}.{version?.Build}";
    }

    [ObservableProperty]
    private string _sourcePath = string.Empty;

    [ObservableProperty]
    private int _quality = 85;

    [ObservableProperty]
    private int _maxPixelSize = 2048;

    [ObservableProperty]
    private bool _enableResizing = true;

    [ObservableProperty]
    private bool _overwriteExisting = true;

    [ObservableProperty]
    private bool _preserveTimestamp = false;

    [ObservableProperty]
    private bool _includeSubfolders = false;

    [ObservableProperty]
    private string _outputDirectory = string.Empty;

    [ObservableProperty]
    private ImageFormat _selectedTargetFormat = ImageFormat.WebP;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedTargetFormat))]
    private ImageFormatOption? _selectedFormatOption;

    [ObservableProperty]
    private AppTheme _selectedTheme = AppTheme.System;

    [ObservableProperty]
    private AppLanguage _selectedLanguage = AppLanguage.German;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedTheme))]
    private ThemeOption? _selectedThemeOption;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedLanguage))]
    private LanguageOption? _selectedLanguageOption;

    [ObservableProperty]
    private bool _enableMultiFormat = false;

    [ObservableProperty]
    private bool _isConverting = false;

    [ObservableProperty]
    private int _progressValue = 0;

    [ObservableProperty]
    private int _progressMaximum = 100;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private bool _showOpenFolderButton = false;

    [ObservableProperty]
    private string _lastOutputDirectory = string.Empty;

    [ObservableProperty]
    private long _totalSizeBefore = 0;

    [ObservableProperty]
    private long _totalSizeAfter = 0;

    [ObservableProperty]
    private string _compressionStats = string.Empty;

    [ObservableProperty]
    private string _estimatedTimeRemaining = string.Empty;

    [ObservableProperty]
    private bool _isDraggingOver = false;

    private System.Diagnostics.Stopwatch? _conversionStopwatch;

    public ObservableCollection<ImageFile> ImageFiles { get; } = new();

    public ObservableCollection<ImageFormatOption> AvailableFormats { get; } = new()
    {
        new ImageFormatOption { Format = ImageFormat.WebP, DisplayName = "WebP" },
        new ImageFormatOption { Format = ImageFormat.Png, DisplayName = "PNG" },
        new ImageFormatOption { Format = ImageFormat.Jpeg, DisplayName = "JPEG" },
        new ImageFormatOption { Format = ImageFormat.Avif, DisplayName = "AVIF" }
    };

    public ObservableCollection<ThemeOption> AvailableThemes { get; } = new()
    {
        new ThemeOption { Theme = AppTheme.Light, DisplayName = "Hell" },
        new ThemeOption { Theme = AppTheme.Dark, DisplayName = "Dunkel" },
        new ThemeOption { Theme = AppTheme.System, DisplayName = "System" }
    };

    public ObservableCollection<LanguageOption> AvailableLanguages { get; } = new()
    {
        new LanguageOption { Language = AppLanguage.German, DisplayName = "Deutsch" },
        new LanguageOption { Language = AppLanguage.English, DisplayName = "English" },
        new LanguageOption { Language = AppLanguage.French, DisplayName = "Français" },
        new LanguageOption { Language = AppLanguage.Spanish, DisplayName = "Español" },
        new LanguageOption { Language = AppLanguage.Italian, DisplayName = "Italiano" }
    };

    public ObservableCollection<MultiFormatOption> MultiFormatOptions { get; } = new()
    {
        new MultiFormatOption { Format = ImageFormat.WebP, DisplayName = "WebP", IsSelected = false },
        new MultiFormatOption { Format = ImageFormat.Png, DisplayName = "PNG", IsSelected = false },
        new MultiFormatOption { Format = ImageFormat.Jpeg, DisplayName = "JPEG", IsSelected = false },
        new MultiFormatOption { Format = ImageFormat.Avif, DisplayName = "AVIF", IsSelected = false }
    };

    // Expose LocalizationService for binding
    public LocalizationService Localization => LocalizationService.Instance;

    public MainWindowViewModel(IImageConverter imageConverter)
    {
        _imageConverter = imageConverter;

        // Load user settings
        LoadSettings();
    }

    private void LoadSettings()
    {
        _isLoadingSettings = true;

        try
        {
            var settings = UserSettings.Load();

            Quality = settings.Quality;
            MaxPixelSize = settings.MaxPixelSize;
            EnableResizing = settings.EnableResizing;
            OverwriteExisting = settings.OverwriteExisting;
            PreserveTimestamp = settings.PreserveTimestamp;
            IncludeSubfolders = settings.IncludeSubfolders;
            OutputDirectory = settings.OutputDirectory;

            // Set the format option based on loaded format
            var formatOption = AvailableFormats.FirstOrDefault(f => f.Format == settings.SelectedTargetFormat);
            SelectedFormatOption = formatOption ?? AvailableFormats[0];

            // Load theme and language settings
            SelectedTheme = settings.Theme;
            SelectedLanguage = settings.Language;

            // Set the theme and language options based on loaded values
            var themeOption = AvailableThemes.FirstOrDefault(t => t.Theme == settings.Theme);
            SelectedThemeOption = themeOption ?? AvailableThemes[2]; // Default to System

            var languageOption = AvailableLanguages.FirstOrDefault(l => l.Language == settings.Language);
            SelectedLanguageOption = languageOption ?? AvailableLanguages[0]; // Default to German

            // Apply theme and language immediately
            ThemeService.ApplyTheme(SelectedTheme);
            LocalizationService.Instance.SetLanguage(SelectedLanguage);
        }
        finally
        {
            _isLoadingSettings = false;
        }
    }

    private void SaveSettings()
    {
        // Don't save during initial load
        if (_isLoadingSettings)
            return;

        var settings = new UserSettings
        {
            Quality = Quality,
            MaxPixelSize = MaxPixelSize,
            EnableResizing = EnableResizing,
            OverwriteExisting = OverwriteExisting,
            PreserveTimestamp = PreserveTimestamp,
            IncludeSubfolders = IncludeSubfolders,
            OutputDirectory = OutputDirectory,
            SelectedTargetFormat = SelectedTargetFormat,
            Theme = SelectedTheme,
            Language = SelectedLanguage
        };

        settings.Save();
    }

    partial void OnSelectedFormatOptionChanged(ImageFormatOption? value)
    {
        if (value != null)
        {
            SelectedTargetFormat = value.Format;
            SaveSettings();
        }
    }

    partial void OnQualityChanged(int value) => SaveSettings();
    partial void OnMaxPixelSizeChanged(int value) => SaveSettings();
    partial void OnEnableResizingChanged(bool value) => SaveSettings();
    partial void OnOverwriteExistingChanged(bool value) => SaveSettings();
    partial void OnPreserveTimestampChanged(bool value) => SaveSettings();
    partial void OnIncludeSubfoldersChanged(bool value) => SaveSettings();
    partial void OnOutputDirectoryChanged(string value) => SaveSettings();

    partial void OnSelectedThemeChanged(AppTheme value)
    {
        ThemeService.ApplyTheme(value);
        SaveSettings();
    }

    partial void OnSelectedThemeOptionChanged(ThemeOption? value)
    {
        if (value != null)
        {
            SelectedTheme = value.Theme;
        }
    }

    partial void OnSelectedLanguageOptionChanged(LanguageOption? value)
    {
        if (value != null)
        {
            SelectedLanguage = value.Language;
        }
    }

    partial void OnSelectedLanguageChanged(AppLanguage value)
    {
        LocalizationService.Instance.SetLanguage(value);
        SaveSettings();
    }

    public class ImageFormatOption
    {
        public ImageFormat Format { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public class ThemeOption
    {
        public AppTheme Theme { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public class LanguageOption
    {
        public AppLanguage Language { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public partial class MultiFormatOption : ObservableObject
    {
        public ImageFormat Format { get; set; }
        public string DisplayName { get; set; } = string.Empty;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private int? _customQuality;
    }

    public void SetStorageProvider(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;

        // Initialize status message after localization is available
        if (string.IsNullOrEmpty(StatusMessage))
        {
            StatusMessage = Localization.StatusReady;
        }
    }

    [RelayCommand]
    private async Task BrowseFolder()
    {
        if (_storageProvider == null)
        {
            StatusMessage = Localization.StatusStorageNotInitialized;
            return;
        }

        try
        {
            var folders = await _storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Ordner mit Bildern auswählen",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                var folder = folders[0];
                SourcePath = folder.Path.LocalPath;
                StatusMessage = string.Format(Localization.StatusFolderSelected, SourcePath);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format(Localization.StatusFolderSelectionError, ex.Message);
        }
    }

    [RelayCommand]
    private async Task BrowseOutputFolder()
    {
        if (_storageProvider == null)
        {
            StatusMessage = Localization.StatusStorageNotInitialized;
            return;
        }

        try
        {
            var folders = await _storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Ausgabeverzeichnis auswählen",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                var folder = folders[0];
                OutputDirectory = folder.Path.LocalPath;
                StatusMessage = string.Format(Localization.StatusOutputDirectorySelected, OutputDirectory);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format(Localization.StatusFolderSelectionError, ex.Message);
        }
    }

    [RelayCommand]
    private async Task LoadImages()
    {
        if (string.IsNullOrWhiteSpace(SourcePath) || !Directory.Exists(SourcePath))
        {
            StatusMessage = Localization.StatusSelectValidFolder;
            return;
        }

        try
        {
            IsLoading = true;
            ImageFiles.Clear();
            StatusMessage = Localization.StatusLoadingImages;

            var supportedExtensions = new[]
            {
                // Standard formats
                ".jpg", ".jpeg", ".png", ".bmp", ".gif",
                // RAW formats
                ".nef",           // Nikon
                ".cr2", ".cr3",   // Canon
                ".arw",           // Sony
                ".dng",           // Adobe
                ".raw", ".raf",   // Fuji
                ".orf",           // Olympus
                ".rw2",           // Panasonic
                ".pef",           // Pentax
                ".srw",           // Samsung
                ".erf"            // Epson
            };
            var searchOption = IncludeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var imageFiles = Directory.GetFiles(SourcePath, "*.*", searchOption)
                .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            ProgressMaximum = imageFiles.Count;
            ProgressValue = 0;

            int loadedCount = 0;
            foreach (var filePath in imageFiles)
            {
                var fileInfo = new FileInfo(filePath);
                var imageFile = new ImageFile
                {
                    FileName = fileInfo.Name,
                    FilePath = fileInfo.DirectoryName ?? string.Empty,
                    FileSizeBefore = fileInfo.Length,
                    Status = Localization.ImageStatusPending
                };

                // Try to get image dimensions and generate thumbnail
                try
                {
                    using var stream = File.OpenRead(filePath);
                    using var bitmap = SKBitmap.Decode(stream);
                    if (bitmap != null)
                    {
                        imageFile.PixelSizeBefore = $"{bitmap.Width} x {bitmap.Height}";

                        // Generate thumbnail (max 300px for high-quality preview)
                        const int thumbnailSize = 300;
                        var scale = Math.Min((float)thumbnailSize / bitmap.Width, (float)thumbnailSize / bitmap.Height);
                        var thumbnailWidth = (int)(bitmap.Width * scale);
                        var thumbnailHeight = (int)(bitmap.Height * scale);

                        var thumbnailInfo = new SKImageInfo(thumbnailWidth, thumbnailHeight);
                        using var thumbnail = bitmap.Resize(thumbnailInfo, SKFilterQuality.High);
                        if (thumbnail != null)
                        {
                            using var image = SKImage.FromBitmap(thumbnail);
                            using var data = image.Encode(SKEncodedImageFormat.Png, 90);
                            imageFile.ThumbnailData = data.ToArray();
                        }
                    }
                }
                catch
                {
                    // Ignore errors when reading dimensions
                }

                ImageFiles.Add(imageFile);

                loadedCount++;
                ProgressValue = loadedCount;
                StatusMessage = $"{Localization.StatusLoadingImages} ({loadedCount}/{imageFiles.Count})";

                // Allow UI to update every 10 images
                if (loadedCount % 10 == 0)
                {
                    await Task.Delay(1);
                }
            }

            StatusMessage = string.Format(Localization.StatusImagesLoaded, ImageFiles.Count);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format(Localization.StatusLoadImagesError, ex.Message);
        }
        finally
        {
            IsLoading = false;
            ProgressValue = 0;
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ConvertImages()
    {
        if (ImageFiles.Count == 0)
        {
            StatusMessage = Localization.StatusNoImagesToConvert;
            return;
        }

        try
        {
            IsConverting = true;
            _cancellationTokenSource = new CancellationTokenSource();

            ProgressMaximum = ImageFiles.Count;
            ProgressValue = 0;

            // Prepare multi-format settings if enabled
            MultiFormatSettings? multiFormat = null;
            if (EnableMultiFormat)
            {
                var selectedFormats = MultiFormatOptions.Where(o => o.IsSelected).ToList();
                if (selectedFormats.Any())
                {
                    multiFormat = new MultiFormatSettings
                    {
                        EnableMultiFormat = true,
                        TargetFormats = selectedFormats.Select(o => new FormatQualityPair
                        {
                            Format = o.Format,
                            Quality = o.CustomQuality
                        }).ToList()
                    };
                }
            }

            var settings = new ConversionSettings
            {
                Quality = Quality,
                MaxPixelSize = MaxPixelSize,
                EnableResizing = EnableResizing,
                TargetFormat = SelectedTargetFormat,
                OverwriteExisting = OverwriteExisting,
                PreserveTimestamp = PreserveTimestamp,
                OutputDirectory = OutputDirectory, // Use custom or default "converted" subfolder
                MultiFormat = multiFormat
            };

            var validation = settings.Validate();
            if (!validation.IsValid)
            {
                StatusMessage = string.Format(Localization.StatusInvalidSettings, validation.ErrorMessage);
                return;
            }

            // Filter only selected images
            var selectedImages = ImageFiles.Where(img => img.IsSelected).ToList();

            if (!selectedImages.Any())
            {
                StatusMessage = "Keine Bilder ausgewählt. Bitte wählen Sie mindestens ein Bild aus.";
                return;
            }

            int successCount = 0;
            int failureCount = 0;
            int currentIndex = 0;
            int totalImages = selectedImages.Count;

            // Reset statistics
            TotalSizeBefore = 0;
            TotalSizeAfter = 0;
            CompressionStats = string.Empty;
            EstimatedTimeRemaining = string.Empty;

            // Start stopwatch for time estimation
            _conversionStopwatch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var imageFile in selectedImages)
            {
                currentIndex++;

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    imageFile.Status = Localization.ImageStatusCancelled;
                    StatusMessage = Localization.StatusConversionCancelled;
                    break;
                }

                imageFile.Status = Localization.ImageStatusConverting;

                // Calculate estimated time remaining
                if (_conversionStopwatch != null && currentIndex > 1)
                {
                    double elapsedSeconds = _conversionStopwatch.Elapsed.TotalSeconds;
                    double averageSecondsPerImage = elapsedSeconds / (currentIndex - 1);
                    double remainingImages = totalImages - currentIndex + 1;
                    double estimatedSecondsRemaining = averageSecondsPerImage * remainingImages;

                    var timeRemaining = TimeSpan.FromSeconds(estimatedSecondsRemaining);
                    if (timeRemaining.TotalHours >= 1)
                    {
                        EstimatedTimeRemaining = $"≈ {(int)timeRemaining.TotalHours}h {timeRemaining.Minutes}m verbleibend";
                    }
                    else if (timeRemaining.TotalMinutes >= 1)
                    {
                        EstimatedTimeRemaining = $"≈ {(int)timeRemaining.TotalMinutes}m {timeRemaining.Seconds}s verbleibend";
                    }
                    else
                    {
                        EstimatedTimeRemaining = $"≈ {(int)timeRemaining.TotalSeconds}s verbleibend";
                    }
                }

                StatusMessage = $"Konvertiere Bild {currentIndex}/{totalImages}: {imageFile.FileName} {EstimatedTimeRemaining}";

                // Create settings for this specific image, using custom settings if available
                var imageSettings = new ConversionSettings
                {
                    Quality = imageFile.CustomQuality ?? settings.Quality,
                    MaxPixelSize = imageFile.CustomMaxPixelSize ?? settings.MaxPixelSize,
                    EnableResizing = settings.EnableResizing,
                    TargetFormat = imageFile.CustomTargetFormat ?? settings.TargetFormat,
                    OverwriteExisting = settings.OverwriteExisting,
                    PreserveTimestamp = settings.PreserveTimestamp,
                    OutputDirectory = settings.OutputDirectory,
                    MultiFormat = settings.MultiFormat
                };

                // Check if multi-format is enabled
                if (settings.MultiFormat?.EnableMultiFormat == true && settings.MultiFormat.TargetFormats.Any())
                {
                    // Convert to multiple formats
                    int formatSuccessCount = 0;
                    foreach (var formatPair in settings.MultiFormat.TargetFormats)
                    {
                        var formatSettings = imageSettings.Clone();
                        formatSettings.TargetFormat = formatPair.Format;
                        formatSettings.Quality = formatPair.Quality ?? imageSettings.Quality;
                        formatSettings.MultiFormat = null; // Disable multi-format for individual conversions

                        var result = await _imageConverter.ConvertAsync(
                            imageFile,
                            formatSettings,
                            _cancellationTokenSource.Token);

                        if (result.IsSuccess)
                        {
                            formatSuccessCount++;
                        }
                    }

                    if (formatSuccessCount == settings.MultiFormat.TargetFormats.Count)
                    {
                        imageFile.Status = string.Format(Localization.ImageStatusSuccessMulti, formatSuccessCount);
                        successCount++;
                    }
                    else if (formatSuccessCount > 0)
                    {
                        imageFile.Status = string.Format(Localization.ImageStatusPartial, formatSuccessCount, settings.MultiFormat.TargetFormats.Count);
                        successCount++;
                    }
                    else
                    {
                        imageFile.Status = Localization.ImageStatusError;
                        failureCount++;
                    }
                }
                else
                {
                    // Single format conversion
                    var result = await _imageConverter.ConvertAsync(
                        imageFile,
                        imageSettings,
                        _cancellationTokenSource.Token);

                    if (result.IsSuccess)
                    {
                        imageFile.FileSizeAfter = result.OutputSize;
                        imageFile.PixelSizeAfter = result.OutputDimensions ?? string.Empty;
                        imageFile.Status = Localization.ImageStatusSuccess;
                        successCount++;

                        // Track compression statistics
                        TotalSizeBefore += imageFile.FileSizeBefore;
                        TotalSizeAfter += result.OutputSize;
                    }
                    else
                    {
                        imageFile.Status = string.Format(Localization.ImageStatusErrorWithMessage, result.ErrorMessage);
                        failureCount++;
                    }
                }

                ProgressValue++;
            }

            // Calculate compression statistics
            if (TotalSizeBefore > 0 && successCount > 0)
            {
                double compressionRatio = (1.0 - ((double)TotalSizeAfter / TotalSizeBefore)) * 100;
                long savedBytes = TotalSizeBefore - TotalSizeAfter;

                var beforeSize = FormatFileSize(TotalSizeBefore);
                var afterSize = FormatFileSize(TotalSizeAfter);
                var savedSize = FormatFileSize(savedBytes);

                CompressionStats = $"{beforeSize} → {afterSize} (Einsparung: {savedSize}, {compressionRatio:F1}%)";
                StatusMessage = $"{string.Format(Localization.StatusConversionComplete, successCount, failureCount)} | {CompressionStats}";
            }
            else
            {
                StatusMessage = string.Format(Localization.StatusConversionComplete, successCount, failureCount);
            }

            // Show "Open Folder" button and save the output directory
            var actualOutputDir = string.IsNullOrWhiteSpace(OutputDirectory)
                ? Path.Combine(SourcePath, "converted")
                : OutputDirectory;

            if (Directory.Exists(actualOutputDir))
            {
                LastOutputDirectory = actualOutputDir;
                ShowOpenFolderButton = true;
            }
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format(Localization.StatusConversionError, ex.Message);
        }
        finally
        {
            IsConverting = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    [RelayCommand]
    private void OpenOutputFolder()
    {
        if (!string.IsNullOrWhiteSpace(LastOutputDirectory) && Directory.Exists(LastOutputDirectory))
        {
            try
            {
                // Cross-platform folder opening
                if (OperatingSystem.IsWindows())
                {
                    System.Diagnostics.Process.Start("explorer.exe", LastOutputDirectory);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    System.Diagnostics.Process.Start("open", LastOutputDirectory);
                }
                else if (OperatingSystem.IsLinux())
                {
                    System.Diagnostics.Process.Start("xdg-open", LastOutputDirectory);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Öffnen des Ordners: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private void CancelConversion()
    {
        _cancellationTokenSource?.Cancel();
        StatusMessage = Localization.StatusCancelling;
    }

    [RelayCommand]
    private void ClearImages()
    {
        ImageFiles.Clear();
        ProgressValue = 0;
        StatusMessage = Localization.StatusReady;
    }

    [RelayCommand]
    private void RemoveImage(ImageFile imageFile)
    {
        if (imageFile != null && ImageFiles.Contains(imageFile))
        {
            ImageFiles.Remove(imageFile);
            StatusMessage = string.Format(Localization.StatusFileRemoved, imageFile.FileName);
        }
    }

    [RelayCommand]
    private void ApplyWebPreset()
    {
        // Web optimiert: WebP, Qualität 80, Max 1920px
        SelectedFormatOption = AvailableFormats.FirstOrDefault(f => f.Format == ImageFormat.WebP);
        Quality = 80;
        MaxPixelSize = 1920;
        EnableResizing = true;
        StatusMessage = "Voreinstellung 'Web optimiert' angewendet (WebP, Q80, 1920px)";
    }

    [RelayCommand]
    private void ApplyQualityPreset()
    {
        // Maximale Qualität: PNG, Qualität 100, keine Größenbeschränkung
        SelectedFormatOption = AvailableFormats.FirstOrDefault(f => f.Format == ImageFormat.Png);
        Quality = 100;
        MaxPixelSize = 0;
        EnableResizing = false;
        StatusMessage = "Voreinstellung 'Maximale Qualität' angewendet (PNG, Q100, Originalgröße)";
    }

    [RelayCommand]
    private void ApplySizePreset()
    {
        // Minimale Größe: WebP, Qualität 60, Max 1280px
        SelectedFormatOption = AvailableFormats.FirstOrDefault(f => f.Format == ImageFormat.WebP);
        Quality = 60;
        MaxPixelSize = 1280;
        EnableResizing = true;
        StatusMessage = "Voreinstellung 'Minimale Größe' angewendet (WebP, Q60, 1280px)";
    }

    [RelayCommand]
    private void SelectAll()
    {
        foreach (var image in ImageFiles)
        {
            image.IsSelected = true;
        }
        StatusMessage = $"Alle {ImageFiles.Count} Bilder ausgewählt";
    }

    [RelayCommand]
    private void DeselectAll()
    {
        foreach (var image in ImageFiles)
        {
            image.IsSelected = false;
        }
        StatusMessage = "Alle Bilder abgewählt";
    }

    public async Task HandleFilesDropped(string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return;

        // Check if it's a directory or files
        if (paths.Length == 1 && Directory.Exists(paths[0]))
        {
            // Single folder dropped
            SourcePath = paths[0];
            StatusMessage = string.Format(Localization.StatusFolderDropped, SourcePath);
            await LoadImages();
        }
        else
        {
            // One or more files dropped - add only those files
            try
            {
                ImageFiles.Clear();
                StatusMessage = Localization.StatusLoadingImages;

                var supportedExtensions = new[]
            {
                // Standard formats
                ".jpg", ".jpeg", ".png", ".bmp", ".gif",
                // RAW formats
                ".nef",           // Nikon
                ".cr2", ".cr3",   // Canon
                ".arw",           // Sony
                ".dng",           // Adobe
                ".raw", ".raf",   // Fuji
                ".orf",           // Olympus
                ".rw2",           // Panasonic
                ".pef",           // Pentax
                ".srw",           // Samsung
                ".erf"            // Epson
            };
                int addedCount = 0;

                foreach (var filePath in paths)
                {
                    if (File.Exists(filePath) && supportedExtensions.Contains(Path.GetExtension(filePath).ToLowerInvariant()))
                    {
                        var fileInfo = new FileInfo(filePath);
                        var imageFile = new ImageFile
                        {
                            FileName = fileInfo.Name,
                            FilePath = fileInfo.DirectoryName ?? string.Empty,
                            FileSizeBefore = fileInfo.Length
                        };

                        // Try to get image dimensions and generate thumbnail
                        try
                        {
                            using var stream = File.OpenRead(filePath);
                            using var bitmap = SKBitmap.Decode(stream);
                            if (bitmap != null)
                            {
                                imageFile.PixelSizeBefore = $"{bitmap.Width} x {bitmap.Height}";

                                // Generate thumbnail (max 50px)
                                const int thumbnailSize = 50;
                                var scale = Math.Min((float)thumbnailSize / bitmap.Width, (float)thumbnailSize / bitmap.Height);
                                var thumbnailWidth = (int)(bitmap.Width * scale);
                                var thumbnailHeight = (int)(bitmap.Height * scale);

                                var thumbnailInfo = new SKImageInfo(thumbnailWidth, thumbnailHeight);
                                using var thumbnail = bitmap.Resize(thumbnailInfo, SKFilterQuality.Medium);
                                if (thumbnail != null)
                                {
                                    using var image = SKImage.FromBitmap(thumbnail);
                                    using var data = image.Encode(SKEncodedImageFormat.Png, 80);
                                    imageFile.ThumbnailData = data.ToArray();
                                }
                            }
                        }
                        catch
                        {
                            // Ignore errors when reading dimensions
                        }

                        ImageFiles.Add(imageFile);
                        addedCount++;
                    }
                }

                StatusMessage = $"{addedCount} Bild(er) per Drag & Drop hinzugefügt";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden der Bilder: {ex.Message}";
            }
        }
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes == 0)
            return "0 B";

        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = 0;
        double size = bytes;

        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:0.##} {sizes[order]}";
    }
}
