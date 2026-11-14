using GoldenHive.NET.WinUI.Framework.CommandContracts;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using VirtualKey = Windows.System.VirtualKey;
using Windows.UI.Core;

namespace GoldenHive.NET.WinUI.Framework.Commands;

/// <summary>
/// Connects the ShortcutManager to the keyboard input of each window: handles PreviewKeyDown on the
/// window root (so controls cannot swallow shell shortcuts), resolves single gestures and two-step chords,
/// and never fires plain keys while a text field has focus.
/// </summary>
public sealed class ShortcutInputRouter : AppObject
{
    private static readonly TimeSpan ChordTimeout = TimeSpan.FromSeconds(1.5);
    private KeyGesture? _pendingChord;
    private DateTimeOffset _pendingSince;

    /// <summary>Raised when the first step of a chord was pressed (text to show) or the chord ended (null).</summary>
    public event EventHandler<string?>? ChordPending;

    /// <summary>While true, the router ignores input (e.g. the navigation wheel or shortcut recording is active).</summary>
    public bool IsSuspended { get; set; }

    public void Attach(UIElement root) => root.PreviewKeyDown += OnPreviewKeyDown;

    public void Detach(UIElement root) => root.PreviewKeyDown -= OnPreviewKeyDown;

    /// <summary>True if the key is currently held down.</summary>
    public static bool IsDown(VirtualKey key) =>
        InputKeyboardSource.GetKeyStateForCurrentThread(key).HasFlag(CoreVirtualKeyStates.Down);

    /// <summary>The modifier keys currently held down.</summary>
    public static KeyModifiers CurrentModifiers()
    {
        var modifiers = KeyModifiers.None;
        if (IsDown(VirtualKey.Control)) modifiers |= KeyModifiers.Ctrl;
        if (IsDown(VirtualKey.Shift)) modifiers |= KeyModifiers.Shift;
        if (IsDown(VirtualKey.Menu)) modifiers |= KeyModifiers.Alt;
        if (IsDown(VirtualKey.LeftWindows) || IsDown(VirtualKey.RightWindows)) modifiers |= KeyModifiers.Win;
        return modifiers;
    }

    public static bool IsModifierKey(VirtualKey key) => key is VirtualKey.Control or VirtualKey.LeftControl or VirtualKey.RightControl
        or VirtualKey.Shift or VirtualKey.LeftShift or VirtualKey.RightShift
        or VirtualKey.Menu or VirtualKey.LeftMenu or VirtualKey.RightMenu
        or VirtualKey.LeftWindows or VirtualKey.RightWindows;

    private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (IsSuspended || IsModifierKey(e.Key))
        {
            return;
        }

        var modifiers = CurrentModifiers();
        var hasCommandModifier = (modifiers & (KeyModifiers.Ctrl | KeyModifiers.Alt | KeyModifiers.Win)) != 0;
        var isFunctionKey = e.Key is >= VirtualKey.F1 and <= VirtualKey.F24;
        if (!hasCommandModifier && !isFunctionKey && IsTextInputFocused(sender as UIElement))
        {
            return;
        }

        var pool = Context<ICommandContext>().Commands;
        var gesture = new KeyGesture(modifiers, (int)e.Key);

        if (_pendingChord is { } first)
        {
            _pendingChord = null;
            ChordPending?.Invoke(this, null);
            if (DateTimeOffset.Now - _pendingSince <= ChordTimeout)
            {
                var chordCommand = pool.Shortcuts.Resolve(first with { Then = gesture });
                if (chordCommand is not null)
                {
                    Execute(pool, chordCommand);
                }

                e.Handled = true;
                return;
            }
        }

        if (pool.Shortcuts.IsChordPrefix(gesture))
        {
            _pendingChord = gesture;
            _pendingSince = DateTimeOffset.Now;
            ChordPending?.Invoke(this, pool.Shortcuts.Format(gesture));
            e.Handled = true;
            return;
        }

        var command = pool.Shortcuts.Resolve(gesture);
        if (command is not null && pool.CanExecute(command, null))
        {
            Execute(pool, command);
            e.Handled = true;
        }
    }

    private void Execute(RelayCommandPool pool, string commandId)
    {
        AppContext.Log.Write($"Shortcut -> {commandId}", LogEntryType.Information, nameof(ShortcutInputRouter));
        _ = pool.ExecuteAsync(commandId);
    }

    private static bool IsTextInputFocused(UIElement? element)
    {
        if (element?.XamlRoot is not { } root)
        {
            return false;
        }

        return FocusManager.GetFocusedElement(root) is TextBox or RichEditBox or PasswordBox;
    }
}
