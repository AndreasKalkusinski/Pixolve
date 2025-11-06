namespace Pixolve.Core.Models;

/// <summary>
/// Supported image formats for conversion
/// </summary>
public enum ImageFormat
{
    /// <summary>
    /// Unknown or unsupported format
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// JPEG format (.jpg, .jpeg)
    /// </summary>
    Jpeg,

    /// <summary>
    /// PNG format (.png)
    /// </summary>
    Png,

    /// <summary>
    /// WebP format (.webp)
    /// </summary>
    WebP,

    /// <summary>
    /// AVIF format (.avif) - Future support
    /// </summary>
    Avif,

    /// <summary>
    /// BMP format (.bmp)
    /// </summary>
    Bmp,

    /// <summary>
    /// GIF format (.gif)
    /// </summary>
    Gif
}
