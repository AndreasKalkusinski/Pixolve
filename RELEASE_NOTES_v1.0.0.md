# Pixolve v1.0.0 - Initial Release

**Pixolve** ist ein moderner, plattformübergreifender Bild-Konverter mit einer benutzerfreundlichen Oberfläche.

## ✨ Features

### Konvertierung
- **Multi-Format-Unterstützung**: WebP, PNG, JPEG, AVIF
- **Batch-Konvertierung**: Mehrere Bilder gleichzeitig konvertieren
- **Fortschrittsanzeige**: Echtzeit-Status für jedes Bild
- **Bildgrößenanpassung**: Automatisches Skalieren auf maximale Pixelgröße
- **Qualitätseinstellungen**: Feinabstimmung der Ausgabequalität (0-100)

### Benutzeroberfläche
- **Moderne Avalonia UI** mit Fluent Design
- **Drag & Drop**: Ziehen Sie Dateien oder Ordner direkt in die Anwendung
- **Miniaturansichten**: 50px Vorschaubilder in der Dateiliste
- **Individuelle Einstellungen**: Überschreiben Sie globale Einstellungen für einzelne Bilder
- **Responsive Layout**: Passt sich der Fenstergröße an
- **Auto-Width Spalten**: Optimale Platznutzung in der Tabelle

### Weitere Features
- **Unterordner-Unterstützung**: Rekursives Laden von Bildern
- **Zeitstempel beibehalten**: Originale Dateidaten erhalten
- **Ausgabeverzeichnis**: Benutzerdefinierter Speicherort oder automatischer "converted"-Ordner
- **Dimensionssuffix**: Automatische Benennung mit maximaler Pixelgröße (z.B. "bild-1920.webp")
- **Einstellungen-Persistenz**: Ihre Präferenzen werden gespeichert

## 📦 Installation

### Windows
1. Laden Sie `Pixolve-v1.0.0-win-x64.zip` herunter
2. Entpacken Sie das Archiv
3. Führen Sie `Pixolve.Desktop.exe` aus

### Linux
1. Laden Sie `Pixolve-v1.0.0-linux-x64.zip` herunter
2. Entpacken Sie das Archiv
3. Machen Sie die Datei ausführbar: `chmod +x Pixolve.Desktop`
4. Führen Sie aus: `./Pixolve.Desktop`

### macOS
1. Laden Sie `Pixolve-v1.0.0-osx-x64.zip` herunter
2. Entpacken Sie das Archiv
3. Machen Sie die Datei ausführbar: `chmod +x Pixolve.Desktop`
4. Führen Sie aus: `./Pixolve.Desktop`

**Hinweis**: Alle Versionen sind selbstständig und benötigen keine .NET-Installation.

## 🚀 Verwendung

1. **Quellordner wählen**: Geben Sie einen Ordnerpfad ein oder klicken Sie auf "Durchsuchen"
2. **Bilder laden**: Klicken Sie auf "Bilder laden" oder ziehen Sie Dateien per Drag & Drop
3. **Einstellungen anpassen**:
   - Format (WebP, PNG, JPEG, AVIF)
   - Qualität (0-100)
   - Max Pixel (512-4096)
   - Optionen (Bildgröße anpassen, Dateien überschreiben, etc.)
4. **Individuelle Einstellungen** (optional): Klicken Sie auf das ⚙-Symbol bei einzelnen Bildern
5. **Konvertieren**: Klicken Sie auf "Alle konvertieren"

## 🛠️ Technische Details

- **Framework**: .NET 9.0
- **UI-Framework**: Avalonia UI 11.3.8
- **Bildverarbeitung**: SkiaSharp 2.88.9
- **Architektur**: MVVM mit CommunityToolkit.Mvvm
- **Plattformen**: Windows, Linux, macOS (x64)

## 📝 Lizenz

Dieses Projekt steht unter der MIT-Lizenz.

---

🤖 Generated with [Claude Code](https://claude.com/claude-code)
