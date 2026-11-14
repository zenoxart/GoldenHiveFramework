// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : JsonController.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Typed service to save and load a data list as JSON.
//   Equivalent of XmlController<T> in WIFI.Anwendung (JSON instead of XML).
//
// MEMBERS (described)
//   - Save(T data, string toPath): serialize with System.Text.Json; write to a temporary
//     file first, then replace the target (no half-written files).
//   - Load(string fromPath): deserialize; throws if the file is missing or invalid.
//
// USAGE
//   Produced by managers that persist data (WindowManager, LanguageManager, settings);
//   callers catch exceptions and report them via OnErrorOccurred
//   (e.g. "can be ignored for a new user" when the file does not exist yet).
//
// WINUI NOTE
//   Use source-generated JsonSerializerContext types so serialization stays reflection-free.
