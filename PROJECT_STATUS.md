# Pixolve - Project Status & Context

**Last Updated**: 2025-12-06
**Version**: 1.3.1
**Status**: Production Release ✅

## Quick Context

**What is Pixolve?**
A modern, cross-platform image converter built with Avalonia UI and .NET 9. Converts images to modern formats (WebP, AVIF) with batch processing, quality control, and an intuitive interface.

**Why this project?**
- Migrated from legacy WPF WebP Converter
- Need for cross-platform support (Windows, macOS, Linux)
- Modern, maintainable architecture
- Open source contribution opportunity

## Project Details

**Name**: Pixolve (Pixel + Evolve)
**License**: MIT
**Language**: C# (.NET 9)
**UI Framework**: Avalonia UI 11.3
**Architecture**: Clean Architecture with MVVM

## Current Status

### ✅ Completed

1. **Project Structure**
   - Created solution at `C:\repo\Pixolve\`
   - Three projects setup:
     - `Pixolve.Desktop` - Avalonia UI application
     - `Pixolve.Core` - Business logic library
     - `Pixolve.Tests` - xUnit test project
   - All projects added to solution
   - References configured (Desktop → Core, Tests → Core)

2. **Documentation**
   - `README.md` - Project overview, features, installation
   - `CONTRIBUTING.md` - Contribution guidelines
   - `LICENSE` - MIT License
   - `docs/ARCHITECTURE.md` - System design & architecture
   - `docs/DEVELOPMENT.md` - Development guide
   - `.gitignore` - Git ignore rules

3. **Templates Installed**
   - Avalonia templates v11.3.8
   - Ready for development

4. **Core Services** (Pixolve.Core) ✅
   - `IImageConverter` interface
   - `WebPConverter` using SkiaSharp with resizing
   - `ImageFile` model with INotifyPropertyChanged
   - `ConversionSettings` model with validation
   - `ConversionResult` model with Result pattern
   - `ImageFormat` enum
   - All 26 unit tests passing

5. **Desktop UI** (Pixolve.Desktop) ✅
   - Dependency injection configured (App.axaml.cs)
   - `MainWindowViewModel` with MVVM Toolkit attributes
   - Complete `MainWindow.axaml` UI design
   - Source folder selection
   - Conversion settings panel (Quality, MaxPixelSize, options)
   - DataGrid for image list with compression stats
   - Progress tracking with ProgressBar
   - Action buttons (Convert, Cancel, Clear)
   - Status bar with live updates
   - Build successful (0 warnings, 0 errors)

6. **Enhanced Features** ✅
   - Cross-platform folder browser dialog (StorageProvider API)
   - Automatic image dimensions reading on load
   - File size formatting (B, KB, MB, GB)
   - Formatted display in DataGrid
   - Real-time property change notifications

### 🚧 In Progress

- Testing with real images (ready for user testing)

### 📋 TODO (Priority Order)

#### Phase 1: Core Functionality
1. **Core Services** (Pixolve.Core)
   - [x] Create `IImageConverter` interface
   - [x] Implement `WebPConverter` using SkiaSharp
   - [x] Create `ImageFile` model
   - [x] Create `ConversionSettings` model
   - [ ] Implement `FileService` for file operations (optional)
   - [ ] Implement `SettingsService` for persistence (optional)

2. **Desktop UI** (Pixolve.Desktop)
   - [x] Setup dependency injection
   - [x] Create `MainWindowViewModel`
   - [x] Design `MainWindow` UI
   - [ ] Implement file selection (folder browser dialog)
   - [ ] Implement drag & drop
   - [x] Create conversion settings panel
   - [x] Implement progress tracking
   - [x] Add result display (DataGrid with images)

3. **Testing**
   - [x] Unit tests for WebPConverter
   - [x] Unit tests for ConversionSettings
   - [ ] Unit tests for FileService (N/A - not implemented)
   - [ ] Unit tests for SettingsService (N/A - not implemented)
   - [ ] Integration tests for conversion workflow

#### Phase 2: Enhanced Features
- [ ] Add AVIF format support
- [ ] Add batch operations
- [ ] Implement preview (before/after)
- [ ] Add quality presets
- [ ] Settings persistence (JSON)
- [ ] Error handling & user feedback

#### Phase 3: Polish
- [ ] Localization (i18n)
- [ ] Themes (Light/Dark mode)
- [ ] Performance optimization
- [ ] Memory management improvements
- [ ] Accessibility features

#### Phase 4: Distribution
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Windows installer (MSI)
- [ ] macOS bundle (.app/.dmg)
- [ ] Linux packages (AppImage, deb, rpm)
- [ ] Auto-update mechanism

## Migration Status from Old Project

### What to Migrate

**From**: `C:\repo\WebP-Converter-WPF\WpfApp1\`

**Key Files to Reference**:
1. `ViewModel/ToWebP02ViewModel.cs` - Main logic
   - File loading logic
   - Conversion logic (`ConvertAllImagesTask()` method)
   - Settings management

2. `Model/ImageFile.cs` - Data model
   - Image file representation
   - Properties to preserve

3. `ImageConverter/ConvertToWebP_SkiaSharp.cs` - Converter
   - SkiaSharp conversion implementation
   - Quality settings

**What NOT to Migrate**:
- ❌ Imazen.WebP references (incomplete, removed)
- ❌ WPF-specific code (MessageBox, etc.)
- ❌ MahApps.Metro UI code
- ❌ Old view files (ToWebpView.xaml)
- ❌ Prism-specific implementations

### Key Code to Preserve

**Conversion Logic** (from ToWebP02ViewModel.cs lines 395-512):
```csharp
// SkiaSharp-based conversion
using var skBitmap = SKBitmap.Decode(fullFilePath);
using var image = SKImage.FromBitmap(skBitmap);
using var data = image.Encode(SKEncodedImageFormat.Webp, (int)ImageQualityPercentage);
```

**File Organization**:
- Output folder: `{sourcePath}\konvertiert\`
- Preserve original files
- Replace existing converted files

## Technical Decisions

### Why Avalonia over WPF?
- Cross-platform support (Windows, macOS, Linux)
- Modern XAML engine
- Active development & community
- Similar to WPF (easy migration)

### Why SkiaSharp?
- Cross-platform 2D graphics
- Excellent WebP support
- High performance
- Actively maintained

### Why Clean Architecture?
- Testability (business logic independent)
- Maintainability (clear separation)
- Flexibility (swap UI/implementations)
- Scalability (easy to extend)

### Why MIT License?
- Maximum freedom for users
- Commercial use allowed
- Simple & permissive
- Industry standard

## Development Environment

**IDE**: Visual Studio Code with C# Dev Kit
**OS**: Windows 10/11
**.NET SDK**: 9.0.100
**.NET Runtime**: Not required (self-contained deployment)

### Key Commands

```bash
# Navigate to project
cd C:\repo\Pixolve

