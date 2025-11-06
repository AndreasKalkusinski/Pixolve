# Pixolve

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey.svg)

**Pixolve** is a modern, cross-platform image conversion tool built with Avalonia UI and .NET 9. Convert your images to modern formats like WebP, AVIF, and more with an intuitive interface.

## Features

- 🖼️ **Multi-Format Support**: WebP, AVIF, PNG, JPEG, and more
- 🚀 **Batch Processing**: Convert multiple images at once
- ⚡ **Fast & Efficient**: Powered by SkiaSharp for high-performance image processing
- 🎨 **Modern UI**: Clean, responsive interface built with Avalonia UI
- 🔧 **Advanced Options**: Quality control, resizing, and optimization settings
- 📁 **Smart Organization**: Automatically organizes converted files
- 🎯 **Drag & Drop**: Intuitive file handling
- 💾 **Settings Persistence**: Remembers your preferences
- 🌍 **Cross-Platform**: Works on Windows, macOS, and Linux

## Screenshots

_Coming soon_

## Installation

### Windows

```bash
# Download the installer
# Coming soon: MSI/EXE installer

# Or use portable version
# Extract and run Pixolve.exe
```

### macOS

```bash
# Download the .dmg file
# Coming soon

# Or via Homebrew (planned)
brew install --cask pixolve
```

### Linux

```bash
# AppImage (portable)
chmod +x Pixolve.AppImage
./Pixolve.AppImage

# Or via package managers (planned)
# Debian/Ubuntu: apt install pixolve
# Fedora: dnf install pixolve
# Flatpak: flatpak install pixolve
```

## Quick Start

1. **Select Images**: Drag & drop images or click "Browse" to select files
2. **Choose Format**: Select your desired output format (WebP, AVIF, etc.)
3. **Adjust Settings**: Configure quality, size, and other options
4. **Convert**: Click "Convert" and your images will be processed

## Building from Source

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

### Clone and Build

```bash
# Clone the repository
git clone https://github.com/yourusername/pixolve.git
cd pixolve

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run --project src/Pixolve.Desktop
```

### Run Tests

```bash
dotnet test
```

## Project Structure

```
Pixolve/
├── src/
│   ├── Pixolve.Desktop/      # Avalonia UI application
│   │   ├── Views/             # UI views
│   │   ├── ViewModels/        # View models (MVVM)
│   │   ├── Assets/            # Images, fonts, resources
│   │   └── Program.cs         # Entry point
│   └── Pixolve.Core/          # Core business logic
│       ├── Models/            # Data models
│       ├── Services/          # Business services
│       │   ├── Converters/    # Image conversion logic
│       │   └── FileService/   # File operations
│       └── Interfaces/        # Service contracts
├── tests/
│   └── Pixolve.Tests/         # Unit tests
├── docs/                      # Documentation
├── .github/                   # CI/CD workflows
└── README.md
```

## Architecture

Pixolve follows clean architecture principles:

- **Pixolve.Desktop**: UI layer (Avalonia MVVM)
- **Pixolve.Core**: Business logic layer (platform-independent)
- **Pixolve.Tests**: Test layer (xUnit)

### Key Technologies

- **UI Framework**: [Avalonia UI](https://avaloniaui.net/) 11.3+
- **Image Processing**: [SkiaSharp](https://github.com/mono/SkiaSharp) 2.88+
- **MVVM Framework**: [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- **Testing**: xUnit, FluentAssertions

## Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details.

### Development Setup

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## Roadmap

### Phase 1: MVP (Current)
- [x] Project structure setup
- [ ] Basic WebP conversion
- [ ] Modern UI implementation
- [ ] Drag & drop support
- [ ] Settings persistence

### Phase 2: Enhanced Features
- [ ] AVIF support
- [ ] HEIF support
- [ ] JPEG XL support
- [ ] Batch operations
- [ ] Preview mode (before/after)

### Phase 3: Advanced
- [ ] Watermark support
- [ ] Resize presets
- [ ] Undo/Redo
- [ ] Profile/Template system
- [ ] CLI interface

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [Avalonia UI](https://avaloniaui.net/)
- Image processing powered by [SkiaSharp](https://github.com/mono/SkiaSharp)
- Inspired by the need for a modern, cross-platform image converter

## Support

- 📖 [Documentation](docs/)
- 🐛 [Issue Tracker](https://github.com/yourusername/pixolve/issues)
- 💬 [Discussions](https://github.com/yourusername/pixolve/discussions)

## Author

Created with ❤️ by QonCierge

---

**Star ⭐ this repository if you find it helpful!**
