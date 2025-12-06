# Pixolve Windows Build Script
# Creates a self-contained Windows x64 build

$VERSION = "1.2.0"
$APP_NAME = "Pixolve"
$PROJECT_PATH = "src/Pixolve.Desktop/Pixolve.Desktop.csproj"
$OUTPUT_DIR = "publish/win-x64"
$ZIP_FILE = "publish/$APP_NAME-v$VERSION-win-x64.zip"

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "                                                                " -ForegroundColor Cyan
Write-Host "           Pixolve v$VERSION - Windows Build                    " -ForegroundColor Cyan
Write-Host "                                                                " -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous build
if (Test-Path $OUTPUT_DIR) {
    Write-Host "🧹 Cleaning previous build..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force $OUTPUT_DIR
}

# Build self-contained Windows x64 version
Write-Host "🔨 Building Windows x64 release..." -ForegroundColor Green
Write-Host ""

dotnet publish $PROJECT_PATH `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $OUTPUT_DIR

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "📦 Creating ZIP archive..." -ForegroundColor Green

# Remove old ZIP if exists
if (Test-Path $ZIP_FILE) {
    Remove-Item -Force $ZIP_FILE
}

# Create ZIP archive
Compress-Archive -Path "$OUTPUT_DIR\*" -DestinationPath $ZIP_FILE

# Get file size
$zipSize = (Get-Item $ZIP_FILE).Length / 1MB
$zipSizeMB = [math]::Round($zipSize, 2)

Write-Host ""
Write-Host "================================================================" -ForegroundColor Green
Write-Host "                                                                " -ForegroundColor Green
Write-Host "                   Build Successful! ✨                         " -ForegroundColor Green
Write-Host "                                                                " -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Green
Write-Host ""
Write-Host "📦 Created: .\$ZIP_FILE" -ForegroundColor Cyan
Write-Host "📊 Size:    $zipSizeMB MB" -ForegroundColor Cyan
Write-Host ""
Write-Host "📝 Distribution:" -ForegroundColor Yellow
Write-Host "   1. Upload $APP_NAME-v$VERSION-win-x64.zip to GitHub Releases"
Write-Host "   2. Users extract and run Pixolve.Desktop.exe"
Write-Host ""
Write-Host "⚠️  Note: Windows SmartScreen may show a warning on first run" -ForegroundColor Yellow
Write-Host "   Users should click 'More info' → 'Run anyway'" -ForegroundColor Yellow
Write-Host ""
