# Pixolve Architecture Documentation

## Overview

Pixolve follows Clean Architecture principles with clear separation of concerns. The application is divided into layers that are independent and testable.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    Pixolve.Desktop                       │
│              (Avalonia UI + MVVM)                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │    Views     │  │  ViewModels  │  │   Converters │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└─────────────────────┬───────────────────────────────────┘
                      │
                      │ depends on
                      ▼
┌─────────────────────────────────────────────────────────┐
│                    Pixolve.Core                          │
│               (Business Logic)                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Models     │  │   Services   │  │  Interfaces  │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└─────────────────────────────────────────────────────────┘
                      ▲
                      │ tested by
                      │
┌─────────────────────┴───────────────────────────────────┐
│                   Pixolve.Tests                          │
│                  (Unit Tests)                            │
└─────────────────────────────────────────────────────────┘
```

## Project Structure

### Pixolve.Desktop

**Responsibility**: User interface and interaction logic

**Key Components**:

```
Pixolve.Desktop/
├── Views/
│   ├── MainWindow.axaml          # Main application window
│   ├── ConverterView.axaml       # Conversion interface
│   └── SettingsView.axaml        # Settings dialog
├── ViewModels/
│   ├── MainWindowViewModel.cs    # Main window logic
│   ├── ConverterViewModel.cs     # Conversion logic
│   ├── SettingsViewModel.cs      # Settings management
│   └── ViewModelBase.cs          # Base class for all VMs
├── Converters/
│   ├── FileSizeConverter.cs      # Format file sizes
│   └── BoolToVisibilityConverter.cs
├── Assets/
│   ├── Icons/                    # Application icons
│   └── Styles/                   # Custom styles
└── Program.cs                    # Entry point
```

**Design Patterns**:
- **MVVM (Model-View-ViewModel)**: Separates UI from logic
- **Dependency Injection**: Services injected into ViewModels
- **Command Pattern**: All user actions as commands
- **Observer Pattern**: Property change notifications

### Pixolve.Core

**Responsibility**: Platform-independent business logic

**Key Components**:

```
Pixolve.Core/
├── Models/
│   ├── ImageFile.cs              # Image file representation
│   ├── ConversionSettings.cs     # Conversion configuration
│   ├── ConversionResult.cs       # Conversion outcome
│   └── ImageFormat.cs            # Supported formats enum
├── Services/
│   ├── Converters/
│   │   ├── IImageConverter.cs    # Converter interface
│   │   ├── WebPConverter.cs      # WebP implementation
│   │   ├── AvifConverter.cs      # AVIF implementation
│   │   └── ConverterFactory.cs   # Factory for converters
│   ├── FileService/
│   │   ├── IFileService.cs       # File operations interface
│   │   └── FileService.cs        # File operations impl
│   └── SettingsService/
│       ├── ISettingsService.cs   # Settings interface
│       └── SettingsService.cs    # Settings persistence
├── Interfaces/
│   └── IImageProcessor.cs        # Image processing contract
└── Exceptions/
    ├── ConversionException.cs    # Conversion errors
    └── FileAccessException.cs    # File access errors
```

**Design Patterns**:
- **Repository Pattern**: File operations abstraction
- **Strategy Pattern**: Multiple converter implementations
- **Factory Pattern**: Creating appropriate converters
- **Dependency Inversion**: Depend on abstractions

### Pixolve.Tests

**Responsibility**: Automated testing

```
Pixolve.Tests/
├── Services/
│   ├── WebPConverterTests.cs
│   ├── FileServiceTests.cs
│   └── SettingsServiceTests.cs
├── Models/
│   └── ImageFileTests.cs
└── Fixtures/
    └── TestImageFixture.cs       # Test data setup
```

## Key Design Decisions

### 1. Avalonia UI over WPF

**Rationale**: Cross-platform support (Windows, macOS, Linux)

**Pros**:
- True cross-platform
- Modern XAML
- Active development
- Great performance

**Cons**:
- Smaller ecosystem than WPF
- Some learning curve

### 2. SkiaSharp for Image Processing

**Rationale**: Cross-platform, high-performance, actively maintained

**Alternatives Considered**:
- ImageSharp: Good, but SkiaSharp has better WebP support
- System.Drawing: Windows-only, legacy

### 3. MVVM with CommunityToolkit

**Rationale**: Standard pattern, excellent tooling

**Benefits**:
- Testable ViewModels
- Source generators
- Reduced boilerplate

### 4. No Database

**Rationale**: Simple JSON file for settings

**Benefits**:
- No migration complexity
- Easy to backup/share
- Portable

## Data Flow

### Image Conversion Flow

```
User Action
    ↓
