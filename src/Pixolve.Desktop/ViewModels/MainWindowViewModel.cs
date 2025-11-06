using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pixolve.Core.Interfaces;
using Pixolve.Core.Models;
using SkiaSharp;

namespace Pixolve.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IImageConverter _imageConverter;
    private CancellationTokenSource? _cancellationTokenSource;
    private IStorageProvider? _storageProvider;
    private bool _isLoadingSettings;

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
    private bool _isConverting = false;

    [ObservableProperty]
    private int _progressValue = 0;

    [ObservableProperty]
    private int _progressMaximum = 100;

    [ObservableProperty]
    private string _statusMessage = "Bereit";

    public ObservableCollection<ImageFile> ImageFiles { get; } = new();

    public ObservableCollection<ImageFormatOption> AvailableFormats { get; } = new()
    {
        new ImageFormatOption { Format = ImageFormat.WebP, DisplayName = "WebP" },
        new ImageFormatOption { Format = ImageFormat.Png, DisplayName = "PNG" },
        new ImageFormatOption { Format = ImageFormat.Jpeg, DisplayName = "JPEG" },
        new ImageFormatOption { Format = ImageFormat.Avif, DisplayName = "AVIF" }
    };

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
            SelectedTargetFormat = SelectedTargetFormat
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

    public class ImageFormatOption
    {
        public ImageFormat Format { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public void SetStorageProvider(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    [RelayCommand]
    private async Task BrowseFolder()
    {
        if (_storageProvider == null)
        {
            StatusMessage = "Speicheranbieter nicht initialisiert";
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
                StatusMessage = $"Ordner ausgewählt: {SourcePath}";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler bei der Ordnerauswahl: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task BrowseOutputFolder()
    {
        if (_storageProvider == null)
        {
            StatusMessage = "Speicheranbieter nicht initialisiert";
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
                StatusMessage = $"Ausgabeverzeichnis ausgewählt: {OutputDirectory}";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler bei der Ordnerauswahl: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task LoadImages()
    {
        if (string.IsNullOrWhiteSpace(SourcePath) || !Directory.Exists(SourcePath))
        {
            StatusMessage = "Bitte einen gültigen Quellordner auswählen";
            return;
        }

        try
        {
            ImageFiles.Clear();
            StatusMessage = "Lade Bilder...";

            var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            var searchOption = IncludeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var imageFiles = Directory.GetFiles(SourcePath, "*.*", searchOption)
                .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            foreach (var filePath in imageFiles)
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
            }

            StatusMessage = $"{ImageFiles.Count} Bilder geladen";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler beim Laden der Bilder: {ex.Message}";
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ConvertImages()
    {
        if (ImageFiles.Count == 0)
        {
            StatusMessage = "Keine Bilder zum Konvertieren vorhanden";
            return;
        }

        try
        {
            IsConverting = true;
            _cancellationTokenSource = new CancellationTokenSource();

            ProgressMaximum = ImageFiles.Count;
            ProgressValue = 0;

            var settings = new ConversionSettings
            {
                Quality = Quality,
                MaxPixelSize = MaxPixelSize,
                EnableResizing = EnableResizing,
                TargetFormat = SelectedTargetFormat,
                OverwriteExisting = OverwriteExisting,
                PreserveTimestamp = PreserveTimestamp,
                OutputDirectory = OutputDirectory // Use custom or default "converted" subfolder
            };

            var validation = settings.Validate();
            if (!validation.IsValid)
            {
                StatusMessage = $"Ungültige Einstellungen: {validation.ErrorMessage}";
                return;
            }

            int successCount = 0;
            int failureCount = 0;

            foreach (var imageFile in ImageFiles)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    imageFile.Status = "Abgebrochen";
                    StatusMessage = "Konvertierung abgebrochen";
                    break;
                }

                imageFile.Status = "Konvertierung läuft...";
                StatusMessage = $"Konvertiere {imageFile.FileName}...";

                // Create settings for this specific image, using custom settings if available
                var imageSettings = new ConversionSettings
                {
                    Quality = imageFile.CustomQuality ?? settings.Quality,
                    MaxPixelSize = imageFile.CustomMaxPixelSize ?? settings.MaxPixelSize,
                    EnableResizing = settings.EnableResizing,
                    TargetFormat = imageFile.CustomTargetFormat ?? settings.TargetFormat,
                    OverwriteExisting = settings.OverwriteExisting,
                    PreserveTimestamp = settings.PreserveTimestamp,
                    OutputDirectory = settings.OutputDirectory
                };

                var result = await _imageConverter.ConvertAsync(
                    imageFile,
                    imageSettings,
                    _cancellationTokenSource.Token);

                if (result.IsSuccess)
                {
                    imageFile.FileSizeAfter = result.OutputSize;
                    imageFile.PixelSizeAfter = result.OutputDimensions ?? string.Empty;
                    imageFile.Status = "Erfolgreich";
                    successCount++;
                }
                else
                {
                    imageFile.Status = $"Fehler: {result.ErrorMessage}";
                    failureCount++;
                }

                ProgressValue++;
            }

            StatusMessage = $"Konvertierung abgeschlossen: {successCount} erfolgreich, {failureCount} fehlgeschlagen";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler bei der Konvertierung: {ex.Message}";
        }
        finally
        {
            IsConverting = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    [RelayCommand]
    private void CancelConversion()
    {
        _cancellationTokenSource?.Cancel();
        StatusMessage = "Konvertierung wird abgebrochen...";
    }

    [RelayCommand]
    private void ClearImages()
    {
        ImageFiles.Clear();
        ProgressValue = 0;
        StatusMessage = "Bereit";
    }

    [RelayCommand]
    private void RemoveImage(ImageFile imageFile)
    {
        if (imageFile != null && ImageFiles.Contains(imageFile))
        {
            ImageFiles.Remove(imageFile);
            StatusMessage = $"Datei entfernt: {imageFile.FileName}";
        }
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
            StatusMessage = $"Ordner per Drag & Drop ausgewählt: {SourcePath}";
            await LoadImages();
        }
        else
        {
            // One or more files dropped - add only those files
            try
            {
                ImageFiles.Clear();
                StatusMessage = "Lade Bilder...";

                var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
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
}
