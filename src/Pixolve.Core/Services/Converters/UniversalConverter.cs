using Pixolve.Core.Interfaces;
using Pixolve.Core.Models;
using SkiaSharp;
using ImageMagick;

namespace Pixolve.Core.Services.Converters;

/// <summary>
/// Universal image converter that supports multiple output formats
/// </summary>
public class UniversalConverter : IImageConverter
{
    public ImageFormat TargetFormat { get; private set; }

    public UniversalConverter()
    {
        TargetFormat = ImageFormat.WebP;
    }

    public bool CanConvert(ImageFormat format)
    {
        return format switch
        {
            ImageFormat.Jpeg => true,
            ImageFormat.Png => true,
            ImageFormat.Bmp => true,
            ImageFormat.Gif => true,
            ImageFormat.WebP => true,
            ImageFormat.Avif => true,
            // RAW formats
            ImageFormat.NikonRaw => true,
            ImageFormat.CanonRaw => true,
            ImageFormat.SonyRaw => true,
            ImageFormat.AdobeDng => true,
            ImageFormat.OtherRaw => true,
            _ => false
        };
    }

    private bool IsRawFormat(ImageFormat format)
    {
        return format is ImageFormat.NikonRaw
            or ImageFormat.CanonRaw
            or ImageFormat.SonyRaw
            or ImageFormat.AdobeDng
            or ImageFormat.OtherRaw;
    }

    public async Task<ConversionResult> ConvertAsync(
        ImageFile imageFile,
        ConversionSettings settings,
        CancellationToken cancellationToken = default)
    {
        if (imageFile == null)
            throw new ArgumentNullException(nameof(imageFile));

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        // Validate settings
        var validation = settings.Validate();
        if (!validation.IsValid)
            return ConversionResult.Failure(validation.ErrorMessage);

        try
        {
            var fullInputPath = imageFile.FullPath;

            if (!File.Exists(fullInputPath))
                return ConversionResult.Failure($"File not found: {fullInputPath}");

            SKBitmap? skBitmap = null;

            // Check if it's a RAW format
            var fileExtension = Path.GetExtension(fullInputPath).ToLowerInvariant();
            var sourceFormat = GetImageFormatFromExtension(fileExtension);

            if (IsRawFormat(sourceFormat))
            {
                // Use ImageMagick to decode RAW file
                skBitmap = await DecodeRawImageAsync(fullInputPath, cancellationToken);
            }
            else
            {
                // Use SkiaSharp for standard formats
                skBitmap = await Task.Run(() => SKBitmap.Decode(fullInputPath), cancellationToken);
            }

            if (skBitmap == null)
                return ConversionResult.Failure("Failed to decode image");

            SKBitmap? processedBitmap = null;

            try
            {
                // Check if resizing is needed
                if (settings.EnableResizing &&
                    settings.MaxPixelSize.HasValue &&
                    (skBitmap.Width > settings.MaxPixelSize || skBitmap.Height > settings.MaxPixelSize))
                {
                    processedBitmap = await ResizeImageAsync(skBitmap, settings.MaxPixelSize.Value, cancellationToken);
                }
                else
                {
                    processedBitmap = skBitmap;
                }

                // Determine output path with dimensions
                var outputPath = GetOutputPath(imageFile, settings, processedBitmap.Width, processedBitmap.Height);
                var outputDirectory = Path.GetDirectoryName(outputPath)!;

                // Create output directory if it doesn't exist
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Delete existing file if overwrite is enabled
                if (settings.OverwriteExisting && File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }

                // Encode to target format
                using var image = SKImage.FromBitmap(processedBitmap);
                var encodedFormat = GetSKEncodedFormat(settings.TargetFormat);
                using var data = await Task.Run(
                    () => image.Encode(encodedFormat, settings.Quality),
                    cancellationToken);

                if (data == null)
                    return ConversionResult.Failure($"Failed to encode image to {settings.TargetFormat}");

                // Save to file
                await using var fileStream = File.OpenWrite(outputPath);
                await Task.Run(() => data.SaveTo(fileStream), cancellationToken);

                // Get output file info
                var outputFileInfo = new FileInfo(outputPath);
                var outputDimensions = $"{processedBitmap.Width} x {processedBitmap.Height}";

                // Preserve timestamp if requested
                if (settings.PreserveTimestamp && File.Exists(fullInputPath))
                {
                    var originalFileInfo = new FileInfo(fullInputPath);
                    File.SetLastWriteTime(outputPath, originalFileInfo.LastWriteTime);
                }

                return ConversionResult.Success(outputPath, outputFileInfo.Length, outputDimensions);
            }
            finally
            {
                // Dispose resized bitmap if it was created
                if (processedBitmap != null && processedBitmap != skBitmap)
                {
                    processedBitmap.Dispose();
                }
            }
        }
        catch (OperationCanceledException)
        {
            return ConversionResult.Failure("Conversion was cancelled");
        }
        catch (Exception ex)
        {
            return ConversionResult.Failure($"Conversion failed: {ex.Message}", ex);
        }
    }

