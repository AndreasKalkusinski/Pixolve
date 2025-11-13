# macOS Installation Guide - Pixolve

## Problem: "kann nicht überprüft werden" / "Cannot verify"

macOS zeigt diese Warnung, weil die App nicht von Apple notarisiert ist. Das ist **normal** und **sicher** - die App ist clean, aber nicht offiziell bei Apple registriert.

## ✅ Lösung 1: Terminal Befehl (EINFACHSTE METHODE)

Dein Kollege soll folgendes im Terminal ausführen:

### Schritt 1: Zur App navigieren
```bash
cd /Applications
```

### Schritt 2: Quarantäne entfernen
```bash
xattr -cr Pixolve.app
```

### Schritt 3: App starten
```bash
open Pixolve.app
```

**FERTIG!** Die App startet jetzt ohne Warnung.

---

## ✅ Lösung 2: Systemeinstellungen (GUI-Methode)

1. **Erste Warnung erscheint** beim Doppelklick auf die App
2. Klicke auf **"Abbrechen"**
3. Gehe zu **Systemeinstellungen** → **Datenschutz & Sicherheit**
4. Scrolle nach unten, dort steht:
   *"Pixolve wurde blockiert"*
5. Klicke auf **"Dennoch öffnen"**
6. Bestätige mit **"Öffnen"**

**FERTIG!** Ab jetzt startet die App normal.

---

## ✅ Lösung 3: Rechtsklick-Methode (manchmal funktioniert es)

1. **Rechtsklick** auf `Pixolve.app`
2. Wähle **"Öffnen"** aus dem Kontextmenü
3. Klicke im Dialog auf **"Öffnen"**

⚠️ **Hinweis:** Bei neueren macOS Versionen funktioniert diese Methode manchmal nicht mehr. Verwende dann Lösung 1 oder 2.

---

## 📦 Komplette Installationsanleitung

### Mit DMG (Empfohlen):

1. **Download**: `Pixolve-v1.1.0-macOS.dmg`
2. **Doppelklick** auf die DMG-Datei
3. **Ziehe** `Pixolve.app` in den `Applications` Ordner
4. **Werfe** das DMG aus (Rechtsklick → Auswerfen)
5. **Öffne Terminal** und führe aus:
   ```bash
   cd /Applications
   xattr -cr Pixolve.app
   open Pixolve.app
   ```

### Mit ZIP:

1. **Download**: `Pixolve-v1.1.0-macOS.zip`
2. **Entpacke** die Datei (Doppelklick)
3. **Ziehe** `Pixolve.app` in `/Applications`
4. **Öffne Terminal** und führe aus:
   ```bash
   cd /Applications
   xattr -cr Pixolve.app
   open Pixolve.app
   ```

---

## ❓ Warum passiert das?

macOS **Gatekeeper** blockiert Apps, die:
- Nicht aus dem App Store kommen
- Nicht von einem registrierten Apple Developer signiert sind
- Nicht bei Apple notarisiert wurden

**Die App ist sicher!** Sie ist nur nicht bei Apple registriert (kostet 99$/Jahr).

---

## 🔒 Ist das sicher?

**JA!** Der Befehl `xattr -cr` entfernt nur die "Quarantäne-Markierung", die macOS beim Download automatisch setzt. Es ändert nichts an der App selbst.

- ✅ Die App ist Open Source
- ✅ Der Code ist auf GitHub einsehbar
- ✅ Keine Malware, keine Spyware
- ✅ Lokale Verarbeitung, keine Cloud-Uploads

---

## 💡 Für Entwickler: App signieren

Um diese Warnung zu vermeiden, bräuchtest du:

1. **Apple Developer Account** (99$/Jahr)
2. **Developer Certificate** erstellen
3. **App signieren**:
   ```bash
   codesign --deep --force --sign "Developer ID Application: Dein Name" Pixolve.app
   ```
4. **App notarisieren** bei Apple:
   ```bash
   xcrun notarytool submit Pixolve.dmg --keychain-profile "notary"
   ```
5. **Notarisierung anheften**:
   ```bash
   xcrun stapler staple Pixolve.app
   ```

---

## 📞 Support

Falls es immer noch nicht funktioniert:

1. Prüfe die macOS Version: `sw_vers`
2. Prüfe die Berechtigungen: `ls -l /Applications/Pixolve.app`
3. Prüfe die Attribute: `xattr -l /Applications/Pixolve.app`

Öffne ein Issue auf GitHub mit diesen Infos!

---

## ✅ Schnell-Referenz

**Problem:** App lässt sich nicht öffnen
**Lösung:**
```bash
cd /Applications
xattr -cr Pixolve.app
open Pixolve.app
```

**Das war's!** 🎉
