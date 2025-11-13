#!/bin/bash

# Pixolve macOS Publish Script
# Creates a ready-to-distribute .app bundle

set -e

VERSION="1.1.0"
APP_NAME="Pixolve"
BUNDLE_ID="com.pixolve.desktop"
PUBLISH_DIR="./publish"

echo "🚀 Building ${APP_NAME} v${VERSION} for macOS..."

# Build for ARM64 (Apple Silicon)
echo "📦 Building for ARM64 (Apple Silicon)..."
dotnet publish src/Pixolve.Desktop/Pixolve.Desktop.csproj \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -p:PublishSingleFile=false \
    -o "${PUBLISH_DIR}/temp-arm64"

# Create .app bundle structure
echo "📁 Creating .app bundle..."
APP_BUNDLE="${PUBLISH_DIR}/${APP_NAME}.app"
rm -rf "${APP_BUNDLE}"
mkdir -p "${APP_BUNDLE}/Contents/"{MacOS,Resources}

# Copy application files
echo "📋 Copying application files..."
cp -r "${PUBLISH_DIR}/temp-arm64/"* "${APP_BUNDLE}/Contents/MacOS/"
chmod +x "${APP_BUNDLE}/Contents/MacOS/Pixolve.Desktop"

# Create Info.plist
echo "📝 Creating Info.plist..."
cat > "${APP_BUNDLE}/Contents/Info.plist" << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>${APP_NAME}</string>
    <key>CFBundleDisplayName</key>
    <string>${APP_NAME}</string>
    <key>CFBundleIdentifier</key>
    <string>${BUNDLE_ID}</string>
    <key>CFBundleVersion</key>
    <string>${VERSION}</string>
    <key>CFBundleShortVersionString</key>
    <string>${VERSION}</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleSignature</key>
    <string>????</string>
    <key>CFBundleExecutable</key>
    <string>Pixolve.Desktop</string>
    <key>CFBundleIconFile</key>
    <string>pixolve-logo.icns</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.15</string>
    <key>NSHighResolutionCapable</key>
    <true/>
    <key>NSSupportsAutomaticGraphicsSwitching</key>
    <true/>
</dict>
</plist>
EOF

# Create app icon (.icns)
echo "🎨 Creating app icon..."
TEMP_PNG="/tmp/pixolve-temp.png"
ICONSET_DIR="/tmp/pixolve.iconset"

sips -s format png src/Pixolve.Desktop/Assets/pixolve-logo.ico --out "${TEMP_PNG}" > /dev/null 2>&1

rm -rf "${ICONSET_DIR}"
mkdir -p "${ICONSET_DIR}"

sips -z 16 16 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_16x16.png" > /dev/null 2>&1
sips -z 32 32 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_16x16@2x.png" > /dev/null 2>&1
sips -z 32 32 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_32x32.png" > /dev/null 2>&1
sips -z 64 64 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_32x32@2x.png" > /dev/null 2>&1
sips -z 128 128 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_128x128.png" > /dev/null 2>&1
sips -z 256 256 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_128x128@2x.png" > /dev/null 2>&1
sips -z 256 256 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_256x256.png" > /dev/null 2>&1
sips -z 512 512 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_256x256@2x.png" > /dev/null 2>&1
sips -z 512 512 "${TEMP_PNG}" --out "${ICONSET_DIR}/icon_512x512.png" > /dev/null 2>&1

iconutil -c icns "${ICONSET_DIR}" -o "${APP_BUNDLE}/Contents/Resources/pixolve-logo.icns"

# Remove quarantine attributes and sign
echo "🔐 Signing app bundle..."
xattr -cr "${APP_BUNDLE}"
codesign --force --deep --sign - "${APP_BUNDLE}"

# Create ZIP archive
echo "📦 Creating ZIP archive..."
cd "${PUBLISH_DIR}"
ZIP_NAME="${APP_NAME}-v${VERSION}-macOS.zip"
rm -f "${ZIP_NAME}"
zip -r -q "${ZIP_NAME}" "${APP_NAME}.app"
cd ..

# Cleanup
echo "🧹 Cleaning up..."
rm -rf "${PUBLISH_DIR}/temp-arm64"
rm -f "${TEMP_PNG}"
rm -rf "${ICONSET_DIR}"

echo "✅ Done!"
echo ""
echo "📦 Created: ${PUBLISH_DIR}/${ZIP_NAME}"
echo "🍎 .app bundle: ${APP_BUNDLE}"
echo ""
echo "📝 Installation instructions:"
echo "   1. Extract ${ZIP_NAME}"
echo "   2. Drag ${APP_NAME}.app to Applications folder"
echo "   3. Double-click to run"
echo ""
echo "⚠️  Note: Users may need to right-click → Open on first launch"
echo "   (if app is not code-signed)"
