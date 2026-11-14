// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : AppObject.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Abstract base class of all application objects (services, managers, controllers).
//   Equivalent of AppObjekt in WIFI.Anwendung.
//
// MEMBERS (described)
//   - AppContext: the ApplicationContext (from IAppContext).
//   - ErrorOccurred event (ErrorOccurredEventArgs with the causing exception).
//   - OnErrorOccurred(e), protected virtual:
//       * copy the handler before invoking it (thread safety),
//       * if a handler is attached, raise the event,
//       * if no handler is attached: in DEBUG builds rethrow the cause, so errors always
//         surface during development (WIFI behaviour of 2020-12-03); in RELEASE builds
//         the object relies on the log handler attached by Produce<T>().
//
// USAGE
//   - Services catch exceptions at their public boundary and report them through
//     OnErrorOccurred instead of letting them escape into the UI.
//   - Subclasses expose typed contexts by shadowing AppContext (see ViewModelAppObject).
