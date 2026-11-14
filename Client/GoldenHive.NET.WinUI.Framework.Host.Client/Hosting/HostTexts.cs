using GoldenHive.NET.WinUI.Framework.Mvvm;

namespace GoldenHive.NET.WinUI.Framework.Host.Client.Hosting;

/// <summary>Texts of the shell (scope "shell") in English and German.</summary>
internal static class HostTexts
{
    public static void Register(LanguageManager languages)
    {
        languages.SetSupported([new LanguageInfo("en", "English"), new LanguageInfo("de", "Deutsch")]);
        languages.DefaultLanguage = "en";
        languages.Register("shell", "en", English);
        languages.Register("shell", "de", German);
    }

    private static readonly Dictionary<string, string> English = new()
    {
        ["app.title"] = "Golden Hive",
        ["menu.file.label"] = "File",
        ["menu.view.label"] = "View",
        ["menu.window.label"] = "Window",
        ["menu.help.label"] = "Help",
        ["group.general.label"] = "General",
        ["group.demo.label"] = "Demo modules",

        ["command.exit.label"] = "Exit",
        ["command.exit.tooltip"] = "Close Golden Hive",
        ["command.palette.label"] = "Command palette",
        ["command.palette.tooltip"] = "Search and run any command",
        ["command.wheel.label"] = "Navigation wheel",
        ["command.wheel.tooltip"] = "Switch between modules and open documents (hold Ctrl)",
        ["command.detach.label"] = "Float view",
        ["command.detach.tooltip"] = "Move the active view into its own window",
        ["command.attach.label"] = "Dock view",
        ["command.attach.tooltip"] = "Dock the floating view back into the main window",
        ["command.closedocument.label"] = "Close document",
        ["command.nextdocument.label"] = "Next document",
        ["command.previousdocument.label"] = "Previous document",
        ["command.toggleleft.label"] = "Left pane",
        ["command.toggleleft.tooltip"] = "Show or hide the left tool window pane",
        ["command.toggleright.label"] = "Right pane",
        ["command.toggleright.tooltip"] = "Show or hide the right tool window pane",
        ["command.togglebottom.label"] = "Bottom pane",
        ["command.togglebottom.tooltip"] = "Show or hide the bottom tool window pane",
        ["command.resetlayout.label"] = "Reset window layout",
        ["command.resetlayout.tooltip"] = "Put all tool windows back to their default places",
        ["command.language.label"] = "Switch language",
        ["command.language.tooltip"] = "Switch between English and German",
        ["command.about.label"] = "About Golden Hive",

        ["view.start.label"] = "Start page",
        ["view.start.tooltip"] = "Overview, modules and quick actions",
        ["view.settings.label"] = "Settings",
        ["view.settings.tooltip"] = "Language, keyboard shortcuts and data folders",
        ["view.moduleexplorer.label"] = "Module Explorer",
        ["view.moduleexplorer.tooltip"] = "All modules and their views",
        ["view.output.label"] = "Output",
        ["view.output.tooltip"] = "The application log",
        ["demo.documents.label"] = "Documents (demo)",
        ["demo.documents.description"] = "Built-in example module to try navigation, docking and floating windows.",
        ["demo.reports.label"] = "Reports (demo)",
        ["demo.reports.description"] = "A second example module, so the navigation wheel has something to switch to.",

        ["key.ctrl"] = "Ctrl",
        ["key.shift"] = "Shift",
        ["key.alt"] = "Alt",
        ["key.win"] = "Win",

        ["status.ready"] = "Ready",
        ["status.chord"] = "{gesture} was pressed. Waiting for the second key...",
        ["sizeclass.compact"] = "Compact",
        ["sizeclass.medium"] = "Medium",
        ["sizeclass.expanded"] = "Expanded",
        ["sizeclass.wide"] = "Wide",

        ["wheel.modules"] = "Module",
        ["wheel.documents"] = "Open view",
        ["wheel.hint"] = "Tab: next  ·  Arrows: move / switch ring  ·  Release Ctrl: open  ·  Esc: cancel",

        ["dock.empty"] = "No document open.\nPress Ctrl+Shift+P to run a command or hold Ctrl+Tab to switch modules.",
        ["dock.float"] = "Float",
        ["dock.close"] = "Close",
        ["dock.closeothers"] = "Close other documents",
        ["dock.dockback"] = "Dock back",
        ["dock.hidepane"] = "Hide pane",

        ["palette.placeholder"] = "Search commands (Ctrl+Shift+P)",

        ["start.welcome"] = "Welcome to Golden Hive",
        ["start.subtitle"] = "Modular WinUI 3 framework - stage 1: the shell",
        ["start.quickactions"] = "Quick actions",
        ["start.modules"] = "Modules",
        ["start.tips"] = "Tips",
        ["start.tip.wheel"] = "Hold Ctrl+Tab to switch between modules and open documents.",
        ["start.tip.palette"] = "Press Ctrl+Shift+P to find any command.",
        ["start.tip.float"] = "Drag a tab out of the window or press Ctrl+Shift+D to float it; it returns to the same monitor next time.",
        ["start.tip.responsive"] = "Make the window narrow: tool windows turn into overlays.",

        ["settings.language"] = "Language",
        ["settings.shortcuts"] = "Keyboard shortcuts",
        ["settings.shortcuts.hint"] = "Click 'Change' and press the new key combination. Esc cancels.",
        ["settings.change"] = "Change",
        ["settings.reset"] = "Reset",
        ["settings.resetall"] = "Reset all shortcuts",
        ["settings.recording"] = "Press keys...",
        ["settings.none"] = "-",
        ["settings.data"] = "Data folders",
        ["settings.localdata"] = "Local data (layouts, logs)",
        ["settings.roamingdata"] = "User data (settings, shortcuts)",
        ["settings.open"] = "Open",

        ["output.clear"] = "Clear",
        ["output.autoscroll"] = "Auto-scroll",

        ["about.title"] = "About Golden Hive",
        ["about.text"] = "Modular, server-driven WinUI 3 framework.\nVersion {version} · .NET 10 · Windows App SDK",
        ["about.close"] = "Close",

        ["demo.view.text"] = "This view is a built-in example. Try floating it (Ctrl+Shift+D), resizing the window or switching with Ctrl+Tab.",
        ["demo.view.items"] = "Sample entries",
    };

