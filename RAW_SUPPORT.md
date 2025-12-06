# RAW Format Support

Pixolve v1.2.0 adds support for converting RAW camera formats to standard image formats!

## ✨ Supported RAW Formats

### Nikon
- `.nef` - Nikon Electronic Format

### Canon
- `.cr2` - Canon Raw 2
- `.cr3` - Canon Raw 3 (newer models)

### Sony
- `.arw` - Sony Alpha Raw

### Adobe
- `.dng` - Digital Negative (universal RAW format)

### Other Manufacturers
- `.raw` - Generic RAW
- `.raf` - Fujifilm RAW
- `.orf` - Olympus RAW
- `.rw2` - Panasonic RAW
- `.pef` - Pentax Electronic File
- `.srw` - Samsung RAW
- `.erf` - Epson RAW

## 🔄 Conversion Options

RAW files can be converted to:
- **JPEG** - Universal compatibility
- **PNG** - Lossless quality
- **WebP** - Modern, efficient format
- **AVIF** - Next-gen format (best compression)

## 🚀 How to Use

1. **Load RAW Files**:
   - Click "Browse" and select a folder containing RAW images
   - Or drag & drop RAW files directly into Pixolve
   - Click "Load Images"

2. **Configure Settings**:
   - **Format**: Choose output format (JPEG, PNG, WebP, AVIF)
   - **Quality**: Adjust quality slider (1-100)
   - **Max Pixel**: Optional resizing (e.g., 1920 for Full HD)
   - **Output Directory**: Where to save converted files

3. **Convert**:
   - Click "Convert All" or convert individual images
   - RAW files are automatically decoded and processed

## 🔧 Technical Details

### How It Works

Pixolve uses a two-step conversion process for RAW files:

1. **ImageMagick.NET** decodes the RAW format
   - Reads camera-specific RAW data
   - Applies basic demosaicing
   - Auto-corrects orientation (EXIF)

2. **SkiaSharp** encodes to final format
   - High-performance encoding
   - Quality control
   - Optional resizing

### Advantages

- ✅ **No RAW processing software needed**
- ✅ **Batch processing** - Convert hundreds of RAW files at once
- ✅ **Automatic orientation** based on EXIF data
- ✅ **Modern formats** - Export to WebP, AVIF
- ✅ **Cross-platform** - Works on Windows, macOS, Linux

### Limitations

- **Basic Processing**: Pixolve applies basic RAW demosaicing. For advanced RAW editing (white balance, exposure, etc.), use dedicated RAW editors like Lightroom or Darktable, then use Pixolve for format conversion.
- **Performance**: RAW decoding is slower than standard image formats due to the complexity of RAW files.
- **Color Profiles**: Uses default embedded profiles. Advanced color management not yet supported.

## 📊 Performance

Typical conversion times on an M1 Mac:

| Source      | Resolution | Output  | Time    |
|-------------|-----------|---------|---------|
| NEF (Nikon) | 24MP      | JPEG    | ~2-3s   |
| CR2 (Canon) | 20MP      | WebP    | ~2-3s   |
| ARW (Sony)  | 42MP      | PNG     | ~4-5s   |
| DNG (Adobe) | 24MP      | AVIF    | ~3-4s   |

*Times include decoding + processing + encoding*

## 🎯 Use Cases

### Professional Photography
- Convert RAW archives to web-friendly formats
- Create client previews from RAW files
- Batch export for social media

### Photo Organization
- Convert old RAW files to modern formats
- Create compressed backups (WebP, AVIF)
- Generate thumbnails and previews

### Web Development
- Prepare images for websites
- Optimize RAW photos for web delivery
- Batch process product photography

## ⚠️ Important Notes

### File Size
- RAW files are typically **20-50MB** each
- Converted files are much smaller:
  - JPEG: ~2-5MB (quality 90)
  - WebP: ~1-3MB (quality 90)
  - AVIF: ~0.5-2MB (quality 90)

### Quality Settings
- **80-90**: Excellent quality, web-ready
- **90-95**: Very high quality, minimal compression
- **95-100**: Near-lossless (larger files)

### Backup Reminder
Always keep your original RAW files! Pixolve creates copies in the output directory and never modifies source files.

## 🆕 What's New in v1.2.0

- ✨ RAW format support via ImageMagick.NET
- 🔄 Automatic orientation correction
- 📦 Support for 15+ camera manufacturers
- 🚀 Optimized batch processing for RAW files
- 📝 Enhanced file scanner for RAW detection

## 🐛 Troubleshooting

### "Failed to decode RAW file"
- Ensure the file is not corrupted
- Try converting with another RAW processor first
- Some proprietary RAW formats may not be fully supported

### Slow Conversion
- RAW files are large and complex - this is normal
- Consider reducing Max Pixel size for faster processing
- Close other applications to free up RAM

### Color Looks Different
- Pixolve uses basic RAW processing
- For color-critical work, pre-process in Lightroom/Capture One
- Then use Pixolve for format conversion

## 📚 Further Reading

- [ImageMagick RAW Formats](https://imagemagick.org/script/formats.php)
- [Digital Negative (DNG) Specification](https://helpx.adobe.com/camera-raw/digital-negative.html)
- [RAW Image Format - Wikipedia](https://en.wikipedia.org/wiki/Raw_image_format)

---

**Questions or Issues?**
Open an issue on [GitHub](https://github.com/AndreasKalkusinski/Pixolve/issues)
