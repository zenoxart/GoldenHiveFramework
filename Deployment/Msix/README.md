# MSIX and App Installer (host)

- The host is packaged as MSIX (single-project MSIX is already enabled in the host project).
- Signed with a company certificate; the certificate must be trusted on the clients.
- Distributed via an `.appinstaller` file that checks for updates at startup or in the background.
- A host update always requires a restart; modules, by contrast, are updated through the ModuleLoader.
- Alternative: unpackaged with a self-contained Windows App SDK and a custom updater.
