# Pixolve - Zukünftige Entwicklungsschritte

Dieses Dokument beschreibt die geplanten Verbesserungen und Features für Pixolve nach der v1.0.0 Release.

---

## Schritt 2: README Verbesserungen

### Ziele
- README.md mit Screenshots erweitern
- Installationsprozess detailliert dokumentieren
- Verwendungsanleitung mit Beispielen hinzufügen
- Feature-Liste mit visuellen Beispielen ergänzen

### Aufgaben
1. **Screenshots erstellen**:
   - Hauptfenster mit geladenen Bildern
   - Einstellungspanel (alle 3 Bereiche)
   - Individuelle Bild-Einstellungen (Flyout-Menü)
   - Drag & Drop in Aktion
   - Fortschrittsanzeige während Konvertierung
   - Vorher/Nachher Vergleich der Dateigröße

2. **README.md Struktur**:
   ```markdown
   # Pixolve
   [Logo und Badge-Zeile]

   ## 📸 Screenshots
   [Hauptfenster, Features in Aktion]

   ## ✨ Features
   [Detaillierte Feature-Liste mit Icons]

   ## 📦 Installation
   [Platform-spezifische Anleitungen]

   ## 🚀 Schnellstart
   [5-Schritte Anleitung mit Screenshots]

   ## ⚙️ Erweiterte Verwendung
   [Individuelle Einstellungen, Batch-Operationen]

   ## 🛠️ Technische Details
   [Architektur, Dependencies, Build-Anleitung]

   ## 🤝 Beitragen
   [Contribution Guidelines]

   ## 📝 Lizenz
   ```

3. **Badges hinzufügen**:
   - Build Status
   - Release Version
   - License
   - Platform Support
   - .NET Version

### Geschätzter Aufwand
1-2 Stunden

---

## Schritt 3: Testing & Qualitätssicherung

### Ziele
- Umfassende Tests mit verschiedenen Bildformaten
- Edge Cases identifizieren und beheben
- Per-Image Settings verifizieren
- Performance unter Last testen

### Testszenarien

#### Format-Tests
- [ ] JPEG → WebP Konvertierung
- [ ] PNG → WebP Konvertierung (mit Transparenz)
- [ ] GIF → PNG Konvertierung (animierte GIFs)
- [ ] BMP → JPEG Konvertierung
- [ ] WebP → PNG Konvertierung
- [ ] AVIF Input/Output Tests

#### Edge Cases
- [ ] Sehr große Bilder (>20MP, >50MP)
- [ ] Sehr kleine Bilder (<100x100px)
- [ ] Korrupte Dateien
- [ ] Ungültige Formate
- [ ] Dateien ohne Erweiterung
- [ ] Schreibgeschützte Ausgabeordner
- [ ] Sehr lange Dateinamen
- [ ] Sonderzeichen in Dateinamen (Umlaute, Emojis)
- [ ] Tausende von Bildern gleichzeitig

#### Feature-Tests
- [ ] Per-Image Settings überschreiben global settings korrekt
- [ ] Drag & Drop mit gemischten Dateitypen
- [ ] Unterordner-Rekursion funktioniert
- [ ] Zeitstempel-Erhaltung funktioniert
- [ ] Überschreiben-Modus funktioniert
- [ ] Abbrechen während Konvertierung
- [ ] Settings-Persistenz beim Neustart

#### Performance-Tests
- [ ] Memory-Nutzung bei 100+ Bildern
- [ ] CPU-Auslastung optimieren
- [ ] Thumbnail-Generierung Performance
- [ ] Multi-Threading Efficiency

### Unit Tests erstellen
```csharp
// Pixolve.Tests/Services/UniversalConverterTests.cs
- ConvertAsync_ValidImage_ReturnsSuccess
- ConvertAsync_InvalidFormat_ReturnsFailure
- ResizeImage_MaintainsAspectRatio
- GetOutputPath_AddsCorrectDimensionSuffix
```

### Geschätzter Aufwand
4-6 Stunden für vollständige Testabdeckung

---

## Schritt 4: Batch-Operationen Erweiterung

### Ziele
- Mehrere Ausgabeformate gleichzeitig generieren
- Konvertierungsprofile speichern und laden
- Preset-Verwaltung

