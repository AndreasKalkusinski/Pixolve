# Pixolve v1.1.0 - Release Notes

## 🎉 What's New

### macOS Native Support
- ✅ **Native .app Bundle**: Professional macOS application bundle with proper icon and metadata
- ✅ **DMG Installer**: Easy drag-and-drop installer for seamless installation
- ✅ **Ad-hoc Code Signing**: Reduced security warnings on macOS
- ✅ **Automated Build Scripts**: One-command publishing for macOS

### Dark Mode & Theming
- 🌓 Full dark mode support with system integration
- 🎨 Light/Dark/System theme options
- 💾 Theme preferences saved between sessions

### Multilingual Support
- 🌍 English and German language support
- 🔄 Easy language switching in settings
- 💾 Language preferences saved between sessions

### Multi-Format Export
- 🔄 Export single images to multiple formats simultaneously
- ⚙️ Per-format quality settings
- 📊 Batch export support

### UI Improvements
- 📐 Enhanced layout and responsiveness
- 🎯 Better drag & drop experience
- 👁️ Improved thumbnail previews
- 📊 Real-time statistics and progress tracking

## 📦 Downloads

### macOS

**Recommended for most users:**
- **Pixolve-v1.1.0-macOS.dmg** (161 MB)
  - Double-click to mount
  - Drag to Applications folder
  - Ready to use!

**Alternative:**
- **Pixolve-v1.1.0-macOS.zip** (40 MB)
  - Extract and drag to Applications
  - Same functionality as DMG

**Architecture:** Universal build for Apple Silicon (M1/M2/M3) Macs

### Windows
- **Pixolve-v1.1.0-win-x64.zip**
  - Extract and run `Pixolve.Desktop.exe`

### Linux
- **Pixolve-v1.1.0-linux-x64.zip**
  - Extract, make executable, and run

## 🔧 Installation

### macOS Installation (Detailed)

1. **Download** the DMG or ZIP file
2. **For DMG:**
   - Double-click `Pixolve-v1.1.0-macOS.dmg`
   - Drag `Pixolve` to the `Applications` folder
   - Eject the disk image
3. **For ZIP:**
   - Extract `Pixolve-v1.1.0-macOS.zip`
   - Drag `Pixolve.app` to `/Applications`
4. **First Launch - Fix Security Warning:**

   If macOS says **"cannot verify"** or **"kann nicht überprüft werden"**, open Terminal and run:
   ```bash
   cd /Applications
   xattr -cr Pixolve.app
   open Pixolve.app
   ```

   **Alternative methods:**
   - System Settings → Privacy & Security → Click "Open Anyway"
   - OR: Right-click app → "Open" → Confirm

   > **Why?** The app is ad-hoc signed but not Apple-notarized (would require 99$/year Developer Account).

   **Detailed troubleshooting:** See [INSTALLATION_MACOS.md](INSTALLATION_MACOS.md)

### Windows Installation
1. Extract the ZIP file
2. Run `Pixolve.Desktop.exe`
3. No installation required!

### Linux Installation
1. Extract the ZIP file
2. Make executable: `chmod +x Pixolve.Desktop`
3. Run: `./Pixolve.Desktop`

## ✨ Key Features

- 🖼️ **Multi-Format Support**: WebP, AVIF, PNG, JPEG
- 🔄 **Batch Processing**: Convert hundreds of images at once
- ⚡ **High Performance**: Powered by SkiaSharp
- 🎨 **Modern UI**: Clean Fluent Design with dark mode
- 🌍 **Cross-Platform**: Windows, macOS, Linux
- 💾 **Settings Persistence**: All preferences saved
- 📏 **Smart Resizing**: Automatic dimension handling
- 🔧 **Per-Image Settings**: Customize each conversion

## 🐛 Bug Fixes

- Fixed macOS security warnings through proper code signing
- Improved app bundle structure for better macOS integration
- Enhanced icon rendering on all platforms
- Better error handling for unsupported formats

## 🚀 Performance Improvements

- Optimized build size with proper bundling
- Faster startup times
- Reduced memory footprint
- Improved image processing pipeline

## 📝 Technical Details

- **.NET Version:** 9.0
- **UI Framework:** Avalonia 11.3+
- **Image Processing:** SkiaSharp 2.88+
- **Minimum macOS:** 10.15 (Catalina)
- **Minimum Windows:** Windows 10
- **Linux:** Any modern distribution with glibc 2.17+

## 🔜 Coming Soon (v1.2)

- [ ] Compression statistics and reporting
- [ ] Image preview with before/after comparison
- [ ] Undo/Redo functionality
- [ ] More language support
- [ ] Performance optimizations

## 💬 Support

- 🐛 [Report Issues](https://github.com/AndreasKalkusinski/Pixolve/issues)
- 💡 [Feature Requests](https://github.com/AndreasKalkusinski/Pixolve/issues/new?labels=enhancement)
- 📖 [Documentation](https://github.com/AndreasKalkusinski/Pixolve)

## 🙏 Acknowledgments

Built with:
- [Avalonia UI](https://avaloniaui.net/) - Cross-platform XAML framework
- [SkiaSharp](https://github.com/mono/SkiaSharp) - High-performance 2D graphics
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) - MVVM helpers

---

**Full Changelog**: [v1.0.0...v1.1.0](https://github.com/AndreasKalkusinski/Pixolve/compare/v1.0.0...v1.1.0)

Made with ❤️ and .NET 9
