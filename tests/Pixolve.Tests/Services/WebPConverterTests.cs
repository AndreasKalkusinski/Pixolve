using Pixolve.Core.Models;
using Pixolve.Core.Services.Converters;

namespace Pixolve.Tests.Services;

public class WebPConverterTests
{
    [Fact]
    public void TargetFormat_ShouldBeWebP()
    {
        // Arrange
        var converter = new WebPConverter();

        // Act
        var format = converter.TargetFormat;

        // Assert
        Assert.Equal(ImageFormat.WebP, format);
    }

    [Theory]
    [InlineData(ImageFormat.Jpeg, true)]
    [InlineData(ImageFormat.Png, true)]
    [InlineData(ImageFormat.Bmp, true)]
    [InlineData(ImageFormat.Gif, true)]
    [InlineData(ImageFormat.WebP, false)]
    [InlineData(ImageFormat.Avif, false)]
    [InlineData(ImageFormat.Unknown, false)]
    public void CanConvert_ShouldReturnCorrectValue(ImageFormat format, bool expected)
    {
        // Arrange
        var converter = new WebPConverter();

        // Act
        var result = converter.CanConvert(format);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ConvertAsync_WithNullImageFile_ShouldThrowArgumentNullException()
    {
        // Arrange
        var converter = new WebPConverter();
        var settings = new ConversionSettings();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => converter.ConvertAsync(null!, settings));
    }

    [Fact]
    public async Task ConvertAsync_WithNullSettings_ShouldThrowArgumentNullException()
    {
        // Arrange
        var converter = new WebPConverter();
        var imageFile = new ImageFile
        {
            FileName = "test.jpg",
            FilePath = "/tmp"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => converter.ConvertAsync(imageFile, null!));
    }

    [Fact]
    public async Task ConvertAsync_WithInvalidSettings_ShouldReturnFailure()
    {
        // Arrange
        var converter = new WebPConverter();
        var imageFile = new ImageFile
        {
            FileName = "test.jpg",
            FilePath = "/tmp"
        };
        var settings = new ConversionSettings { Quality = 150 }; // Invalid quality

        // Act
        var result = await converter.ConvertAsync(imageFile, settings);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ConvertAsync_WithNonExistentFile_ShouldReturnFailure()
    {
        // Arrange
        var converter = new WebPConverter();
        var imageFile = new ImageFile
        {
            FileName = "nonexistent.jpg",
            FilePath = "/tmp"
        };
        var settings = new ConversionSettings();

        // Act
        var result = await converter.ConvertAsync(imageFile, settings);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }
}