### Features

#### Multi-Format Export
```
Beispiel: Ein Bild → WebP (Web) + PNG (Backup) + AVIF (Modern)
```

**UI-Änderungen**:
- Checkbox "Multi-Format Export"
- Liste mit wählbaren Formaten
- Pro Format individuelle Qualitätseinstellung

**Implementation**:
```csharp
public class MultiFormatSettings
{
    public List<FormatQualityPair> Formats { get; set; }
}

public class FormatQualityPair
{
    public ImageFormat Format { get; set; }
    public int Quality { get; set; }
}
```

#### Konvertierungs-Presets
**Vordefinierte Presets**:
1. **Web Optimiert**
   - Format: WebP
   - Qualität: 85
   - Max Pixel: 1920
   - Bildgröße anpassen: Ja

2. **Hohe Qualität**
   - Format: PNG
   - Qualität: 100
   - Max Pixel: 4096
   - Bildgröße anpassen: Nein

3. **Thumbnail**
   - Format: JPEG
   - Qualität: 75
   - Max Pixel: 512
   - Bildgröße anpassen: Ja

4. **Modern (AVIF)**
   - Format: AVIF
   - Qualität: 90
   - Max Pixel: 2560
   - Bildgröße anpassen: Ja

**UI-Änderungen**:
```xml
<ComboBox Header="Preset laden">
    <ComboBoxItem>Web Optimiert</ComboBoxItem>
    <ComboBoxItem>Hohe Qualität</ComboBoxItem>
    <ComboBoxItem>Thumbnail</ComboBoxItem>
    <ComboBoxItem>Modern (AVIF)</ComboBoxItem>
    <ComboBoxItem>Benutzerdefiniert...</ComboBoxItem>
</ComboBox>
<Button Content="Aktuelles Preset speichern"/>
```

**Speicherung**:
```json
{
  "presets": [
    {
      "name": "Web Optimiert",
      "settings": { ... }
    }
  ]
}
```

### Geschätzter Aufwand
6-8 Stunden

---

## Schritt 5: Erweiterte Features

### EXIF-Daten Verwaltung

**Features**:
- EXIF-Daten anzeigen (Kamera, Datum, GPS, etc.)
- EXIF-Daten beibehalten/entfernen Option
- EXIF-Daten editieren (Copyright, Autor, etc.)

**Dependencies**:
```xml
<PackageReference Include="MetadataExtractor" Version="2.8.1" />
```

**UI-Erweiterung**:
```xml
<Button Content="ℹ" ToolTip.Tip="EXIF Info">
    <Button.Flyout>
        <Flyout>
            <StackPanel>
                <TextBlock Text="Kamera: Canon EOS R5"/>
                <TextBlock Text="Datum: 2024-01-15 14:32"/>
                <TextBlock Text="ISO: 400"/>
                <TextBlock Text="Blende: f/2.8"/>
                <!-- ... -->
                <CheckBox Content="EXIF in Ausgabe beibehalten"/>
            </StackPanel>
        </Flyout>
    </Button.Flyout>
</Button>
```

### Wasserzeichen

**Features**:
- Text-Wasserzeichen hinzufügen
- Bild-Wasserzeichen (Logo) hinzufügen
- Position wählbar (Ecken, Mitte)
- Transparenz einstellbar
- Größe/Schriftart konfigurierbar

**Implementation mit SkiaSharp**:
```csharp
private SKBitmap AddWatermark(SKBitmap original, WatermarkSettings settings)
{
    using var canvas = new SKCanvas(original);

    if (settings.Type == WatermarkType.Text)
    {
        using var paint = new SKPaint
        {
            Color = settings.Color.WithAlpha(settings.Opacity),
            TextSize = settings.FontSize,
            IsAntialias = true
        };

        var position = GetWatermarkPosition(settings.Position, original.Width, original.Height);
        canvas.DrawText(settings.Text, position.X, position.Y, paint);
    }
    // ... Bild-Wasserzeichen

    return original;
}
```

### Bildoptimierung

**Features**:
- Schärfen (Sharpen)
- Kontrast anpassen
- Helligkeit anpassen
- Sättigung anpassen
- Auto-Optimierung (AI-basiert optional)

