using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pixolve.Core.Models;

/// <summary>
/// Represents an image file with conversion metadata
/// </summary>
public class ImageFile : INotifyPropertyChanged
{
    private string _fileName = string.Empty;
    private string _filePath = string.Empty;
    private long _fileSizeBefore;
    private long _fileSizeAfter;
    private string _pixelSizeBefore = string.Empty;
    private string _pixelSizeAfter = string.Empty;
    private ImageFormat _originalFormat;
    private ImageFormat _targetFormat;
    private bool _isConverted;
    private bool _isInProgress;
    private string _errorMessage = string.Empty;
    private string _status = "Warten";
    private byte[]? _thumbnailData;
    private int? _customQuality;
    private int? _customMaxPixelSize;
    private ImageFormat? _customTargetFormat;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Name of the image file (without path)
    /// </summary>
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }

    /// <summary>
    /// Full path to the image file
    /// </summary>
    public string FilePath
    {
        get => _filePath;
        set => SetProperty(ref _filePath, value);
    }

    /// <summary>
    /// Original file size in bytes
    /// </summary>
    public long FileSizeBefore
    {
        get => _fileSizeBefore;
        set
        {
            if (SetProperty(ref _fileSizeBefore, value))
            {
                OnPropertyChanged(nameof(FileSizeBeforeFormatted));
                OnPropertyChanged(nameof(CompressionRatio));
            }
        }
    }

    /// <summary>
    /// Converted file size in bytes
    /// </summary>
    public long FileSizeAfter
    {
        get => _fileSizeAfter;
        set
        {
            if (SetProperty(ref _fileSizeAfter, value))
            {
                OnPropertyChanged(nameof(FileSizeAfterFormatted));
                OnPropertyChanged(nameof(CompressionRatio));
                OnPropertyChanged(nameof(CompressionRatioFormatted));
            }
        }
    }

    /// <summary>
    /// Original image dimensions (e.g., "1920x1080")
    /// </summary>
    public string PixelSizeBefore
    {
        get => _pixelSizeBefore;
        set => SetProperty(ref _pixelSizeBefore, value);
    }

    /// <summary>
    /// Converted image dimensions (e.g., "1920x1080")
    /// </summary>
    public string PixelSizeAfter
    {
        get => _pixelSizeAfter;
        set => SetProperty(ref _pixelSizeAfter, value);
    }

    /// <summary>
    /// Original image format
    /// </summary>
    public ImageFormat OriginalFormat
    {
        get => _originalFormat;
        set => SetProperty(ref _originalFormat, value);
    }

    /// <summary>
    /// Target format for conversion
    /// </summary>
    public ImageFormat TargetFormat
    {
        get => _targetFormat;
        set => SetProperty(ref _targetFormat, value);
    }

    /// <summary>
    /// Indicates if the image has been successfully converted
    /// </summary>
    public bool IsConverted
    {
        get => _isConverted;
        set => SetProperty(ref _isConverted, value);
    }

    /// <summary>
    /// Indicates if conversion is currently in progress
    /// </summary>
    public bool IsInProgress
    {
        get => _isInProgress;
        set => SetProperty(ref _isInProgress, value);
    }

    /// <summary>
    /// Error message if conversion failed
    /// </summary>
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// Current status of the image (Warten, Konvertierung läuft, Erfolgreich, Fehler)
    /// </summary>
    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    /// <summary>
    /// Thumbnail image data for preview (small version of the image)
    /// </summary>
    public byte[]? ThumbnailData
    {
        get => _thumbnailData;
        set => SetProperty(ref _thumbnailData, value);
    }

    /// <summary>
    /// Custom quality setting for this specific image (overrides global setting)
    /// </summary>
    public int? CustomQuality
    {
        get => _customQuality;
        set => SetProperty(ref _customQuality, value);
    }

    /// <summary>
    /// Custom max pixel size for this specific image (overrides global setting)
    /// </summary>
    public int? CustomMaxPixelSize
    {
        get => _customMaxPixelSize;
        set => SetProperty(ref _customMaxPixelSize, value);
    }

    /// <summary>
    /// Custom target format for this specific image (overrides global setting)
    /// </summary>
    public ImageFormat? CustomTargetFormat
    {
        get => _customTargetFormat;
        set => SetProperty(ref _customTargetFormat, value);
    }

    /// <summary>
    /// Gets the file extension for the original format
    /// </summary>
    public string FileExtension => Path.GetExtension(FileName);

    /// <summary>
    /// Gets the full path including file name
    /// </summary>
    public string FullPath => Path.Combine(FilePath, FileName);

    /// <summary>
    /// Calculates the compression ratio (percentage saved)
    /// </summary>
    public double CompressionRatio
    {
        get
        {
            if (FileSizeBefore == 0 || FileSizeAfter == 0)
                return 0;

            return ((FileSizeBefore - FileSizeAfter) / (double)FileSizeBefore) * 100;
        }
    }

    /// <summary>
    /// Gets formatted compression ratio (e.g., "42.5%")
    /// </summary>
    public string CompressionRatioFormatted
    {
        get
        {
            if (FileSizeAfter == 0)
                return "";
            return $"{CompressionRatio:F1}%";
        }
    }

    /// <summary>
    /// Gets formatted file size before conversion (e.g., "1.5 MB")
    /// </summary>
    public string FileSizeBeforeFormatted => FormatFileSize(FileSizeBefore);

    /// <summary>
    /// Gets formatted file size after conversion (e.g., "500 KB")
    /// </summary>
    public string FileSizeAfterFormatted => FileSizeAfter == 0 ? "" : FormatFileSize(FileSizeAfter);

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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
