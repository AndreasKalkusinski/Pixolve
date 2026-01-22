#!/bin/bash

# Create DMG installer for Pixolve
# This creates a nice drag-and-drop installer image

set -e

VERSION="1.3.1"
APP_NAME="Pixolve"
APP_BUNDLE="./publish/${APP_NAME}.app"
DMG_NAME="Pixolve-v${VERSION}-macOS.dmg"
VOLUME_NAME="${APP_NAME} ${VERSION}"
DMG_TEMP="./publish/dmg-temp"

echo "💿 Creating DMG installer for ${APP_NAME} v${VERSION}..."

# Check if app exists
if [ ! -d "${APP_BUNDLE}" ]; then
    echo "❌ Error: ${APP_BUNDLE} not found!"
    echo "Run ./publish-macos.sh first"
    exit 1
fi

# Clean up
rm -rf "${DMG_TEMP}"
rm -f "./publish/${DMG_NAME}"

# Create temporary DMG directory
echo "📁 Creating temporary directory structure..."
mkdir -p "${DMG_TEMP}"

# Copy app to temp directory
cp -r "${APP_BUNDLE}" "${DMG_TEMP}/"

# Copy installation instructions
if [ -f "./publish/READ_ME_FIRST.txt" ]; then
    cp "./publish/READ_ME_FIRST.txt" "${DMG_TEMP}/"
fi

# Create symlink to Applications folder
echo "🔗 Creating Applications symlink..."
ln -s /Applications "${DMG_TEMP}/Applications"

# Calculate size needed (app size + 50MB buffer)
APP_SIZE=$(du -sm "${APP_BUNDLE}" | cut -f1)
DMG_SIZE=$((APP_SIZE + 50))

echo "📦 Creating DMG (${DMG_SIZE}MB)..."

# Create DMG
hdiutil create -volname "${VOLUME_NAME}" \
    -srcfolder "${DMG_TEMP}" \
    -ov -format UDZO \
    -size ${DMG_SIZE}m \
    "./publish/${DMG_NAME}"

# Clean up temp directory
echo "🧹 Cleaning up..."
rm -rf "${DMG_TEMP}"

echo "✅ Done!"
echo ""
echo "📦 Created: ./publish/${DMG_NAME}"
echo ""
echo "📝 Users can now:"
echo "   1. Double-click ${DMG_NAME}"
echo "   2. Drag ${APP_NAME} to Applications folder"
echo "   3. Eject the disk image"
echo "   4. Run ${APP_NAME} from Applications"
