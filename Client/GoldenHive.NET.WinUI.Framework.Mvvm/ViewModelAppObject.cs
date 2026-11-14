// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Mvvm
// File    : ViewModelAppObject.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Base class of all ViewModels in the host and in modules.
//   Equivalent of ViewModelAppObjekt in WIFI.Anwendung.Datenbank.
//
// BASE
//   AppObject + INotifyPropertyChanged (self-written, no MVVM library).
//
// MEMBERS (described)
//   - PropertyChanged event.
//   - OnPropertyChanged([CallerMemberName] propertyName): copy the handler, raise the event.
//     WinUI requires this on the UI thread: if called from a background thread, enqueue it
//     on AppContext.Dispatcher (WPF tolerated cross-thread PropertyChanged, WinUI does not).
//   - SetProperty(ref field, value, [CallerMemberName]) helper: compare, assign, notify;
//     returns whether the value changed.
//   - Typed context: shadow AppContext with the derived context type the ViewModel needs
//     (e.g. ClientAppContext), so callers do not have to cast (same trick as WIFI).
//   - IsBusy: counter-based busy flag (several overlapping operations keep it true until
//     the last one ends).
//   - BeginWork([CallerMemberName] methodName) / EndWork(...): increment/decrement IsBusy and
//     write "<ViewModel>.<method> starts/ends" to the log (WIFI: StartProtokollieren /
//     EndeProtokollieren). Always paired via try/finally.
//   - Lifecycle hooks: OnNavigatedTo, OnNavigatedFrom, OnClosing.
//
// COMMANDS
//   ViewModels do not create command objects; they expose commands from the central
//   command manager: AppContext.Commands.Get("<module>.<action>").
//
// HOT-SWAP
//   ViewModels that must keep their state implement IStatefulViewModel.
//
// RESPONSIVE
//   - Implements IResponsiveView: SizeClass / HeightClass are pushed in by the hosting frame or
//     window (ResponsiveLayoutManager) and raise PropertyChanged, so bindings and visual states
//     can switch layouts (e.g. list + detail side by side in Expanded, stacked in Compact).
//   - ViewModels never assume a window or screen size.
