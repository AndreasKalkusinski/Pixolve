using Pixolve.Core.Models;

namespace Pixolve.Core.Interfaces;

/// <summary>
/// Interface for image conversion operations
/// </summary>
public interface IImageConverter
{
    /// <summary>
    /// Gets the target format this converter produces
    /// </summary>
    ImageFormat TargetFormat { get; }

    /// <summary>
    /// Converts an image file to the target format
    /// </summary>
    /// <param name="imageFile">The image file to convert</param>
    /// <param name="settings">Conversion settings</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Conversion result</returns>
    Task<ConversionResult> ConvertAsync(
        ImageFile imageFile,
        ConversionSettings settings,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the converter can handle the given input format
    /// </summary>
    /// <param name="format">Input image format</param>
    /// <returns>True if the format is supported</returns>
    bool CanConvert(ImageFormat format);
}
