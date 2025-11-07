namespace Pixolve.Core.Models;

/// <summary>
/// Settings for multi-format export (one image to multiple formats)
/// </summary>
public class MultiFormatSettings
{
    /// <summary>
    /// Enable multi-format export
    /// </summary>
    public bool EnableMultiFormat { get; set; } = false;

    /// <summary>
    /// List of target formats to export
    /// </summary>
    public List<FormatQualityPair> TargetFormats { get; set; } = new();
}

/// <summary>
/// A pair of format and quality for multi-format export
/// </summary>
public class FormatQualityPair
{
    /// <summary>
    /// Image format
    /// </summary>
    public ImageFormat Format { get; set; } = ImageFormat.WebP;

    /// <summary>
    /// Quality setting for this format (0-100)
    /// Null means use global quality setting
    /// </summary>
    public int? Quality { get; set; }

    /// <summary>
    /// Display name for UI
    /// </summary>
    public string DisplayName => Format switch
    {
        ImageFormat.WebP => "WebP",
        ImageFormat.Png => "PNG",
        ImageFormat.Jpeg => "JPEG",
        ImageFormat.Avif => "AVIF",
        _ => "Unknown"
    };
}
