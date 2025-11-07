# GitHub Release v1.1.0 Anleitung

## Schritt 1: Builds erstellen
Führe die `build-release.bat` aus:
```cmd
build-release.bat
```

Dies erstellt:
- `publish/Pixolve-v1.1.0-win-x64.zip`
- `publish/Pixolve-v1.1.0-linux-x64.zip`
- `publish/Pixolve-v1.1.0-osx-x64.zip`

## Schritt 2: GitHub Release erstellen

### Option A: Via GitHub CLI (gh)
```cmd
gh release create v1.1.0 ^
  --title "Pixolve v1.1.0 - Multilingual Support" ^
  --notes-file RELEASE_NOTES_v1.1.0.md ^
  publish\Pixolve-v1.1.0-win-x64.zip ^
  publish\Pixolve-v1.1.0-linux-x64.zip ^
  publish\Pixolve-v1.1.0-osx-x64.zip
```

### Option B: Via GitHub Web UI
1. Gehe zu: https://github.com/DEIN-USERNAME/Pixolve/releases/new
2. Tag: `v1.1.0`
3. Title: `Pixolve v1.1.0 - Multilingual Support`
4. Description: Kopiere den Inhalt aus `RELEASE_NOTES_v1.1.0.md`
5. Lade die ZIP-Dateien aus dem `publish` Ordner hoch:
   - Pixolve-v1.1.0-win-x64.zip
   - Pixolve-v1.1.0-linux-x64.zip
   - Pixolve-v1.1.0-osx-x64.zip
6. Klicke auf "Publish release"

## Schritt 3: Lokale Installation aktualisieren

### Windows Installation updaten:
```cmd
cd c:\repo\Pixolve\publish\win-x64
copy Pixolve.Desktop.exe "DEIN-INSTALLATIONS-PFAD\"
```

Oder: Extrahiere `Pixolve-v1.1.0-win-x64.zip` an den gewünschten Ort.

## Hinweise
- Die Release Notes sind bereits in `RELEASE_NOTES_v1.1.0.md` vorbereitet
- Die Screenshots für das Release sind in `docs/screenshots/` verfügbar
- Vergiss nicht, das Repository zu taggen: `git tag v1.1.0 && git push origin v1.1.0`
