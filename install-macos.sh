#!/bin/bash

# Pixolve macOS Installer
# Professional installation script for end users

set -e

VERSION="1.2.0"
APP_NAME="Pixolve"
INSTALL_DIR="/Applications"
DMG_URL="https://github.com/AndreasKalkusinski/Pixolve/releases/download/v${VERSION}/Pixolve-v${VERSION}-macOS.dmg"

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║         Pixolve v${VERSION} - macOS Installer                  ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Check if running on macOS
if [[ "$(uname)" != "Darwin" ]]; then
    echo "❌ Error: This installer is for macOS only"
    exit 1
fi

# Check for existing installation
if [ -d "${INSTALL_DIR}/${APP_NAME}.app" ]; then
    echo "📦 Existing installation found"
    CURRENT_VERSION=$(defaults read "${INSTALL_DIR}/${APP_NAME}.app/Contents/Info.plist" CFBundleShortVersionString 2>/dev/null || echo "unknown")
    echo "   Current version: v${CURRENT_VERSION}"
    echo ""

    if [ "${CURRENT_VERSION}" = "${VERSION}" ]; then
        read -p "   Same version detected. Reinstall? (y/N): " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            echo "Installation cancelled."
            exit 0
        fi
    else
        read -p "   Upgrade to v${VERSION}? (Y/n): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Nn]$ ]]; then
            echo "Installation cancelled."
            exit 0
        fi
    fi

    # Stop running instances
    echo "🛑 Stopping running instances..."
    killall ${APP_NAME}.Desktop 2>/dev/null || true
    sleep 1

    # Backup old version
    echo "💾 Backing up current version..."
    mv "${INSTALL_DIR}/${APP_NAME}.app" "${INSTALL_DIR}/${APP_NAME}.app.backup" 2>/dev/null || true
fi

# Check if local build exists (for development)
if [ -f "./publish/${APP_NAME}.app/Contents/MacOS/${APP_NAME}.Desktop" ]; then
    echo "📦 Local build found - installing from local files..."

    # Remove old backup
    rm -rf "${INSTALL_DIR}/${APP_NAME}.app.backup" 2>/dev/null || true

    # Install
    cp -r "./publish/${APP_NAME}.app" "${INSTALL_DIR}/"

    # Remove quarantine and sign
    xattr -cr "${INSTALL_DIR}/${APP_NAME}.app"
    codesign --force --deep --sign - "${INSTALL_DIR}/${APP_NAME}.app" 2>/dev/null || true

    echo ""
    echo "✅ Installation complete!"
else
    echo "ℹ️  For automated installation from GitHub releases,"
    echo "   download and run: curl -fsSL https://pixolve.com/install.sh | bash"
    echo ""
    echo "❌ No local build found. Run ./publish-macos.sh first."
    exit 1
fi

# Verify installation
if [ -f "${INSTALL_DIR}/${APP_NAME}.app/Contents/MacOS/${APP_NAME}.Desktop" ]; then
    INSTALLED_VERSION=$(defaults read "${INSTALL_DIR}/${APP_NAME}.app/Contents/Info.plist" CFBundleShortVersionString)

    echo ""
    echo "╔════════════════════════════════════════════════════════════╗"
    echo "║                                                            ║"
    echo "║                Installation Successful! ✨                 ║"
    echo "║                                                            ║"
    echo "╚════════════════════════════════════════════════════════════╝"
    echo ""
    echo "📍 Location: ${INSTALL_DIR}/${APP_NAME}.app"
    echo "📦 Version:  v${INSTALLED_VERSION}"
    echo ""
    echo "🚀 Launch ${APP_NAME}:"
    echo "   • From Applications folder"
    echo "   • Or run: open ${INSTALL_DIR}/${APP_NAME}.app"
    echo ""
    echo "⚠️  First Launch Note:"
    echo "   If macOS shows a security warning:"
    echo "   1. Right-click ${APP_NAME}.app → 'Open'"
    echo "   2. Or: System Settings → Privacy & Security → 'Open Anyway'"
    echo ""

    # Offer to launch
    read -p "Launch ${APP_NAME} now? (Y/n): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Nn]$ ]]; then
        open "${INSTALL_DIR}/${APP_NAME}.app"
        echo "🎉 ${APP_NAME} is starting..."
    fi

    # Clean up backup
    rm -rf "${INSTALL_DIR}/${APP_NAME}.app.backup" 2>/dev/null || true
else
    echo "❌ Installation verification failed"

    # Restore backup if exists
    if [ -d "${INSTALL_DIR}/${APP_NAME}.app.backup" ]; then
        echo "🔄 Restoring previous version..."
        rm -rf "${INSTALL_DIR}/${APP_NAME}.app"
        mv "${INSTALL_DIR}/${APP_NAME}.app.backup" "${INSTALL_DIR}/${APP_NAME}.app"
    fi

    exit 1
fi

echo ""
echo "📚 Documentation: https://github.com/AndreasKalkusinski/Pixolve"
echo "🐛 Report Issues: https://github.com/AndreasKalkusinski/Pixolve/issues"
echo ""
