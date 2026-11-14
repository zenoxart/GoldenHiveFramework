using GoldenHive.NET.WinUI.Framework.Mvvm;

namespace GoldenHive.NET.WinUI.Framework.Security;

/// <summary>A remembered session (access token + expiry) for one server.</summary>
public sealed record StoredSession(string ServerUrl, string UserName, string AccessToken, DateTimeOffset ExpiresAt);

/// <summary>
/// Keeps the access token of "remember me" sessions in the Windows credential vault (encrypted per Windows user),
/// never in plain files.
/// </summary>
public sealed class TokenStore : AppObject
{
    private const string Resource = "GoldenHive";

    public StoredSession? Load(string serverUrl)
    {
        try
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            foreach (var credential in vault.FindAllByResource(Resource))
            {
                credential.RetrievePassword();
                var parts = credential.Password.Split('|', 3);
                if (parts.Length == 3 && parts[0].Equals(serverUrl, StringComparison.OrdinalIgnoreCase) &&
                    DateTimeOffset.TryParse(parts[1], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out var expires))
                {
                    return new StoredSession(parts[0], credential.UserName, parts[2], expires);
                }
            }
        }
        catch (Exception ex) when (ex is System.Runtime.InteropServices.COMException or InvalidOperationException or UnauthorizedAccessException)
        {
            // No stored credential (the vault throws when the resource is empty).
        }

        return null;
    }

    public void Save(StoredSession session)
    {
        try
        {
            Clear();
            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Add(new Windows.Security.Credentials.PasswordCredential(Resource, session.UserName,
                $"{session.ServerUrl}|{session.ExpiresAt:O}|{session.AccessToken}"));
        }
        catch (Exception ex) when (ex is System.Runtime.InteropServices.COMException or UnauthorizedAccessException)
        {
            AppContext.Log.Write($"The session could not be stored in the credential vault: {ex.Message}", LogEntryType.Warning, nameof(TokenStore));
        }
    }

    public void Clear()
    {
        try
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            foreach (var credential in vault.FindAllByResource(Resource))
            {
                vault.Remove(credential);
            }
        }
        catch (Exception ex) when (ex is System.Runtime.InteropServices.COMException or InvalidOperationException or UnauthorizedAccessException)
        {
        }
    }
}