    private static readonly Dictionary<string, string> German = new()
    {
        ["app.title"] = "Golden Hive",
        ["menu.file.label"] = "Datei",
        ["menu.view.label"] = "Ansicht",
        ["menu.window.label"] = "Fenster",
        ["menu.help.label"] = "Hilfe",
        ["group.general.label"] = "Allgemein",
        ["group.demo.label"] = "Demo-Module",

        ["command.exit.label"] = "Beenden",
        ["command.exit.tooltip"] = "Golden Hive schließen",
        ["command.palette.label"] = "Befehlspalette",
        ["command.palette.tooltip"] = "Jeden Befehl suchen und ausführen",
        ["command.wheel.label"] = "Navigationsrad",
        ["command.wheel.tooltip"] = "Zwischen Modulen und offenen Dokumenten wechseln (Strg gedrückt halten)",
        ["command.detach.label"] = "Ansicht abdocken",
        ["command.detach.tooltip"] = "Die aktive Ansicht in ein eigenes Fenster verschieben",
        ["command.attach.label"] = "Ansicht andocken",
        ["command.attach.tooltip"] = "Die schwebende Ansicht wieder ins Hauptfenster andocken",
        ["command.closedocument.label"] = "Dokument schließen",
        ["command.nextdocument.label"] = "Nächstes Dokument",
        ["command.previousdocument.label"] = "Vorheriges Dokument",
        ["command.toggleleft.label"] = "Linker Bereich",
        ["command.toggleleft.tooltip"] = "Linken Toolfensterbereich ein- oder ausblenden",
        ["command.toggleright.label"] = "Rechter Bereich",
        ["command.toggleright.tooltip"] = "Rechten Toolfensterbereich ein- oder ausblenden",
        ["command.togglebottom.label"] = "Unterer Bereich",
        ["command.togglebottom.tooltip"] = "Unteren Toolfensterbereich ein- oder ausblenden",
        ["command.resetlayout.label"] = "Fensterlayout zurücksetzen",
        ["command.resetlayout.tooltip"] = "Alle Toolfenster an ihren Standardplatz zurücksetzen",
        ["command.language.label"] = "Sprache wechseln",
        ["command.language.tooltip"] = "Zwischen Deutsch und Englisch wechseln",
        ["command.about.label"] = "Über Golden Hive",

        ["view.start.label"] = "Startseite",
        ["view.start.tooltip"] = "Übersicht, Module und Schnellzugriff",
        ["view.settings.label"] = "Einstellungen",
        ["view.settings.tooltip"] = "Sprache, Tastenkombinationen und Datenordner",
        ["view.moduleexplorer.label"] = "Modul-Explorer",
        ["view.moduleexplorer.tooltip"] = "Alle Module und ihre Ansichten",
        ["view.output.label"] = "Ausgabe",
        ["view.output.tooltip"] = "Das Anwendungsprotokoll",
        ["demo.documents.label"] = "Dokumente (Demo)",
        ["demo.documents.description"] = "Eingebautes Beispielmodul zum Ausprobieren von Navigation, Andocken und schwebenden Fenstern.",
        ["demo.reports.label"] = "Berichte (Demo)",
        ["demo.reports.description"] = "Ein zweites Beispielmodul, damit das Navigationsrad etwas zum Wechseln hat.",

        ["key.ctrl"] = "Strg",
        ["key.shift"] = "Umschalt",
        ["key.alt"] = "Alt",
        ["key.win"] = "Win",

        ["status.ready"] = "Bereit",
        ["status.chord"] = "{gesture} wurde gedrückt. Warte auf die zweite Taste...",
        ["sizeclass.compact"] = "Kompakt",
        ["sizeclass.medium"] = "Mittel",
        ["sizeclass.expanded"] = "Erweitert",
        ["sizeclass.wide"] = "Breit",

        ["wheel.modules"] = "Modul",
        ["wheel.documents"] = "Offene Ansicht",
        ["wheel.hint"] = "Tab: weiter  ·  Pfeile: bewegen / Ring wechseln  ·  Strg loslassen: öffnen  ·  Esc: abbrechen",

        ["dock.empty"] = "Kein Dokument geöffnet.\nStrg+Umschalt+P öffnet die Befehlspalette, Strg+Tab gedrückt halten wechselt das Modul.",
        ["dock.float"] = "Abdocken",
        ["dock.close"] = "Schließen",
        ["dock.closeothers"] = "Andere Dokumente schließen",
        ["dock.dockback"] = "Wieder andocken",
        ["dock.hidepane"] = "Bereich ausblenden",

        ["palette.placeholder"] = "Befehle suchen (Strg+Umschalt+P)",

        ["start.welcome"] = "Willkommen bei Golden Hive",
        ["start.subtitle"] = "Modulares WinUI-3-Framework - Stufe 1: die Shell",
        ["start.quickactions"] = "Schnellzugriff",
        ["start.modules"] = "Module",
        ["start.tips"] = "Tipps",
        ["start.tip.wheel"] = "Strg+Tab gedrückt halten, um zwischen Modulen und offenen Dokumenten zu wechseln.",
        ["start.tip.palette"] = "Strg+Umschalt+P findet jeden Befehl.",
        ["start.tip.float"] = "Ziehe einen Tab aus dem Fenster oder drücke Strg+Umschalt+D zum Abdocken - beim nächsten Start erscheint er wieder auf demselben Bildschirm.",
        ["start.tip.responsive"] = "Mach das Fenster schmal: Toolfenster werden zu Overlays.",

        ["settings.language"] = "Sprache",
        ["settings.shortcuts"] = "Tastenkombinationen",
        ["settings.shortcuts.hint"] = "Auf 'Ändern' klicken und die neue Tastenkombination drücken. Esc bricht ab.",
        ["settings.change"] = "Ändern",
        ["settings.reset"] = "Zurücksetzen",
        ["settings.resetall"] = "Alle Tastenkombinationen zurücksetzen",
        ["settings.recording"] = "Tasten drücken...",
        ["settings.none"] = "-",
        ["settings.data"] = "Datenordner",
        ["settings.localdata"] = "Lokale Daten (Layouts, Protokolle)",
        ["settings.roamingdata"] = "Benutzerdaten (Einstellungen, Tastenkombinationen)",
        ["settings.open"] = "Öffnen",

        ["output.clear"] = "Leeren",
        ["output.autoscroll"] = "Automatisch scrollen",

        ["about.title"] = "Über Golden Hive",
        ["about.text"] = "Modulares, servergesteuertes WinUI-3-Framework.\nVersion {version} · .NET 10 · Windows App SDK",
        ["about.close"] = "Schließen",

        ["demo.view.text"] = "Diese Ansicht ist ein eingebautes Beispiel. Probier Abdocken (Strg+Umschalt+D), Fenstergröße ändern oder Wechseln mit Strg+Tab.",
        ["demo.view.items"] = "Beispieleinträge",
    };
}
