namespace Pixolve.Core.Models;

/// <summary>
/// Settings for image conversion
/// </summary>
public class ConversionSettings
{
    /// <summary>
    /// Image quality (0-100). Higher values mean better quality but larger file size.
    /// Default is 85.
    /// </summary>
    public int Quality { get; set; } = 85;

    /// <summary>
    /// Maximum width/height in pixels. Images larger than this will be resized.
    /// Null means no resizing. Default is 2048.
    /// </summary>
    public int? MaxPixelSize { get; set; } = 2048;

    /// <summary>
    /// Whether to resize images that exceed MaxPixelSize
    /// </summary>
    public bool EnableResizing { get; set; } = true;

    /// <summary>
    /// Target format for conversion
    /// </summary>
    public ImageFormat TargetFormat { get; set; } = ImageFormat.WebP;

    /// <summary>
    /// Output directory for converted images.
    /// Null means create "converted" subfolder in source directory.
    /// </summary>
    public string? OutputDirectory { get; set; }

    /// <summary>
    /// Overwrite existing files
    /// </summary>
    public bool OverwriteExisting { get; set; } = true;

    /// <summary>
    /// Preserve original file modification date
    /// </summary>
    public bool PreserveTimestamp { get; set; } = false;

    /// <summary>
    /// Creates a deep copy of the settings
    /// </summary>
    public ConversionSettings Clone()
    {
        return new ConversionSettings
        {
            Quality = Quality,
            MaxPixelSize = MaxPixelSize,
            EnableResizing = EnableResizing,
            TargetFormat = TargetFormat,
            OutputDirectory = OutputDirectory,
            OverwriteExisting = OverwriteExisting,
            PreserveTimestamp = PreserveTimestamp
        };
    }

    /// <summary>
    /// Validates the settings
    /// </summary>
    public (bool IsValid, string ErrorMessage) Validate()
    {
        if (Quality < 0 || Quality > 100)
            return (false, "Quality must be between 0 and 100");

        if (MaxPixelSize.HasValue && MaxPixelSize.Value <= 0)
            return (false, "MaxPixelSize must be greater than 0");

        if (TargetFormat == ImageFormat.Unknown)
            return (false, "TargetFormat must be specified");

        return (true, string.Empty);
    }
}
