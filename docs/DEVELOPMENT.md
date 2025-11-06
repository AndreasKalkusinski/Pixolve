# Development Guide

This guide will help you set up your development environment and understand the development workflow for Pixolve.

## Prerequisites

### Required Software

1. **.NET 9 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet/9.0
   - Verify: `dotnet --version` (should show 9.0.x)

2. **Git**
   - Download: https://git-scm.com/downloads
   - Verify: `git --version`

3. **IDE** (choose one)
   - **Visual Studio 2022** (Windows/Mac)
     - Workload: ".NET Desktop Development"
     - Component: "Avalonia for Visual Studio 2022"
   - **Visual Studio Code**
     - Extensions:
       - C# Dev Kit
       - Avalonia for VSCode
   - **JetBrains Rider**
     - Built-in Avalonia support

### Recommended Tools

- **Git GUI**: GitHub Desktop, GitKraken, or SourceTree
- **REST Client**: Postman or Insomnia (for API testing, future)
- **Image Tools**: GIMP, Photoshop (for testing different image formats)

## Initial Setup

### 1. Clone the Repository

```bash
# HTTPS
git clone https://github.com/yourusername/pixolve.git

# SSH
git clone git@github.com:yourusername/pixolve.git

cd pixolve
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
# Build all projects
dotnet build

# Or build specific project
dotnet build src/Pixolve.Desktop
```

### 4. Run the Application

```bash
# From solution root
dotnet run --project src/Pixolve.Desktop/Pixolve.Desktop.csproj

# Or navigate to project
cd src/Pixolve.Desktop
dotnet run
```

### 5. Run Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~WebPConverterTests"
```

## Project Structure Explained

```
Pixolve/
├── .github/                    # GitHub-specific files
│   └── workflows/              # CI/CD workflows
├── docs/                       # Documentation
│   ├── ARCHITECTURE.md         # System architecture
│   ├── DEVELOPMENT.md          # This file
│   └── API.md                  # API documentation
├── src/                        # Source code
│   ├── Pixolve.Desktop/        # UI project
│   └── Pixolve.Core/           # Business logic
├── tests/                      # Test projects
│   └── Pixolve.Tests/
├── .gitignore                  # Git ignore rules
├── LICENSE                     # MIT License
├── README.md                   # Project readme
├── CONTRIBUTING.md             # Contribution guidelines
└── Pixolve.sln                 # Solution file
```

## Development Workflow

### Creating a New Feature

1. **Create a branch**
   ```bash
   git checkout -b feature/my-awesome-feature
   ```

2. **Make changes**
   - Write code in appropriate layer (Core vs Desktop)
   - Follow coding standards (see CONTRIBUTING.md)
   - Add unit tests

3. **Test locally**
   ```bash
   dotnet test
   dotnet run --project src/Pixolve.Desktop
   ```

4. **Commit changes**
   ```bash
   git add .
   git commit -m "feat: add awesome feature"
   ```

5. **Push and create PR**
   ```bash
   git push origin feature/my-awesome-feature
   # Then create Pull Request on GitHub
   ```

### Working with Avalonia UI

#### Hot Reload

Avalonia supports XAML hot reload:

```bash
# Run with hot reload
dotnet watch --project src/Pixolve.Desktop run
```

Changes to XAML files will be reflected immediately without restart.

#### Previewer

Use the Avalonia XAML Previewer in your IDE:

**Visual Studio Code:**
1. Open any `.axaml` file
2. Press `Ctrl+F5` (Windows/Linux) or `Cmd+F5` (Mac)
3. Previewer opens in new panel

**Visual Studio 2022:**
- Split view shows design preview automatically

**JetBrains Rider:**
- Split view with live preview

### Debugging

#### Visual Studio Code

1. Open the project in VS Code
2. Set breakpoints in code
3. Press `F5` or go to Run & Debug
4. Select "Pixolve Debug" configuration

#### Visual Studio 2022

1. Open `Pixolve.sln`
2. Set `Pixolve.Desktop` as startup project
3. Set breakpoints
4. Press `F5` to debug

#### Rider

1. Open `Pixolve.sln`
2. Set breakpoints
3. Click Debug icon or press `Shift+F9`

### Common Development Tasks

#### Adding a New Image Format Converter

1. **Create converter in Core**
   ```csharp
   // src/Pixolve.Core/Services/Converters/AvifConverter.cs
   public class AvifConverter : IImageConverter
   {
       public ImageFormat TargetFormat => ImageFormat.Avif;

       public async Task<ConversionResult> ConvertAsync(ImageFile file, ConversionSettings settings)
       {
           // Implementation
       }
   }
   ```

2. **Register in DI** (Program.cs)
   ```csharp
   services.AddTransient<IImageConverter, AvifConverter>();
   ```

3. **Add to ImageFormat enum**
   ```csharp
   public enum ImageFormat
   {
       WebP,
       Avif,  // New format
       Png,
       Jpeg
   }
   ```

4. **Update UI** (ConverterView.axaml)
   ```xml
   <ComboBox ItemsSource="{Binding AvailableFormats}">
     <!-- AVIF will appear automatically -->
   </ComboBox>
   ```

5. **Add tests**
   ```csharp
   [Fact]
   public async Task ConvertAsync_ToAvif_Succeeds()
   {
       // Arrange
       var converter = new AvifConverter();
       var file = CreateTestImage();

       // Act
       var result = await converter.ConvertAsync(file, settings);

       // Assert
       Assert.True(result.IsSuccess);
   }
   ```

#### Adding a New View

1. **Create View** (Views/NewView.axaml)
   ```xml
   <UserControl xmlns="https://github.com/avaloniaui"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:vm="using:Pixolve.Desktop.ViewModels"
                x:Class="Pixolve.Desktop.Views.NewView">
     <Design.DataContext>
       <vm:NewViewModel />
     </Design.DataContext>

     <!-- Content -->
   </UserControl>
   ```

2. **Create ViewModel**
   ```csharp
   public class NewViewModel : ViewModelBase
   {
       // Properties and commands
   }
   ```

3. **Wire up navigation**
   ```csharp
   // In MainWindowViewModel
   public void NavigateToNew()
   {
       CurrentView = _serviceProvider.GetService<NewViewModel>();
   }
   ```

#### Adding Dependencies

```bash
# Add to specific project
cd src/Pixolve.Core
dotnet add package NewPackage

