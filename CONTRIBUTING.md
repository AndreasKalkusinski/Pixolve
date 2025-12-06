# Contributing to Pixolve

First off, thank you for considering contributing to Pixolve! It's people like you that make Pixolve such a great tool.

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues as you might find that you don't need to create one. When you are creating a bug report, please include as many details as possible:

* **Use a clear and descriptive title**
* **Describe the exact steps to reproduce the problem**
* **Provide specific examples to demonstrate the steps**
* **Describe the behavior you observed and what behavior you expected**
* **Include screenshots if possible**
* **Include your environment details** (OS, .NET version, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

* **Use a clear and descriptive title**
* **Provide a detailed description of the suggested enhancement**
* **Explain why this enhancement would be useful**
* **List some examples of how it would be used**

### Pull Requests

1. **Fork the repo** and create your branch from `master`
2. **Follow the coding standards** (see below)
3. **Add tests** if you're adding code that should be tested
4. **Ensure the test suite passes** (`dotnet test`)
5. **Update documentation** if needed
6. **Write a clear commit message**

## Development Setup

### Prerequisites

```bash
# Install .NET 9 SDK
https://dotnet.microsoft.com/download/dotnet/9.0

# Install Git
https://git-scm.com/downloads
```

### Getting Started

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/pixolve.git
cd pixolve

# Add upstream remote
git remote add upstream https://github.com/ORIGINAL-OWNER/pixolve.git

# Create a branch
git checkout -b feature/my-feature

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Run the application
dotnet run --project src/Pixolve.Desktop
```

## Coding Standards

### C# Style Guide

We follow the official [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

```csharp
// Good
public class ImageConverter
{
    private readonly IImageService _imageService;

    public ImageConverter(IImageService imageService)
    {
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    }

    public async Task<ConversionResult> ConvertAsync(ImageFile file)
    {
        // Implementation
    }
}
```

### Naming Conventions

* **Classes/Interfaces**: PascalCase (`ImageConverter`, `IImageService`)
* **Methods**: PascalCase (`ConvertAsync`, `GetFiles`)
* **Properties**: PascalCase (`FileName`, `Quality`)
* **Private fields**: _camelCase with underscore (`_imageService`)
* **Local variables**: camelCase (`fileName`, `quality`)
* **Constants**: PascalCase (`MaxImageSize`)

### Project Structure

```
Pixolve.Core/
├── Models/              # Data models, DTOs
├── Services/            # Business logic
│   ├── Converters/      # Image conversion implementations
│   ├── FileService/     # File operations
│   └── SettingsService/ # User settings
├── Interfaces/          # Service contracts
└── Exceptions/          # Custom exceptions

Pixolve.Desktop/
├── Views/               # XAML views
├── ViewModels/          # View models
├── Converters/          # Value converters
├── Assets/              # Resources
└── Helpers/             # UI helpers
```

### Testing

* Write unit tests for all business logic
* Use xUnit for testing
* Follow AAA pattern (Arrange, Act, Assert)

```csharp
[Fact]
public async Task ConvertAsync_WithValidFile_ReturnsSuccess()
{
    // Arrange
    var converter = new ImageConverter();
    var file = new ImageFile("test.jpg");

    // Act
    var result = await converter.ConvertAsync(file);

    // Assert
    Assert.True(result.IsSuccess);
}
```

### XAML Guidelines

* Use proper indentation (2 spaces)
* Keep views simple, logic in ViewModels
* Use data binding, avoid code-behind when possible

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="using:Pixolve.Desktop.ViewModels"
        x:Class="Pixolve.Desktop.Views.MainWindow">
  <Design.DataContext>
    <vm:MainWindowViewModel />
  </Design.DataContext>

  <Grid RowDefinitions="Auto,*">
    <!-- Content -->
  </Grid>
</Window>
```

## Git Workflow

### Branches

* `master` - Production-ready code
* `develop` - Development branch (if used)
* `feature/xxx` - New features
* `bugfix/xxx` - Bug fixes
* `hotfix/xxx` - Urgent fixes

### Commit Messages

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
feat: add AVIF format support
fix: resolve memory leak in batch conversion
docs: update installation instructions
refactor: simplify image loading logic
test: add unit tests for converter service
chore: update dependencies
```

### Pull Request Process

1. Update the README.md if needed
2. Update documentation
3. Ensure all tests pass
4. Update the CHANGELOG.md
5. Request review from maintainers
6. Address review feedback
7. Squash commits if needed

## Review Process

* All submissions require review
* Changes may be requested
* Be patient and respectful
* Reviewers will provide constructive feedback

## Recognition

Contributors will be:
* Listed in the CONTRIBUTORS.md file
* Mentioned in release notes
* Given credit in the README

## Questions?

Don't hesitate to ask questions:
* Create a GitHub Discussion
* Join our community chat (coming soon)
* Email the maintainers

Thank you for contributing to Pixolve! 🎉
