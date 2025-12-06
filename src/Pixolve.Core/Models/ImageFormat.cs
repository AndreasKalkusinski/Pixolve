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
    Gif,

    /// <summary>
    /// RAW format - Nikon (.nef)
    /// </summary>
    NikonRaw,

    /// <summary>
    /// RAW format - Canon (.cr2, .cr3)
    /// </summary>
    CanonRaw,

    /// <summary>
    /// RAW format - Sony (.arw)
    /// </summary>
    SonyRaw,

    /// <summary>
    /// RAW format - Adobe DNG (.dng)
    /// </summary>
    AdobeDng,

    /// <summary>
    /// RAW format - Generic/Other (.raw, .raf, .orf, .rw2, etc.)
    /// </summary>
    OtherRaw
}