**UI**:
```xml
<Expander Header="🎨 Bildoptimierung">
    <StackPanel>
        <Slider Header="Schärfen" Minimum="0" Maximum="100"/>
        <Slider Header="Kontrast" Minimum="-50" Maximum="50"/>
        <Slider Header="Helligkeit" Minimum="-50" Maximum="50"/>
        <Slider Header="Sättigung" Minimum="-50" Maximum="50"/>
        <CheckBox Content="Auto-Optimierung"/>
    </StackPanel>
</Expander>
```

### Geschätzter Aufwand
10-15 Stunden (komplett)
- EXIF: 3-4 Stunden
- Wasserzeichen: 4-5 Stunden
- Bildoptimierung: 3-6 Stunden

---

## Schritt 6: UI/UX Verbesserungen

### Dark Mode

**Implementation**:
```csharp
public enum AppTheme
{
    Light,
    Dark,
    System // Folgt dem OS-Theme
}
```

**Avalonia Theme Switching**:
```csharp
private void SetTheme(AppTheme theme)
{
    var themeVariant = theme switch
    {
        AppTheme.Light => ThemeVariant.Light,
        AppTheme.Dark => ThemeVariant.Dark,
        AppTheme.System => DetectSystemTheme(),
        _ => ThemeVariant.Default
    };

    Application.Current.RequestedThemeVariant = themeVariant;
}
```

**UI**:
```xml
<ComboBox Header="Theme" SelectedIndex="{Binding SelectedTheme}">
    <ComboBoxItem Content="Hell"/>
    <ComboBoxItem Content="Dunkel"/>
    <ComboBoxItem Content="System"/>
</ComboBox>
```

### Mehrsprachigkeit (i18n)

**Unterstützte Sprachen (Phase 1)**:
- Deutsch (aktuell)
- Englisch
- Französisch
- Spanisch

**Implementation**:
```csharp
// Resources/Strings.de.resx
// Resources/Strings.en.resx
// Resources/Strings.fr.resx
// Resources/Strings.es.resx

public class LocalizationService
{
    public string GetString(string key)
    {
        var culture = CultureInfo.CurrentUICulture;
        return ResourceManager.GetString(key, culture);
    }
}
```

**In XAML**:
```xml
<TextBlock Text="{Binding Source={x:Static loc:Strings.SourceFolder}}"/>
```

### Konvertierungs-Historie

**Features**:
- Historie der letzten 50 Konvertierungen
- Datum, Zeit, Anzahl Bilder, Formate
- "Wiederholen"-Button
- Export als CSV

**Datenmodell**:
```csharp
public class ConversionHistory
{
    public DateTime Timestamp { get; set; }
    public int ImageCount { get; set; }
    public ImageFormat TargetFormat { get; set; }
    public long TotalSizeBefore { get; set; }
    public long TotalSizeAfter { get; set; }
    public ConversionSettings Settings { get; set; }
}
```

**UI**:
```xml
<TabControl>
    <TabItem Header="Konvertieren">
        <!-- Existing UI -->
    </TabItem>
    <TabItem Header="Historie">
        <DataGrid ItemsSource="{Binding History}">
            <!-- History columns -->
        </DataGrid>
    </TabItem>
</TabControl>
```

### Vorher/Nachher Vergleichsansicht

**Features**:
- Side-by-Side Bildvergleich
- Slider für wischenden Übergang
- Zoom-Funktion
- Metadaten-Vergleich (Größe, Qualität)

**UI**:
```xml
<Window Title="Bildvergleich">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <StackPanel Grid.Column="0">
            <TextBlock Text="Vorher" FontWeight="Bold"/>
            <Image Source="{Binding OriginalImage}"/>
            <TextBlock Text="{Binding OriginalSize}"/>
        </StackPanel>

        <StackPanel Grid.Column="1">
            <TextBlock Text="Nachher" FontWeight="Bold"/>
            <Image Source="{Binding ConvertedImage}"/>
            <TextBlock Text="{Binding ConvertedSize}"/>
        </StackPanel>
    </Grid>
</Window>
```

### Geschätzter Aufwand
8-12 Stunden
- Dark Mode: 2-3 Stunden
- i18n: 3-4 Stunden
- Historie: 2-3 Stunden
- Vergleichsansicht: 1-2 Stunden

