// GoldenHive.NET.WinUI.Framework.Security
// Connection of the client to the Golden Hive backend (API gateway): sign-in (JWT), session restore with the token kept
// in the Windows credential vault, permissions of the signed-in user, Result-based HTTP access (ApiClient implements
// IModuleApi for modules) with one automatic token refresh on 401, and localized error messages.
