using Pixolve.Core.Models;

namespace Pixolve.Tests.Models;

public class ConversionSettingsTests
{
    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var settings = new ConversionSettings();

        // Assert
        Assert.Equal(85, settings.Quality);
        Assert.Equal(2048, settings.MaxPixelSize);
        Assert.True(settings.EnableResizing);
        Assert.Equal(ImageFormat.WebP, settings.TargetFormat);
        Assert.True(settings.OverwriteExisting);
        Assert.False(settings.PreserveTimestamp);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(50, true)]
    [InlineData(85, true)]
    [InlineData(100, true)]
    [InlineData(-1, false)]
    [InlineData(101, false)]
    public void Validate_WithDifferentQualityValues_ShouldReturnCorrectResult(int quality, bool expectedValid)
    {
        // Arrange
        var settings = new ConversionSettings { Quality = quality };

        // Act
        var (isValid, _) = settings.Validate();

        // Assert
        Assert.Equal(expectedValid, isValid);
    }

    [Fact]
    public void Validate_WithUnknownFormat_ShouldReturnInvalid()
    {
        // Arrange
        var settings = new ConversionSettings { TargetFormat = ImageFormat.Unknown };

        // Act
        var (isValid, errorMessage) = settings.Validate();

        // Assert
        Assert.False(isValid);
        Assert.Contains("TargetFormat", errorMessage);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(1024, true)]
    [InlineData(4096, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void Validate_WithDifferentMaxPixelSize_ShouldReturnCorrectResult(int maxPixelSize, bool expectedValid)
    {
        // Arrange
        var settings = new ConversionSettings { MaxPixelSize = maxPixelSize };

        // Act
        var (isValid, _) = settings.Validate();

        // Assert
        Assert.Equal(expectedValid, isValid);
    }

    [Fact]
    public void Clone_ShouldCreateDeepCopy()
    {
        // Arrange
        var original = new ConversionSettings
        {
            Quality = 75,
            MaxPixelSize = 1024,
            EnableResizing = false,
            TargetFormat = ImageFormat.Png,
            OutputDirectory = "/tmp/output",
            OverwriteExisting = false,
            PreserveTimestamp = true
        };

        // Act
        var clone = original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.Quality, clone.Quality);
        Assert.Equal(original.MaxPixelSize, clone.MaxPixelSize);
        Assert.Equal(original.EnableResizing, clone.EnableResizing);
        Assert.Equal(original.TargetFormat, clone.TargetFormat);
        Assert.Equal(original.OutputDirectory, clone.OutputDirectory);
        Assert.Equal(original.OverwriteExisting, clone.OverwriteExisting);
        Assert.Equal(original.PreserveTimestamp, clone.PreserveTimestamp);
    }
}
