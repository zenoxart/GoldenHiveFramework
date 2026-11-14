# Module package format

A package is a ZIP file containing:

- `manifest.json` (signed)
- the entry assembly and module-specific dependencies (without framework contracts)
- `<Module>.deps.json`
- optional resources (`.pri`, images, loose XAML as an embedded resource in the DLL)

Packages are immutable: a new version always produces a new package.
