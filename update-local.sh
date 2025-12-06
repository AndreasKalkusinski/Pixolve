#!/bin/bash

# Pixolve Development Update Script
# Quick rebuild and install for developers

set -e

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║           Pixolve - Development Update                    ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Get current version
VERSION=$(grep -A 1 "<Version>" src/Pixolve.Desktop/Pixolve.Desktop.csproj | tail -1 | sed 's/.*<Version>\(.*\)<\/Version>/\1/')

# Check if app is installed
if [ ! -d "/Applications/Pixolve.app" ]; then
    echo "⚠️  Pixolve is not installed in /Applications"
    echo "   Running full installation instead..."
    echo ""
    ./install-macos.sh
    exit 0
fi

# Get installed version
INSTALLED_VERSION=$(defaults read "/Applications/Pixolve.app/Contents/Info.plist" CFBundleShortVersionString 2>/dev/null || echo "unknown")

echo "🔍 Current Status:"
echo "   Installed: v${INSTALLED_VERSION}"
echo "   Building:  v${VERSION}"
echo ""

# Stop running instances
echo "🛑 Stopping running instances..."
pkill -f "Pixolve.Desktop" 2>/dev/null || true
sleep 1

# Build latest version
echo "🔨 Building latest version..."
echo ""
./publish-macos.sh
echo ""

# Backup current version
if [ -d "/Applications/Pixolve.app" ]; then
    echo "💾 Creating backup..."
    rm -rf /Applications/Pixolve.app.backup 2>/dev/null || true
    cp -r /Applications/Pixolve.app /Applications/Pixolve.app.backup
fi

# Install new version
echo "📦 Installing update..."
rm -rf /Applications/Pixolve.app
cp -r publish/Pixolve.app /Applications/Pixolve.app

# Verify installation
if [ ! -f "/Applications/Pixolve.app/Contents/MacOS/Pixolve.Desktop" ]; then
    echo "❌ Installation failed! Restoring backup..."
    rm -rf /Applications/Pixolve.app
    mv /Applications/Pixolve.app.backup /Applications/Pixolve.app
    exit 1
fi

# Clean up backup
rm -rf /Applications/Pixolve.app.backup 2>/dev/null || true

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║                   Update Successful! ✨                    ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""
echo "📍 Location: /Applications/Pixolve.app"
echo "📦 Version:  v${VERSION}"
echo ""

# Launch new version
read -p "🚀 Launch Pixolve now? (Y/n): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Nn]$ ]]; then
    open /Applications/Pixolve.app
    echo "   Pixolve is starting..."
fi

echo ""
