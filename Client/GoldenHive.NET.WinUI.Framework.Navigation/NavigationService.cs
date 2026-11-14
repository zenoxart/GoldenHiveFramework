// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : NavigationService.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Navigates within the shell's content frame.
//
// BEHAVIOUR
//   - NavigateTo(navigationKey, parameter): look up the target in NavigationRegistry,
//     check permission, create the ViewModel, create the view via ViewLocator.
//   - Set the frame content; back/forward via its own history (key + parameter),
//     not via types from old module versions.
//   - During a hot-swap: remove or rewrite all history entries of a module.
//
// NAVIGATION WHEEL
//   - Records each navigation in the ModuleActivationHistory.
//   - Executes the wheel's selection like any other navigation command.
//
// DETACHED VIEWS
//   - Navigation can target the main frame or a detached window; navigating to a view that is
//     already detached brings its window to front instead of opening it twice.
//   - The navigation history of the main window does not contain detached views.
//
// DOCK LAYOUT
//   - Navigating opens the target through the DockManager: documents as tabs in the document well,
//     tool windows at their dock position; an already open view is activated instead of reopened.
//   - Back/forward history spans documents (Ctrl+- / Ctrl+Shift+-), like Visual Studio.
