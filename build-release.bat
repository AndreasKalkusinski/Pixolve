@echo off
echo Building Pixolve v1.1.0 for all platforms...
echo.

REM Set dotnet path
set "DOTNET_PATH=C:\Program Files\dotnet\dotnet.exe"
if not exist "%DOTNET_PATH%" set "DOTNET_PATH=C:\Program Files (x86)\dotnet\dotnet.exe"

REM Clean previous builds
if exist publish rmdir /s /q publish
mkdir publish

echo [1/3] Building for Windows x64...
"%DOTNET_PATH%" publish src\Pixolve.Desktop\Pixolve.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish\win-x64
if errorlevel 1 goto error

echo.
echo [2/3] Building for Linux x64...
"%DOTNET_PATH%" publish src\Pixolve.Desktop\Pixolve.Desktop.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish\linux-x64
if errorlevel 1 goto error

echo.
echo [3/3] Building for macOS x64...
"%DOTNET_PATH%" publish src\Pixolve.Desktop\Pixolve.Desktop.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish\osx-x64
if errorlevel 1 goto error

echo.
echo Creating ZIP archives...
cd publish

echo Zipping Windows build...
powershell -Command "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::CreateFromDirectory('win-x64', 'Pixolve-v1.1.0-win-x64.zip')"

echo Zipping Linux build...
powershell -Command "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::CreateFromDirectory('linux-x64', 'Pixolve-v1.1.0-linux-x64.zip')"

echo Zipping macOS build...
powershell -Command "Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::CreateFromDirectory('osx-x64', 'Pixolve-v1.1.0-osx-x64.zip')"

cd ..

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo ZIP files are ready in the publish folder:
echo - Pixolve-v1.1.0-win-x64.zip
echo - Pixolve-v1.1.0-linux-x64.zip
echo - Pixolve-v1.1.0-osx-x64.zip
echo.
pause
goto end

:error
echo.
echo ========================================
echo ERROR: Build failed!
echo ========================================
pause
exit /b 1

:end