View (XAML)
    ↓
Command Binding
    ↓
ViewModel
    ↓
Converter Service (Pixolve.Core)
    ↓
SkiaSharp Image Processing
    ↓
File Service (Save)
    ↓
Result → ViewModel
    ↓
UI Update
```

### Settings Persistence Flow

```
User Changes Setting
    ↓
ViewModel Property
    ↓
Settings Service
    ↓
JSON Serialization
    ↓
File System
    ↓
(On Startup)
    ↓
Settings Service Load
    ↓
Deserialize JSON
    ↓
Bind to ViewModels
```

## Dependency Injection

Using Microsoft.Extensions.DependencyInjection:

```csharp
services.AddSingleton<IFileService, FileService>();
services.AddSingleton<ISettingsService, SettingsService>();
services.AddTransient<IImageConverter, WebPConverter>();
services.AddTransient<MainWindowViewModel>();
```

**Lifetime Scopes**:
- **Singleton**: Services that maintain state (Settings, File)
- **Transient**: Converters, ViewModels (new instance each time)

## Error Handling Strategy

### Layers of Error Handling:

1. **Core Layer**: Throws specific exceptions
2. **Service Layer**: Catches and wraps with context
3. **ViewModel Layer**: Catches and converts to user messages
4. **View Layer**: Displays user-friendly messages

Example:

```csharp
// Core
public class WebPConverter
{
    public async Task<byte[]> ConvertAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Image not found: {path}");

        // Conversion logic
    }
}

// ViewModel
public class ConverterViewModel
{
    public async Task ConvertCommand()
    {
        try
        {
            await _converter.ConvertAsync(filePath);
        }
        catch (FileNotFoundException ex)
        {
            ErrorMessage = "Selected file could not be found.";
            _logger.LogError(ex, "File not found during conversion");
        }
        catch (Exception ex)
        {
            ErrorMessage = "An unexpected error occurred.";
            _logger.LogError(ex, "Unexpected error");
        }
    }
}
```

## Threading Model

### UI Thread
- All Avalonia UI operations
- Property change notifications
- Command execution start

### Background Threads
- Image conversion (CPU-intensive)
- File I/O operations
- Batch processing

### Synchronization
```csharp
// Use Dispatcher for UI updates from background
await Dispatcher.UIThread.InvokeAsync(() =>
{
    Progress = 50;
});
```

## Performance Considerations

### Image Loading
- Load thumbnails on-demand
- Use async loading
- Implement caching

### Batch Operations
- Process in parallel (configurable degree)
- Show progress for each item
- Allow cancellation

### Memory Management
- Dispose SKBitmap after use
- Limit concurrent operations
- Use weak references for caches

## Security Considerations

### File Access
- Validate file paths
- Check file extensions
- Limit file sizes
- Sandboxed file access

### User Settings
- Validate loaded settings
- Use safe defaults
- Don't store sensitive data

## Testing Strategy

### Unit Tests
- Test Core services in isolation
- Mock file system operations
- Test error scenarios

### Integration Tests
- Test complete conversion workflows
- Test with real image files
- Test file operations

### UI Tests (Future)
- Automated UI testing with Avalonia UI Testing
- Screenshot comparisons
- Accessibility testing

## Extensibility Points

### Adding New Image Formats

1. Implement `IImageConverter`
2. Register in DI container
3. Add to `ImageFormat` enum
4. Update UI format selector

### Adding New Features

1. Create service in Core
2. Create interface
3. Inject into ViewModel
4. Add UI in View
5. Wire up commands

## Future Improvements

### Planned
- [ ] Plugin system for custom converters
- [ ] Batch operation profiles
- [ ] Command-line interface
- [ ] Web API for headless operation

### Under Consideration
- [ ] Cloud storage integration
- [ ] Image comparison tools
- [ ] Metadata preservation options
- [ ] Scripting support

## References

- [Avalonia UI Documentation](https://docs.avaloniaui.net/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MVVM Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