---

## Schritt 7: Performance-Optimierung

### Multi-Threading Verbesserungen

**Aktuell**: Sequential processing mit async/await
**Ziel**: Parallel processing mit TPL Dataflow

```csharp
public async Task ConvertImagesParallelAsync(
    IEnumerable<ImageFile> images,
    ConversionSettings settings,
    int maxDegreeOfParallelism = 4)
{
    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
        CancellationToken = cancellationToken
    };

    await Parallel.ForEachAsync(images, options, async (image, ct) =>
    {
        await ConvertImageAsync(image, settings, ct);
    });
}
```

**UI-Einstellung**:
```xml
<NumericUpDown Header="Parallele Konvertierungen"
               Minimum="1"
               Maximum="{Binding ProcessorCount}"
               Value="{Binding ParallelismDegree}"/>
```

### Memory Management

**Problem**: Große Bilder können viel RAM verbrauchen

**Lösungen**:
1. **Streaming-basierte Verarbeitung**:
```csharp
using var inputStream = File.OpenRead(inputPath);
using var skStream = new SKManagedStream(inputStream);
using var codec = SKCodec.Create(skStream);
// Process in chunks
```

2. **Aggressive Garbage Collection**:
```csharp
private void ProcessBatch(List<ImageFile> batch)
{
    foreach (var image in batch)
    {
        ProcessImage(image);

        if (batch.IndexOf(image) % 10 == 0)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
```

3. **Memory Pool für Thumbnails**:
```csharp
private static readonly ArrayPool<byte> ThumbnailPool = ArrayPool<byte>.Shared;
```

### Per-Image Fortschrittsanzeige

**Feature**: Zeige Fortschritt für jedes Bild einzeln

```csharp
public class ImageFile
{
    private int _progress; // 0-100

    public int Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }
}
```

**UI**:
```xml
<DataGridTemplateColumn Header="Fortschritt" Width="100">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <ProgressBar Value="{Binding Progress}"
                        Minimum="0"
                        Maximum="100"
                        Height="16"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

### Caching-Strategie für Thumbnails

```csharp
public class ThumbnailCache
{
    private readonly MemoryCache _cache;

    public byte[]? GetThumbnail(string filePath)
    {
        var key = $"{filePath}:{File.GetLastWriteTime(filePath).Ticks}";
        return _cache.Get<byte[]>(key);
    }

    public void SetThumbnail(string filePath, byte[] thumbnail)
    {
        var key = $"{filePath}:{File.GetLastWriteTime(filePath).Ticks}";
        _cache.Set(key, thumbnail, TimeSpan.FromMinutes(30));
    }
}
```

### Geschätzter Aufwand
6-10 Stunden

---

## Schritt 8: Distribution & Updates

### Microsoft Store

**Voraussetzungen**:
- Microsoft Developer Account ($19/Jahr oder $99 Lifetime)
- App-Zertifizierung
- Privacy Policy
- Support-Kontakt

**Package erstellen**:
```bash
dotnet publish -c Release -r win-x64 \
  -p:PublishSingleFile=true \
  -p:WindowsPackageType=MSIX \
  -p:WindowsAppSDKSelfContained=true
```

**MSIX Manifest**:
```xml
<Package xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10">
    <Identity Name="Pixolve"
              Publisher="CN=AndreasKalkusinski"
              Version="1.0.0.0"/>
    <Properties>
        <DisplayName>Pixolve</DisplayName>
        <PublisherDisplayName>Andreas Kalkusinski</PublisherDisplayName>
        <Logo>Assets\StoreLogo.png</Logo>
    </Properties>
    <!-- ... -->
</Package>
```

### Mac App Store

**Voraussetzungen**:
- Apple Developer Account ($99/Jahr)
- Code-Signierung
- Notarization
- Sandbox-Kompatibilität

**App Bundle erstellen**:
```bash
dotnet publish -c Release -r osx-x64 \
  -p:PublishSingleFile=false \
  -p:CreatePackage=true
```

**Code Signing**:
```bash
codesign --deep --force --verify --verbose \
  --sign "Developer ID Application: Andreas Kalkusinski" \
  Pixolve.app
