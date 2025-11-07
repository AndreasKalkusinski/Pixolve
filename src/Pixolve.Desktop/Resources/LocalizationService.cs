using CommunityToolkit.Mvvm.ComponentModel;
using Pixolve.Core.Models;
using System.Collections.Generic;

namespace Pixolve.Desktop.Resources;

/// <summary>
/// Service for managing application localization
/// </summary>
public partial class LocalizationService : ObservableObject
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance => _instance ??= new LocalizationService();

    private AppLanguage _currentLanguage = AppLanguage.German;

    public void SetLanguage(AppLanguage language)
    {
        if (_currentLanguage == language) return;

        _currentLanguage = language;

        // Notify all properties changed
        OnPropertyChanged(string.Empty); // Empty string notifies ALL properties changed
    }

    private string GetString(Dictionary<AppLanguage, string> translations)
    {
        return translations.TryGetValue(_currentLanguage, out var value) ? value : translations[AppLanguage.English];
    }

    // Window Title
    public string AppTitle => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Pixolve - Moderner Bild-Konverter",
        [AppLanguage.English] = "Pixolve - Modern Image Converter",
        [AppLanguage.French] = "Pixolve - Convertisseur d'images moderne",
        [AppLanguage.Spanish] = "Pixolve - Conversor de imágenes moderno",
        [AppLanguage.Italian] = "Pixolve - Convertitore di immagini moderno"
    });

    public string AppSubtitle => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Moderner plattformübergreifender Bild-Konverter",
        [AppLanguage.English] = "Modern cross-platform image converter",
        [AppLanguage.French] = "Convertisseur d'images multiplateforme moderne",
        [AppLanguage.Spanish] = "Conversor de imágenes multiplataforma moderno",
        [AppLanguage.Italian] = "Convertitore di immagini multipiattaforma moderno"
    });

    // Source Selection
    public string SourceFolder => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Quellordner:",
        [AppLanguage.English] = "Source Folder:",
        [AppLanguage.French] = "Dossier source :",
        [AppLanguage.Spanish] = "Carpeta de origen:",
        [AppLanguage.Italian] = "Cartella sorgente:"
    });

    public string SourceFolderPlaceholder => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ordnerpfad eingeben oder durchsuchen...",
        [AppLanguage.English] = "Enter folder path or browse...",
        [AppLanguage.French] = "Entrez le chemin ou parcourez...",
        [AppLanguage.Spanish] = "Ingrese la ruta o navegue...",
        [AppLanguage.Italian] = "Inserisci il percorso o sfoglia..."
    });

    public string Browse => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Durchsuchen",
        [AppLanguage.English] = "Browse",
        [AppLanguage.French] = "Parcourir",
        [AppLanguage.Spanish] = "Examinar",
        [AppLanguage.Italian] = "Sfoglia"
    });

    public string LoadImages => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Bilder laden",
        [AppLanguage.English] = "Load Images",
        [AppLanguage.French] = "Charger les images",
        [AppLanguage.Spanish] = "Cargar imágenes",
        [AppLanguage.Italian] = "Carica immagini"
    });

    // Settings Headers
    public string FormatAndQuality => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "FORMAT & QUALITÄT",
        [AppLanguage.English] = "FORMAT & QUALITY",
        [AppLanguage.French] = "FORMAT & QUALITÉ",
        [AppLanguage.Spanish] = "FORMATO Y CALIDAD",
        [AppLanguage.Italian] = "FORMATO E QUALITÀ"
    });

    public string SizeAndOutput => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "GRÖSSE & AUSGABE",
        [AppLanguage.English] = "SIZE & OUTPUT",
        [AppLanguage.French] = "TAILLE & SORTIE",
        [AppLanguage.Spanish] = "TAMAÑO Y SALIDA",
        [AppLanguage.Italian] = "DIMENSIONE E OUTPUT"
    });

    public string Options => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "OPTIONEN",
        [AppLanguage.English] = "OPTIONS",
        [AppLanguage.French] = "OPTIONS",
        [AppLanguage.Spanish] = "OPCIONES",
        [AppLanguage.Italian] = "OPZIONI"
    });

    public string Appearance => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "DARSTELLUNG",
        [AppLanguage.English] = "APPEARANCE",
        [AppLanguage.French] = "APPARENCE",
        [AppLanguage.Spanish] = "APARIENCIA",
        [AppLanguage.Italian] = "ASPETTO"
    });

    // Settings Labels
    public string Format => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Format:",
        [AppLanguage.English] = "Format:",
        [AppLanguage.French] = "Format :",
        [AppLanguage.Spanish] = "Formato:",
        [AppLanguage.Italian] = "Formato:"
    });

    public string Quality => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Qualität:",
        [AppLanguage.English] = "Quality:",
        [AppLanguage.French] = "Qualité :",
        [AppLanguage.Spanish] = "Calidad:",
        [AppLanguage.Italian] = "Qualità:"
    });

    public string MaxPixel => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Max Pixel:",
        [AppLanguage.English] = "Max Pixel:",
        [AppLanguage.French] = "Pixels max :",
        [AppLanguage.Spanish] = "Píxeles máx:",
        [AppLanguage.Italian] = "Pixel max:"
    });

    public string OutputDirectory => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ausgabeverzeichnis:",
        [AppLanguage.English] = "Output Directory:",
        [AppLanguage.French] = "Répertoire de sortie :",
        [AppLanguage.Spanish] = "Directorio de salida:",
        [AppLanguage.Italian] = "Directory di output:"
    });

    public string Theme => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Theme:",
        [AppLanguage.English] = "Theme:",
        [AppLanguage.French] = "Thème :",
        [AppLanguage.Spanish] = "Tema:",
        [AppLanguage.Italian] = "Tema:"
    });

    public string Language => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Sprache:",
        [AppLanguage.English] = "Language:",
        [AppLanguage.French] = "Langue :",
        [AppLanguage.Spanish] = "Idioma:",
        [AppLanguage.Italian] = "Lingua:"
    });

    // Theme Options
    public string ThemeLight => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Hell",
        [AppLanguage.English] = "Light",
        [AppLanguage.French] = "Clair",
        [AppLanguage.Spanish] = "Claro",
        [AppLanguage.Italian] = "Chiaro"
    });

    public string ThemeDark => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Dunkel",
        [AppLanguage.English] = "Dark",
        [AppLanguage.French] = "Sombre",
        [AppLanguage.Spanish] = "Oscuro",
        [AppLanguage.Italian] = "Scuro"
    });

    public string ThemeSystem => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "System",
        [AppLanguage.English] = "System",
        [AppLanguage.French] = "Système",
        [AppLanguage.Spanish] = "Sistema",
        [AppLanguage.Italian] = "Sistema"
    });

    // Language Options
    public string LanguageGerman => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Deutsch",
        [AppLanguage.English] = "German",
        [AppLanguage.French] = "Allemand",
        [AppLanguage.Spanish] = "Alemán",
        [AppLanguage.Italian] = "Tedesco"
    });

    public string LanguageEnglish => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Englisch",
        [AppLanguage.English] = "English",
        [AppLanguage.French] = "Anglais",
        [AppLanguage.Spanish] = "Inglés",
        [AppLanguage.Italian] = "Inglese"
    });

    public string LanguageFrench => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Französisch",
        [AppLanguage.English] = "French",
        [AppLanguage.French] = "Français",
        [AppLanguage.Spanish] = "Francés",
        [AppLanguage.Italian] = "Francese"
    });

    public string LanguageSpanish => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Spanisch",
        [AppLanguage.English] = "Spanish",
        [AppLanguage.French] = "Espagnol",
        [AppLanguage.Spanish] = "Español",
        [AppLanguage.Italian] = "Spagnolo"
    });

    public string LanguageItalian => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Italienisch",
        [AppLanguage.English] = "Italian",
        [AppLanguage.French] = "Italien",
        [AppLanguage.Spanish] = "Italiano",
        [AppLanguage.Italian] = "Italiano"
    });

    // Checkboxes
    public string EnableResizing => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Bildgröße anpassen",
        [AppLanguage.English] = "Resize images",
        [AppLanguage.French] = "Redimensionner les images",
        [AppLanguage.Spanish] = "Redimensionar imágenes",
        [AppLanguage.Italian] = "Ridimensiona immagini"
    });

    public string OverwriteExisting => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Dateien überschreiben",
        [AppLanguage.English] = "Overwrite existing",
        [AppLanguage.French] = "Écraser existants",
        [AppLanguage.Spanish] = "Sobrescribir existentes",
        [AppLanguage.Italian] = "Sovrascrivi esistenti"
    });

    public string PreserveTimestamp => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Zeitstempel beibehalten",
        [AppLanguage.English] = "Preserve timestamp",
        [AppLanguage.French] = "Conserver horodatage",
        [AppLanguage.Spanish] = "Conservar marca de tiempo",
        [AppLanguage.Italian] = "Preserva timestamp"
    });

    public string IncludeSubfolders => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Unterordner einbeziehen",
        [AppLanguage.English] = "Include subfolders",
        [AppLanguage.French] = "Inclure sous-dossiers",
        [AppLanguage.Spanish] = "Incluir subcarpetas",
        [AppLanguage.Italian] = "Includi sottocartelle"
    });

    // Actions
    public string Actions => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Aktionen",
        [AppLanguage.English] = "Actions",
        [AppLanguage.French] = "Actions",
        [AppLanguage.Spanish] = "Acciones",
        [AppLanguage.Italian] = "Azioni"
    });

    public string ConvertAll => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Alle konvertieren",
        [AppLanguage.English] = "Convert All",
        [AppLanguage.French] = "Tout convertir",
        [AppLanguage.Spanish] = "Convertir todo",
        [AppLanguage.Italian] = "Converti tutto"
    });

    public string Cancel => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Abbrechen",
        [AppLanguage.English] = "Cancel",
        [AppLanguage.French] = "Annuler",
        [AppLanguage.Spanish] = "Cancelar",
        [AppLanguage.Italian] = "Annulla"
    });

    public string ClearList => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Liste leeren",
        [AppLanguage.English] = "Clear List",
        [AppLanguage.French] = "Vider la liste",
        [AppLanguage.Spanish] = "Limpiar lista",
        [AppLanguage.Italian] = "Svuota lista"
    });

    public string MultiFormatExport => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Multi-Format Export",
        [AppLanguage.English] = "Multi-Format Export",
        [AppLanguage.French] = "Export multi-format",
        [AppLanguage.Spanish] = "Exportación multiformato",
        [AppLanguage.Italian] = "Esportazione multiformato"
    });

    // DataGrid Headers
    public string Preview => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Vorschau",
        [AppLanguage.English] = "Preview",
        [AppLanguage.French] = "Aperçu",
        [AppLanguage.Spanish] = "Vista previa",
        [AppLanguage.Italian] = "Anteprima"
    });

    public string FileName => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Dateiname",
        [AppLanguage.English] = "File Name",
        [AppLanguage.French] = "Nom de fichier",
        [AppLanguage.Spanish] = "Nombre de archivo",
        [AppLanguage.Italian] = "Nome file"
    });

    public string Status => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Status",
        [AppLanguage.English] = "Status",
        [AppLanguage.French] = "Statut",
        [AppLanguage.Spanish] = "Estado",
        [AppLanguage.Italian] = "Stato"
    });

    public string StatusReady => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Bereit",
        [AppLanguage.English] = "Ready",
        [AppLanguage.French] = "Prêt",
        [AppLanguage.Spanish] = "Listo",
        [AppLanguage.Italian] = "Pronto"
    });

    // DataGrid Additional Headers
    public string Before => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Vorher",
        [AppLanguage.English] = "Before",
        [AppLanguage.French] = "Avant",
        [AppLanguage.Spanish] = "Antes",
        [AppLanguage.Italian] = "Prima"
    });

    public string After => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Nachher",
        [AppLanguage.English] = "After",
        [AppLanguage.French] = "Après",
        [AppLanguage.Spanish] = "Después",
        [AppLanguage.Italian] = "Dopo"
    });

    public string Pixel => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Pixel",
        [AppLanguage.English] = "Pixel",
        [AppLanguage.French] = "Pixels",
        [AppLanguage.Spanish] = "Píxeles",
        [AppLanguage.Italian] = "Pixel"
    });

    // Per-Image Custom Settings
    public string CustomSettings => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Individuelle Einstellungen",
        [AppLanguage.English] = "Custom Settings",
        [AppLanguage.French] = "Paramètres personnalisés",
        [AppLanguage.Spanish] = "Configuración personalizada",
        [AppLanguage.Italian] = "Impostazioni personalizzate"
    });

    public string FormatEmptyGlobal => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Format (leer = global):",
        [AppLanguage.English] = "Format (empty = global):",
        [AppLanguage.French] = "Format (vide = global) :",
        [AppLanguage.Spanish] = "Formato (vacío = global):",
        [AppLanguage.Italian] = "Formato (vuoto = globale):"
    });

    public string QualityEmptyGlobal => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Qualität (leer = global):",
        [AppLanguage.English] = "Quality (empty = global):",
        [AppLanguage.French] = "Qualité (vide = global) :",
        [AppLanguage.Spanish] = "Calidad (vacío = global):",
        [AppLanguage.Italian] = "Qualità (vuoto = globale):"
    });

    public string MaxPixelEmptyGlobal => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Max Pixel (leer = global):",
        [AppLanguage.English] = "Max Pixel (empty = global):",
        [AppLanguage.French] = "Pixels max (vide = global) :",
        [AppLanguage.Spanish] = "Píxeles máx (vacío = global):",
        [AppLanguage.Italian] = "Pixel max (vuoto = globale):"
    });

    public string Global => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Global",
        [AppLanguage.English] = "Global",
        [AppLanguage.French] = "Global",
        [AppLanguage.Spanish] = "Global",
        [AppLanguage.Italian] = "Globale"
    });

    public string CustomSettingsNote => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Hinweis: Leere Felder verwenden die globalen Einstellungen.",
        [AppLanguage.English] = "Note: Empty fields use global settings.",
        [AppLanguage.French] = "Note : Les champs vides utilisent les paramètres globaux.",
        [AppLanguage.Spanish] = "Nota: Los campos vacíos usan la configuración global.",
        [AppLanguage.Italian] = "Nota: I campi vuoti usano le impostazioni globali."
    });

    // Watermarks
    public string QualityWatermark => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Qualität",
        [AppLanguage.English] = "Quality",
        [AppLanguage.French] = "Qualité",
        [AppLanguage.Spanish] = "Calidad",
        [AppLanguage.Italian] = "Qualità"
    });

    public string OutputDirectoryWatermark => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "'converted'",
        [AppLanguage.English] = "'converted'",
        [AppLanguage.French] = "'converti'",
        [AppLanguage.Spanish] = "'convertido'",
        [AppLanguage.Italian] = "'convertito'"
    });

    // Status Bar
    public string StatusLabel => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Status:",
        [AppLanguage.English] = "Status:",
        [AppLanguage.French] = "Statut :",
        [AppLanguage.Spanish] = "Estado:",
        [AppLanguage.Italian] = "Stato:"
    });

    public string ImagesCount => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Bilder:",
        [AppLanguage.English] = "Images:",
        [AppLanguage.French] = "Images :",
        [AppLanguage.Spanish] = "Imágenes:",
        [AppLanguage.Italian] = "Immagini:"
    });

    // Status Messages
    public string StatusStorageNotInitialized => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Speicheranbieter nicht initialisiert",
        [AppLanguage.English] = "Storage provider not initialized",
        [AppLanguage.French] = "Fournisseur de stockage non initialisé",
        [AppLanguage.Spanish] = "Proveedor de almacenamiento no inicializado",
        [AppLanguage.Italian] = "Provider di archiviazione non inizializzato"
    });

    public string StatusFolderSelected => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ordner ausgewählt: {0}",
        [AppLanguage.English] = "Folder selected: {0}",
        [AppLanguage.French] = "Dossier sélectionné : {0}",
        [AppLanguage.Spanish] = "Carpeta seleccionada: {0}",
        [AppLanguage.Italian] = "Cartella selezionata: {0}"
    });

    public string StatusFolderSelectionError => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Fehler bei der Ordnerauswahl: {0}",
        [AppLanguage.English] = "Folder selection error: {0}",
        [AppLanguage.French] = "Erreur de sélection de dossier : {0}",
        [AppLanguage.Spanish] = "Error de selección de carpeta: {0}",
        [AppLanguage.Italian] = "Errore di selezione cartella: {0}"
    });

    public string StatusOutputDirectorySelected => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ausgabeverzeichnis ausgewählt: {0}",
        [AppLanguage.English] = "Output directory selected: {0}",
        [AppLanguage.French] = "Répertoire de sortie sélectionné : {0}",
        [AppLanguage.Spanish] = "Directorio de salida seleccionado: {0}",
        [AppLanguage.Italian] = "Directory di output selezionata: {0}"
    });

    public string StatusSelectValidFolder => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Bitte einen gültigen Quellordner auswählen",
        [AppLanguage.English] = "Please select a valid source folder",
        [AppLanguage.French] = "Veuillez sélectionner un dossier source valide",
        [AppLanguage.Spanish] = "Por favor seleccione una carpeta de origen válida",
        [AppLanguage.Italian] = "Selezionare una cartella di origine valida"
    });

    public string StatusLoadingImages => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Lade Bilder...",
        [AppLanguage.English] = "Loading images...",
        [AppLanguage.French] = "Chargement des images...",
        [AppLanguage.Spanish] = "Cargando imágenes...",
        [AppLanguage.Italian] = "Caricamento immagini..."
    });

    public string StatusImagesLoaded => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "{0} Bilder geladen",
        [AppLanguage.English] = "{0} images loaded",
        [AppLanguage.French] = "{0} images chargées",
        [AppLanguage.Spanish] = "{0} imágenes cargadas",
        [AppLanguage.Italian] = "{0} immagini caricate"
    });

    public string StatusLoadImagesError => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Fehler beim Laden der Bilder: {0}",
        [AppLanguage.English] = "Error loading images: {0}",
        [AppLanguage.French] = "Erreur de chargement des images : {0}",
        [AppLanguage.Spanish] = "Error al cargar imágenes: {0}",
        [AppLanguage.Italian] = "Errore nel caricamento delle immagini: {0}"
    });

    public string StatusNoImagesToConvert => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Keine Bilder zum Konvertieren vorhanden",
        [AppLanguage.English] = "No images to convert",
        [AppLanguage.French] = "Aucune image à convertir",
        [AppLanguage.Spanish] = "No hay imágenes para convertir",
        [AppLanguage.Italian] = "Nessuna immagine da convertire"
    });

    public string StatusInvalidSettings => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ungültige Einstellungen: {0}",
        [AppLanguage.English] = "Invalid settings: {0}",
        [AppLanguage.French] = "Paramètres invalides : {0}",
        [AppLanguage.Spanish] = "Configuración no válida: {0}",
        [AppLanguage.Italian] = "Impostazioni non valide: {0}"
    });

    public string StatusConversionCancelled => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Konvertierung abgebrochen",
        [AppLanguage.English] = "Conversion cancelled",
        [AppLanguage.French] = "Conversion annulée",
        [AppLanguage.Spanish] = "Conversión cancelada",
        [AppLanguage.Italian] = "Conversione annullata"
    });

    public string StatusConverting => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Konvertiere {0}...",
        [AppLanguage.English] = "Converting {0}...",
        [AppLanguage.French] = "Conversion de {0}...",
        [AppLanguage.Spanish] = "Convirtiendo {0}...",
        [AppLanguage.Italian] = "Conversione di {0}..."
    });

    public string StatusConversionComplete => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Konvertierung abgeschlossen: {0} erfolgreich, {1} fehlgeschlagen",
        [AppLanguage.English] = "Conversion complete: {0} successful, {1} failed",
        [AppLanguage.French] = "Conversion terminée : {0} réussies, {1} échouées",
        [AppLanguage.Spanish] = "Conversión completada: {0} exitosas, {1} fallidas",
        [AppLanguage.Italian] = "Conversione completata: {0} riuscite, {1} fallite"
    });

    public string StatusConversionError => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Fehler bei der Konvertierung: {0}",
        [AppLanguage.English] = "Conversion error: {0}",
        [AppLanguage.French] = "Erreur de conversion : {0}",
        [AppLanguage.Spanish] = "Error de conversión: {0}",
        [AppLanguage.Italian] = "Errore di conversione: {0}"
    });

    public string StatusCancelling => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Konvertierung wird abgebrochen...",
        [AppLanguage.English] = "Cancelling conversion...",
        [AppLanguage.French] = "Annulation de la conversion...",
        [AppLanguage.Spanish] = "Cancelando conversión...",
        [AppLanguage.Italian] = "Annullamento conversione..."
    });

    public string StatusFileRemoved => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Datei entfernt: {0}",
        [AppLanguage.English] = "File removed: {0}",
        [AppLanguage.French] = "Fichier supprimé : {0}",
        [AppLanguage.Spanish] = "Archivo eliminado: {0}",
        [AppLanguage.Italian] = "File rimosso: {0}"
    });

    public string StatusFolderDropped => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Ordner per Drag & Drop ausgewählt: {0}",
        [AppLanguage.English] = "Folder selected via drag & drop: {0}",
        [AppLanguage.French] = "Dossier sélectionné par glisser-déposer : {0}",
        [AppLanguage.Spanish] = "Carpeta seleccionada mediante arrastrar y soltar: {0}",
        [AppLanguage.Italian] = "Cartella selezionata tramite drag & drop: {0}"
    });

    // Image Status in DataGrid
    public string ImageStatusPending => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Warten",
        [AppLanguage.English] = "Pending",
        [AppLanguage.French] = "En attente",
        [AppLanguage.Spanish] = "Pendiente",
        [AppLanguage.Italian] = "In attesa"
    });

    public string ImageStatusCancelled => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Abgebrochen",
        [AppLanguage.English] = "Cancelled",
        [AppLanguage.French] = "Annulé",
        [AppLanguage.Spanish] = "Cancelado",
        [AppLanguage.Italian] = "Annullato"
    });

    public string ImageStatusConverting => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Konvertierung läuft...",
        [AppLanguage.English] = "Converting...",
        [AppLanguage.French] = "Conversion...",
        [AppLanguage.Spanish] = "Convirtiendo...",
        [AppLanguage.Italian] = "Conversione..."
    });

    public string ImageStatusSuccess => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Erfolgreich",
        [AppLanguage.English] = "Successful",
        [AppLanguage.French] = "Réussi",
        [AppLanguage.Spanish] = "Exitoso",
        [AppLanguage.Italian] = "Riuscito"
    });

    public string ImageStatusSuccessMulti => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Erfolgreich ({0} Formate)",
        [AppLanguage.English] = "Successful ({0} formats)",
        [AppLanguage.French] = "Réussi ({0} formats)",
        [AppLanguage.Spanish] = "Exitoso ({0} formatos)",
        [AppLanguage.Italian] = "Riuscito ({0} formati)"
    });

    public string ImageStatusPartial => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Teilweise ({0}/{1})",
        [AppLanguage.English] = "Partial ({0}/{1})",
        [AppLanguage.French] = "Partiel ({0}/{1})",
        [AppLanguage.Spanish] = "Parcial ({0}/{1})",
        [AppLanguage.Italian] = "Parziale ({0}/{1})"
    });

    public string ImageStatusError => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Fehler",
        [AppLanguage.English] = "Error",
        [AppLanguage.French] = "Erreur",
        [AppLanguage.Spanish] = "Error",
        [AppLanguage.Italian] = "Errore"
    });

    public string ImageStatusErrorWithMessage => GetString(new Dictionary<AppLanguage, string>
    {
        [AppLanguage.German] = "Fehler: {0}",
        [AppLanguage.English] = "Error: {0}",
        [AppLanguage.French] = "Erreur : {0}",
        [AppLanguage.Spanish] = "Error: {0}",
        [AppLanguage.Italian] = "Errore: {0}"
    });
}
