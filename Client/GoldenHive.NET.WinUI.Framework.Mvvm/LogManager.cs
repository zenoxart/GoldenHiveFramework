// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : LogManager.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Application log. Equivalent of ProtokollManager in WIFI.Anwendung.Datenbank.
//
// CONTENTS
//   - Entries: observable list of log entries (time, type, text, source object),
//     bindable for a log view; changes are marshalled onto AppContext.Dispatcher.
//   - Entry types: Information, Warning, Error, NewInstance, Start, End.
//   - Write(text, type) and Write(entry).
//   - Automatic compaction: keep the last N entries in memory (configurable).
//   - Optional file sink in LocalDataPath\Logs (one file per day) and forwarding of
//     errors to NotificationPoint once the NotificationClient is connected.
//
// RULE
//   The LogManager itself is produced without writing a "produced" entry for itself
//   (avoids recursion, as in WIFI).