# Or edit .csproj manually
<PackageReference Include="NewPackage" Version="1.0.0" />
```

## Testing

### Unit Testing Guidelines

**Good Unit Test:**
```csharp
[Fact]
public async Task ConvertAsync_WithValidJpeg_CreatesWebP()
{
    // Arrange
    var converter = new WebPConverter();
    var inputPath = "test.jpg";
    var expectedFormat = ImageFormat.WebP;

    // Act
    var result = await converter.ConvertAsync(inputPath);

    // Assert
    Assert.Equal(expectedFormat, result.Format);
    Assert.True(result.OutputSize > 0);
}
```

**Test Naming Convention:**
```
MethodName_Scenario_ExpectedBehavior
```

### Mocking

Use Moq for mocking dependencies:

```csharp
[Fact]
public async Task SaveAsync_CallsFileService()
{
    // Arrange
    var mockFileService = new Mock<IFileService>();
    var converter = new WebPConverter(mockFileService.Object);

    // Act
    await converter.SaveAsync("output.webp", data);

    // Assert
    mockFileService.Verify(x => x.WriteAllBytesAsync("output.webp", data), Times.Once);
}
```

## Building for Distribution

### Windows

```bash
# Self-contained executable
dotnet publish src/Pixolve.Desktop -c Release -r win-x64 --self-contained

# Output: src/Pixolve.Desktop/bin/Release/net9.0-windows/win-x64/publish/
```

### macOS

```bash
# Self-contained app bundle
dotnet publish src/Pixolve.Desktop -c Release -r osx-x64 --self-contained

# Create DMG (requires additional tools)
# See: https://github.com/create-dmg/create-dmg
```

### Linux

```bash
# Self-contained executable
dotnet publish src/Pixolve.Desktop -c Release -r linux-x64 --self-contained

# Create AppImage (requires appimagetool)
# See: https://appimage.org/
```

## Troubleshooting

### Common Issues

**Issue: "dotnet command not found"**
```bash
# Add to PATH (Windows)
setx PATH "%PATH%;C:\Program Files\dotnet"

# Add to PATH (Linux/Mac)
export PATH="$PATH:/usr/local/share/dotnet"
```

**Issue: "Avalonia templates not found"**
```bash
dotnet new install Avalonia.Templates
```

**Issue: Hot reload not working**
```bash
# Ensure you're using dotnet watch
dotnet watch --project src/Pixolve.Desktop run

# Clear obj/bin folders
dotnet clean
```

**Issue: Tests not discovered**
```bash
# Rebuild solution
dotnet build

# Clear test cache
dotnet clean
dotnet build
```

## Performance Tips

### Profiling

Use dotnet-trace for performance profiling:

```bash
# Install tool
dotnet tool install --global dotnet-trace

# Collect trace
dotnet-trace collect --process-id <PID>
```

### Memory Profiling

Use dotnet-counters for memory monitoring:

```bash
# Install tool
dotnet tool install --global dotnet-counters

# Monitor memory
dotnet-counters monitor --process-id <PID> System.Runtime
```

## Code Quality Tools

### StyleCop

Add StyleCop Analyzers:

```xml
<PackageReference Include="StyleCop.Analyzers" Version="1.2.0-beta.435">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

### SonarLint

Install SonarLint extension in your IDE for real-time code quality feedback.

## Resources

### Avalonia UI
- [Documentation](https://docs.avaloniaui.net/)
- [Samples](https://github.com/AvaloniaUI/Avalonia.Samples)
- [Community](https://github.com/AvaloniaUI/Avalonia/discussions)

### .NET
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# Language Reference](https://docs.microsoft.com/en-us/dotnet/csharp/)

### SkiaSharp
- [API Documentation](https://learn.microsoft.com/en-us/dotnet/api/skiasharp)
- [Samples](https://github.com/mono/SkiaSharp/tree/main/samples)

## Getting Help

- **GitHub Issues**: Report bugs or request features
- **GitHub Discussions**: Ask questions, share ideas
- **Stack Overflow**: Tag questions with `pixolve` and `avalonia`

## Next Steps

After setting up your development environment:

1. Read [ARCHITECTURE.md](ARCHITECTURE.md) to understand the system design
2. Check [CONTRIBUTING.md](../CONTRIBUTING.md) for contribution guidelines
3. Look at open issues labeled "good first issue"
4. Join the community discussions

Happy coding! 🚀
