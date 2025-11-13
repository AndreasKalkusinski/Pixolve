# Pixolve Installation - Anleitung für deinen Kollegen

Schick ihm diese Nachricht zusammen mit der DMG-Datei:

---

## Hi! Hier ist Pixolve v1.1.0 für macOS 🎉

### Installation (3 einfache Schritte):

**1. DMG öffnen**
- Doppelklick auf `Pixolve-v1.1.0-macOS.dmg`

**2. App in Programme kopieren**
- Ziehe `Pixolve.app` in den `Applications` Ordner

**3. Sicherheitswarnung beheben**
- Öffne das **Terminal** (Cmd+Leertaste → "Terminal" eingeben)
- Kopiere diese Zeilen und drücke Enter:

```bash
cd /Applications
xattr -cr Pixolve.app
open Pixolve.app
```

**Fertig!** Die App startet jetzt. 🚀

---

### Warum die Terminal-Befehle?

macOS blockiert Apps, die nicht von Apple "notarisiert" sind. Die App ist 100% sicher und Open Source, aber nicht bei Apple registriert (kostet 99$/Jahr).

Der Befehl entfernt einfach die "Quarantäne-Markierung". Danach funktioniert alles normal!

---

### Falls das Terminal nicht klappt:

**Alternative:**
1. Versuche die App zu öffnen (wird blockiert)
2. Gehe zu **Systemeinstellungen** → **Datenschutz & Sicherheit**
3. Scrolle runter bis "Pixolve wurde blockiert"
4. Klicke **"Dennoch öffnen"**

---

### Was kann die App?

- ✨ Bilder in WebP, AVIF, PNG, JPEG konvertieren
- 📦 Batch-Verarbeitung (viele Bilder auf einmal)
- 🌓 Dark Mode
- 🇩🇪🇬🇧 Deutsch & Englisch
- 🎨 Modern & schnell

---

Viel Spaß! Bei Fragen einfach melden. 😊

---

## Technische Details (falls gefragt):

- **App:** Pixolve-v1.1.0-macOS.dmg (49 MB)
- **Plattform:** macOS 10.15+ (Catalina oder neuer)
- **Architektur:** Apple Silicon (M1/M2/M3) optimiert
- **Source Code:** https://github.com/AndreasKalkusinski/Pixolve
- **Lizenz:** MIT (Open Source)