# Build
dotnet build

# Run
dotnet run --project src/Pixolve.Desktop

# Test
dotnet test

# Watch (hot reload)
dotnet watch --project src/Pixolve.Desktop run
```

## Dependencies

### Pixolve.Desktop
- Avalonia 11.3.8
- Avalonia.Themes.Fluent 11.3.8
- Avalonia.Fonts.Inter 11.3.8
- CommunityToolkit.Mvvm 8.2.2 (planned)
- Microsoft.Extensions.DependencyInjection 9.0.0 (planned)

### Pixolve.Core
- SkiaSharp 2.88.8
- System.Text.Json 9.0.0 (for settings)

### Pixolve.Tests
- xUnit 2.9.2
- FluentAssertions 7.0.0 (planned)
- Moq 4.20.70 (planned)

## Known Issues

**Current**:
- None (project just created)

**Expected Challenges**:
1. Cross-platform file dialogs
2. Thumbnail generation performance
3. Large batch processing memory usage
4. macOS notarization for distribution

## Next Steps for Developer

### Immediate (Next Session)
1. Add SkiaSharp package to Pixolve.Core
2. Create IImageConverter interface
3. Implement WebPConverter class
4. Create basic models (ImageFile, ConversionSettings)
5. Test conversion with sample images

### Short Term (This Week)
1. Implement FileService
2. Create MainWindowViewModel
3. Design basic UI in MainWindow.axaml
4. Wire up file selection
5. Test end-to-end conversion

### Medium Term (This Month)
1. Add all planned features (Phase 1 & 2)
2. Comprehensive testing
3. Polish UI/UX
4. Prepare for alpha release

## Resources & References

**Documentation**:
- Avalonia UI Docs: https://docs.avaloniaui.net/
- SkiaSharp API: https://learn.microsoft.com/en-us/dotnet/api/skiasharp
- Clean Architecture: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

**Similar Projects** (for inspiration):
- ImageGlass: https://github.com/d2phap/ImageGlass
- Squoosh: https://github.com/GoogleChromeLabs/squoosh
- XnConvert: https://www.xnview.com/en/xnconvert/

**Original Project**:
- Location: `C:\repo\WebP-Converter-WPF\`
- Status: Legacy, .NET 9.0 upgrade complete but staying with WPF

## Contact & Collaboration

**Developer**: QonCierge
**Repository**: (To be created on GitHub)
**Collaboration**: Open to contributions (see CONTRIBUTING.md)

## Version History

### v0.1.0-alpha (Current)
- Initial project structure
- Documentation complete
- Ready for development

---

**Note**: This document should be updated regularly as the project progresses. It serves as a quick reference for anyone (including future you) who needs to understand the project context quickly.

**Last worked on**: Implemented complete UI layer with MainWindowViewModel and MainWindow.axaml
**Next task**:
1. Implement folder browser dialog for cross-platform file selection
2. Test end-to-end conversion workflow with real images
3. Implement drag & drop functionality (optional enhancement)

## Implementation Summary

### What Works Now
- Complete core conversion logic (WebP format with SkiaSharp)
- Quality and size settings with validation
- Manual path entry and image loading
- Batch conversion with progress tracking
- Result display with compression statistics
- Cancel operation support

### What Needs Testing
- Run the application: `dotnet run --project src/Pixolve.Desktop`
- Manually enter a folder path with images
- Click "Load Images" to populate the list
- Adjust Quality and MaxPixelSize settings
- Click "Convert All" to convert images
- Check the "converted" subfolder for output

### Known Limitations
- Folder browser dialog not implemented yet (Browse button shows placeholder message)
- Drag & drop not implemented yet
- No settings persistence (resets on restart)
- No thumbnail preview in DataGrid
