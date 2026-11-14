// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : IAppContext.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Marks an object that works with the application infrastructure.
//   Equivalent of IAppKontext in WIFI.Anwendung.
//
// MEMBERS (described)
//   - AppContext: gets or sets the ApplicationContext the object belongs to.
//
// RULES
//   - Set exactly once, by ApplicationContext.Produce<T>(), right after construction.
//   - Objects never create other application objects with "new"; they ask their
//     AppContext to produce them, so every object is wired up the same way.