    private async Task<SKBitmap> ResizeImageAsync(SKBitmap originalBitmap, int maxSize, CancellationToken cancellationToken)
    {
        var aspectRatio = (double)originalBitmap.Width / originalBitmap.Height;

        int newWidth, newHeight;

        if (aspectRatio >= 1) // Width is greater or equal
        {
            newWidth = maxSize;
            newHeight = (int)(maxSize / aspectRatio);
        }
        else // Height is greater
        {
            newWidth = (int)(maxSize * aspectRatio);
            newHeight = maxSize;
        }

        var imageInfo = new SKImageInfo(newWidth, newHeight);

        return await Task.Run(
            () => originalBitmap.Resize(imageInfo, SKFilterQuality.High),
            cancellationToken);
    }

    private string GetOutputPath(ImageFile imageFile, ConversionSettings settings, int width, int height)
    {
        // Get the base filename without extension
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
        var extension = GetFileExtension(settings.TargetFormat);

        // Add max dimension to filename: filename-1920.webp (only the longer side)
        var maxDimension = Math.Max(width, height);
        var newFileName = $"{fileNameWithoutExtension}-{maxDimension}.{extension}";

        // Determine output directory
        string outputDirectory;

        if (!string.IsNullOrWhiteSpace(settings.OutputDirectory))
        {
            // Use specified output directory
            outputDirectory = settings.OutputDirectory;
        }
        else
        {
            // Create "converted" subfolder in source directory
            outputDirectory = Path.Combine(imageFile.FilePath, "converted");
        }

        return Path.Combine(outputDirectory, newFileName);
    }

    private SKEncodedImageFormat GetSKEncodedFormat(ImageFormat format)
    {
        return format switch
        {
            ImageFormat.WebP => SKEncodedImageFormat.Webp,
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Avif => SKEncodedImageFormat.Avif,
            _ => SKEncodedImageFormat.Webp
        };
    }

    private string GetFileExtension(ImageFormat format)
    {
        return format switch
        {
            ImageFormat.WebP => "webp",
            ImageFormat.Png => "png",
            ImageFormat.Jpeg => "jpg",
            ImageFormat.Avif => "avif",
            _ => "webp"
        };
    }

    private ImageFormat GetImageFormatFromExtension(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".bmp" => ImageFormat.Bmp,
            ".gif" => ImageFormat.Gif,
            ".webp" => ImageFormat.WebP,
            ".avif" => ImageFormat.Avif,
            ".nef" => ImageFormat.NikonRaw,
            ".cr2" or ".cr3" => ImageFormat.CanonRaw,
            ".arw" => ImageFormat.SonyRaw,
            ".dng" => ImageFormat.AdobeDng,
            ".raw" or ".raf" or ".orf" or ".rw2" or ".pef" or ".srw" or ".erf"
                => ImageFormat.OtherRaw,
            _ => ImageFormat.Unknown
        };
    }

    private async Task<SKBitmap?> DecodeRawImageAsync(string filePath, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            try
            {
                using var magickImage = new MagickImage(filePath);

                // Apply basic RAW processing
                magickImage.AutoOrient(); // Fix orientation based on EXIF

                // Convert to PNG bytes (lossless intermediate)
                var pngBytes = magickImage.ToByteArray(MagickFormat.Png);

                // Decode with SkiaSharp
                using var stream = new MemoryStream(pngBytes);
                return SKBitmap.Decode(stream);
            }
            catch (MagickException ex)
            {
                // Log or handle RAW decoding error
                Console.WriteLine($"RAW decode error: {ex.Message}");
                return null;
            }
        }, cancellationToken);
    }
}
