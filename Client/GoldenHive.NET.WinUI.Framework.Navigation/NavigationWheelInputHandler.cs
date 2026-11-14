// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Navigation
// File    : NavigationWheelInputHandler.cs
// Layer   : Client / Framework
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Keyboard, mouse, pen, touch and gamepad handling for the navigation wheel.
//
// KEYBOARD (Alt+Tab semantics)
//   - Ctrl+Tab pressed      -> open the wheel, preselect the previous module (MRU index 1).
//   - Tab while Ctrl held    -> next segment; Shift+Tab -> previous segment.
//   - Arrow keys             -> move around the ring (left/right) and between rings (up/down,
//                               or Right/Left = drill into / out of a module's outer ring).
//   - Ctrl released          -> Confirm(): navigate to the selected segment and close.
//   - Esc                    -> Cancel(); focus returns to where it was.
//   - Enter                  -> Confirm() (for users who hold Ctrl with the other hand).
//   - Quick tap (released before the wheel's show delay of ~150 ms) -> switch to the previous
//     module without showing the wheel at all.
//
// DETECTION IN WINUI 3
//   - Registered as the command "shell.navigationwheel.show" with the accelerators Ctrl+Tab and
//     Ctrl+Shift+Tab in the command manager's shortcut table; Ctrl+Tab is reserved for the shell
//     and cannot be claimed by modules (conflict detection).
//   - Handled on the window's root element via PreviewKeyDown / PreviewKeyUp so controls with
//     their own Ctrl+Tab handling (e.g. TabView) do not swallow it; the event is marked handled.
//   - Release of Ctrl is detected in PreviewKeyUp and verified with
//     InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control), so a missed key-up
//     (e.g. focus moved to another app) does not leave the wheel stuck open.
//   - Window deactivated -> Cancel().
//
// POINTER AND OTHER INPUT
//   - Mouse/pen: the segment under the pointer direction from the centre is highlighted;
//     click = Confirm. Touch: tap a segment; tap outside = Cancel.
//   - Gamepad: hold View button, select with the left stick direction, release to confirm.
//
// SHORTCUT MANAGER
//   The wheel's Ctrl+Tab is registered with the ShortcutManager as a reserved hold gesture;
//   the user may remap it in the shortcut settings, modules may not.
