// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Commands
// File    : ShortcutInputRouter.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Connects the ShortcutManager to the keyboard input of each WinUI window.
//
// BEHAVIOUR
//   - Hooks PreviewKeyDown / PreviewKeyUp of each window's root element (registered by the
//     WindowManager), so shortcuts work regardless of which control has focus and controls
//     cannot swallow reserved gestures.
//   - Determines the active scopes from focus, the current page/workflow step and the module.
//   - Single gestures: look up and execute through the CommandExecutor; mark the event handled
//     only if a command actually ran.
//   - Chords: after the first gesture, wait for the second (status hint "Ctrl+K was pressed,
//     waiting for second key"); timeout or Esc cancels.
//   - Hold gestures: key-down starts the action, release of the modifier ends it
//     (used by the navigation wheel); the modifier state is double-checked with
//     InputKeyboardSource.GetKeyStateForCurrentThread when the window regains focus.
//   - Additionally generates KeyboardAccelerators (with KeyboardAcceleratorTextOverride) on
//     menu items, so WinUI shows the shortcut text and Narrator announces it.
//   - Runs all UI work through the command pool's dispatcher.
