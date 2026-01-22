# Changelog

All notable changes to Pixolve will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.1] - 2026-01-22

### Added
- **macOS Code Signing**: App is now signed with Apple Developer ID certificate
- **macOS Notarization**: App is notarized by Apple for seamless installation without Gatekeeper warnings
- **Hardened Runtime**: Enhanced security with proper entitlements for .NET runtime

### Changed
- macOS users no longer need to right-click → "Open" on first launch
- Improved installation experience on macOS

## [1.3.0] - 2025-12-06

### Added
- **High-Quality Thumbnail Preview**: Hover over thumbnails to see a 300x300px preview with sharp, non-pixelated images
- **Clickable Output Folder Link**: Status bar now shows a clickable folder path that opens the output directory directly
- **Color-Coded Status Display**: Image conversion status is now color-coded (green for success, red for errors, orange for in-progress, gray for waiting)
- **Select All/Deselect All**: Quick buttons to select or deselect all images for batch operations

### Changed
- Increased thumbnail generation quality from 50px to 300px for sharper preview tooltips
- Improved thumbnail encoding quality from 80 to 90 for better image clarity
- Enhanced thumbnail resize filter from Medium to High quality

### Technical
- Thumbnail size: 50px → 300px
- PNG quality: 80 → 90
- SKFilterQuality: Medium → High
- Added StatusToColorConverter for color-coded status display
- Added ShowOpenFolderButton and LastOutputDirectory properties to ViewModel

## [1.2.0] - 2025-12-05

### Added
- **RAW Camera Format Support**: Convert professional camera RAW files to standard formats
  - Nikon (`.nef`), Canon (`.cr2`, `.cr3`), Sony (`.arw`), Adobe DNG (`.dng`)
  - Fujifilm (`.raf`), Olympus (`.orf`), Panasonic (`.rw2`), Pentax (`.pef`)
  - Samsung (`.srw`), Epson (`.erf`), and more
- **Version Display**: App version now visible in the UI status bar
- **Professional macOS Installer**: New `install-macos.sh` script with beautiful UI
  - Automatic version checking and upgrade prompts
  - Backup/restore functionality for safe updates
  - Enhanced error handling with rollback capability
- **Developer Update Script**: `update-local.sh` for seamless local development updates

### Changed
- Enhanced file scanner to detect RAW format extensions
- Improved error messages for RAW conversion failures
- Updated macOS bundle version to 1.2.0

### Technical
- Added ImageMagick.NET (Magick.NET-Q16-AnyCPU v14.9.1) for RAW format decoding
- Implemented two-stage RAW conversion pipeline (ImageMagick → SkiaSharp)
- Automatic EXIF-based orientation correction for RAW files
- Extended ImageFormat enum with RAW format types
- Implemented `DecodeRawImageAsync()` in UniversalConverter

## [1.1.0] - 2025-12-04

### Added
- **Multilingual Support**: German and English language options
- **Quick Preset Buttons**: Web-optimized, Maximum Quality, Minimum Size presets
- **Keyboard Shortcuts**:
  - `Ctrl+O`: Browse folder
  - `Ctrl+L`: Load images
  - `Ctrl+Enter`: Convert all
  - `Esc`: Cancel conversion
- **Drag & Drop Support**: Drop images or folders directly into the application
- **Compression Statistics**: Real-time compression ratio and size savings display
- **Estimated Time Remaining**: Shows approximate time left during batch conversion
- **Custom Per-Image Settings**: Configure quality, size, and format for individual images
- **Multi-Format Export**: Export to multiple formats simultaneously
- **Theme Support**: Light and Dark theme with System preference option

### Changed
- Completely redesigned modern UI with responsive grid layout
- Enhanced DataGrid with color-coded status, thumbnails, and compression info
- Improved error handling with detailed error messages
- Better progress indication during loading and conversion

### Fixed
- Fixed file loading performance issues
- Resolved thumbnail generation crashes
- Fixed theme switching bugs

## [1.0.0] - 2025-12-03

### Added
- Initial release of Pixolve
- Cross-platform image converter (macOS, Windows, Linux)
- Support for modern image formats:
  - WebP (input & output)
  - AVIF (output)
  - PNG (input & output)
  - JPEG (input & output)
  - BMP, GIF (input)
- Batch conversion with folder scanning
- Quality control (0-100)
- Image resizing with max pixel size option
- Output directory selection
- Subfolder scanning support
- Timestamp preservation option
- Overwrite protection
- Conversion progress tracking
- Status bar with real-time updates
- Built with Avalonia UI 11.3.8 and .NET 9.0

### Technical
- SkiaSharp for high-performance image processing
- MVVM architecture with CommunityToolkit.Mvvm
- Dependency injection with Microsoft.Extensions.DependencyInjection
- Cross-platform folder/file dialogs
- Responsive UI design

---

## Release Notes Format

- **Added**: New features
- **Changed**: Changes in existing functionality
- **Deprecated**: Soon-to-be removed features
- **Removed**: Removed features
- **Fixed**: Bug fixes
- **Security**: Security vulnerability fixes
- **Technical**: Internal technical changes

[1.3.1]: https://github.com/AndreasKalkusinski/Pixolve/releases/tag/v1.3.1
[1.3.0]: https://github.com/AndreasKalkusinski/Pixolve/releases/tag/v1.3.0
[1.2.0]: https://github.com/AndreasKalkusinski/Pixolve/releases/tag/v1.2.0
[1.1.0]: https://github.com/AndreasKalkusinski/Pixolve/releases/tag/v1.1.0
[1.0.0]: https://github.com/AndreasKalkusinski/Pixolve/releases/tag/v1.0.0
