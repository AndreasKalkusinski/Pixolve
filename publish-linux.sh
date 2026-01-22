#!/bin/bash

# Pixolve Linux Build Script
# Creates a self-contained Linux x64 build

set -e

VERSION="1.3.1"
APP_NAME="Pixolve"
PROJECT_PATH="src/Pixolve.Desktop/Pixolve.Desktop.csproj"
OUTPUT_DIR="publish/linux-x64"
ZIP_FILE="publish/${APP_NAME}-v${VERSION}-linux-x64.zip"

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║           Pixolve v${VERSION} - Linux Build                    ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Clean previous build
if [ -d "$OUTPUT_DIR" ]; then
    echo "🧹 Cleaning previous build..."
    rm -rf "$OUTPUT_DIR"
fi

# Build self-contained Linux x64 version
echo "🔨 Building Linux x64 release..."
echo ""

dotnet publish "$PROJECT_PATH" \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=false \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:EnableCompressionInSingleFile=true \
    -o "$OUTPUT_DIR"

echo ""
echo "📦 Creating ZIP archive..."

# Remove old ZIP if exists
rm -f "$ZIP_FILE"

# Create ZIP archive
cd publish
zip -r "$(basename "$ZIP_FILE")" linux-x64/
cd ..

# Get file size
ZIP_SIZE=$(du -h "$ZIP_FILE" | cut -f1)

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║                   Build Successful! ✨                     ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""
echo "📦 Created: ./$ZIP_FILE"
echo "📊 Size:    $ZIP_SIZE"
echo ""
echo "📝 Distribution:"
echo "   1. Upload ${APP_NAME}-v${VERSION}-linux-x64.zip to GitHub Releases"
echo "   2. Users extract and run: chmod +x Pixolve.Desktop && ./Pixolve.Desktop"
echo ""
echo "🐧 Tested on: Ubuntu 22.04+, Debian 11+, Fedora 36+"
echo ""
