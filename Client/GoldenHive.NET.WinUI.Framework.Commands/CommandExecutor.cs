using GoldenHive.NET.WinUI.Framework.Mvvm;

namespace GoldenHive.NET.WinUI.Framework.Commands;

/// <summary>Result data of a command execution.</summary>
public sealed record CommandExecutedEventArgs(string CommandId, object? Parameter, Exception? Error);

/// <summary>
/// Runs commands uniformly: permission check (UI gating only - the server authorizes again),
/// CanExecute, asynchronous execution, error reporting to the log instead of crashing the UI.
/// </summary>
public sealed class CommandExecutor : AppObject
{
    /// <summary>Raised after every execution (also failed ones).</summary>
    public event EventHandler<CommandExecutedEventArgs>? Executed;

    /// <summary>Permission check; returns true if the current user has the permission. Default: allow.</summary>
    public Func<string, bool>? PermissionCheck { get; set; }

    public bool HasPermission(string? permission) => permission is null || PermissionCheck?.Invoke(permission) != false;

    public async Task<bool> ExecuteAsync(CommandRegistration registration, object? parameter, CancellationToken cancellationToken = default)
    {
        var id = registration.Descriptor.Id;
        if (!HasPermission(registration.Descriptor.Permission))
        {
            AppContext.Log.Write($"Command '{id}' denied: missing permission '{registration.Descriptor.Permission}'", LogEntryType.Warning, nameof(CommandExecutor));
            return false;
        }

        try
        {
            if (!registration.Handler.CanExecute(parameter))
            {
                return false;
            }

            await registration.Handler.ExecuteAsync(parameter, cancellationToken);
            Executed?.Invoke(this, new CommandExecutedEventArgs(id, parameter, null));
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            OnErrorOccurred(new InvalidOperationException($"Command '{id}' failed: {ex.Message}", ex));
            Executed?.Invoke(this, new CommandExecutedEventArgs(id, parameter, ex));
            return false;
        }
    }
}
