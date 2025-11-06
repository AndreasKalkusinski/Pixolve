namespace Pixolve.Core.Models;

/// <summary>
/// Result of an image conversion operation
/// </summary>
public class ConversionResult
{
    /// <summary>
    /// Indicates if the conversion was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Path to the output file
    /// </summary>
    public string? OutputPath { get; set; }

    /// <summary>
    /// Size of the output file in bytes
    /// </summary>
    public long OutputSize { get; set; }

    /// <summary>
    /// Dimensions of the output image (e.g., "1920x1080")
    /// </summary>
    public string? OutputDimensions { get; set; }

    /// <summary>
    /// Error message if conversion failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Exception that caused the failure (if any)
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static ConversionResult Success(string outputPath, long outputSize, string outputDimensions)
    {
        return new ConversionResult
        {
            IsSuccess = true,
            OutputPath = outputPath,
            OutputSize = outputSize,
            OutputDimensions = outputDimensions
        };
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static ConversionResult Failure(string errorMessage, Exception? exception = null)
    {
        return new ConversionResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            Exception = exception
        };
    }
}
