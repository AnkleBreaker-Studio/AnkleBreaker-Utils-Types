# Changelog

## [1.1.0] - 2026-03-04

### Changed

- **UniversalString**: I2L localization now uses `I2.Loc.LocalizedString` directly instead of a plain string term with reflection
- **UniversalString**: `ToString()` supports both I2L and Unity Localization simultaneously with I2L priority
- **UniversalStringDrawer**: Localized mode now displays I2L's native term picker (popup + RTL mask) instead of a raw text field

### Added

- `UniversalString` constructors: `(string)`, `(I2.Loc.LocalizedString)`, `(UnityEngine.Localization.LocalizedString)`
- `implicit operator UniversalString(string)` — allows `UniversalString us = "hello";`
- `I2LocAsmdefSetup` editor script — auto-detects I2L without asmdef and offers to create `I2.Loc.asmdef` + `I2.Loc.Editor.asmdef`
- Menu item: **AnkleBreaker > UniversalTypes > Create I2L Assembly Definitions**
- Assembly references to `I2.Loc` and `Unity.Localization` in both Runtime and Editor asmdefs

### Removed

- Reflection-based I2L translation resolution (replaced by direct `LocalizedString.ToString()`)

## [1.0.0] - 2026-03-03

### Added

- `UniversalString` — Inspector-friendly string that supports PlainText, i2Localize term, or Unity Localization (table + entry key)
- `UniversalAsset<T>` — Generic asset reference supporting Direct reference or Addressable key
- `UniversalSprite` — Convenience alias for `UniversalAsset<Sprite>`
- `UniversalSound` — Inspector-friendly sound reference supporting AudioClip, Wwise event name, or FMOD event path
- Custom PropertyDrawers for all types with mode dropdown and contextual fields
- `versionDefines` in asmdef for auto-detection of Unity Localization and Addressables packages