```

### Chocolatey Package (Windows)

**Package Definition** (`pixolve.nuspec`):
```xml
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2015/06/nuspec.xsd">
  <metadata>
    <id>pixolve</id>
    <version>1.0.0</version>
    <title>Pixolve</title>
    <authors>Andreas Kalkusinski</authors>
    <description>Modern cross-platform image converter</description>
    <projectUrl>https://github.com/AndreasKalkusinski/Pixolve</projectUrl>
    <tags>image converter webp png jpeg avif</tags>
  </metadata>
  <files>
    <file src="tools\**" target="tools" />
  </files>
</package>
```

**Installation**:
```powershell
choco install pixolve
```

### Homebrew Cask (macOS)

**Cask Definition** (`pixolve.rb`):
```ruby
cask "pixolve" do
  version "1.0.0"
  sha256 "..."

  url "https://github.com/AndreasKalkusinski/Pixolve/releases/download/v#{version}/Pixolve-v#{version}-osx-x64.zip"
  name "Pixolve"
  desc "Modern cross-platform image converter"
  homepage "https://github.com/AndreasKalkusinski/Pixolve"

  app "Pixolve.app"
end
```

**Installation**:
```bash
brew install --cask pixolve
```

### Auto-Update Funktionalität

**Dependencies**:
```xml
<PackageReference Include="Velopack" Version="0.0.500" />
```

**Implementation**:
```csharp
public class UpdateService
{
    private readonly UpdateManager _updateManager;

    public async Task<bool> CheckForUpdatesAsync()
    {
        var updateInfo = await _updateManager.CheckForUpdate();
        return updateInfo != null;
    }

    public async Task DownloadAndInstallUpdateAsync()
    {
        await _updateManager.UpdateApp();
    }
}
```

**UI**:
```xml
<MenuItem Header="Nach Updates suchen..."
          Command="{Binding CheckForUpdatesCommand}"/>
```

### Geschätzter Aufwand
15-20 Stunden (komplett)
- Store-Vorbereitung: 3-4 Stunden
- Microsoft Store: 3-4 Stunden
- Mac App Store: 4-5 Stunden
- Package Manager: 2-3 Stunden
- Auto-Update: 3-4 Stunden

---

## Priorisierte Roadmap

### Phase 1 (v1.1.0) - Quick Wins
**Zeitrahmen**: 1-2 Wochen
- Schritt 2: README Verbesserungen ✅
- Schritt 3: Basic Testing ✅
- Dark Mode (aus Schritt 6)
- Multi-Format Export (aus Schritt 4)

### Phase 2 (v1.2.0) - Quality of Life
**Zeitrahmen**: 2-3 Wochen
- Schritt 4: Preset-System komplett
- Schritt 6: i18n (Englisch + Deutsch)
- Schritt 7: Performance-Optimierung Basics

### Phase 3 (v2.0.0) - Advanced Features
**Zeitrahmen**: 4-6 Wochen
- Schritt 5: EXIF + Wasserzeichen
- Schritt 6: Historie + Vergleichsansicht
- Schritt 7: Advanced Performance

### Phase 4 (v2.1.0) - Distribution
**Zeitrahmen**: 3-4 Wochen
- Schritt 8: Store-Submissions
- Schritt 8: Package Manager
- Schritt 8: Auto-Update

---

## Zusätzliche Ideen (Backlog)

### Cloud-Integration
- OneDrive/Google Drive/Dropbox Integration
- Direkte Konvertierung aus Cloud-Speicher
- Upload konvertierter Bilder zurück zur Cloud

### Kommandozeilen-Interface
```bash
pixolve convert --input ./photos --format webp --quality 85 --max-size 1920
pixolve batch --preset "web-optimized" --recursive ./images
```

### Plugin-System
- Eigene Konverter-Plugins
- Filter-Plugins
- Export-Plugins

### Batch-Renaming
- Pattern-basiertes Umbenennen
- Sequenznummern
- Metadaten in Dateinamen

### API/REST Service
- Pixolve als Service betreiben
- REST API für Konvertierung
- Docker Container

---

**Dokumentation erstellt**: November 2024
**Letzte Aktualisierung**: v1.0.0 Release
**Autor**: Andreas Kalkusinski & Claude Code
