// GoldenHive.NET.WinUI.Framework.ModuleLoader
// Loads client modules at run time: the catalogue of the Module Registry decides which version a user gets (rollouts),
// packages come from the ModuleDelivery service (SHA-256 + ECDSA signature checked, key pinned on first use) or from the
// development folder next to the host (Modules/<id>/). Every module runs in its own load context with its own
// ModuleAppContext; all registrations (commands, views, icons, texts, subscriptions, workflow steps) are tracked and
// removed on unload, so modules can be updated while the app runs (HotSwap).
// The Sdk folder holds the small module SDK: ModuleEntryPoint base class, ModuleViewModel, UI helpers, InputForm, SimpleChart.
