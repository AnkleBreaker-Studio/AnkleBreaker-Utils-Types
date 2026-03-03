# Changelog

## [1.0.0] - 2026-03-03

### Added

- `UniversalString` — Inspector-friendly string that supports PlainText, i2Localize term, or Unity Localization (table + entry key)
- `UniversalAsset<T>` — Generic asset reference supporting Direct reference or Addressable key
- `UniversalSprite` — Convenience alias for `UniversalAsset<Sprite>`
- `UniversalSound` — Inspector-friendly sound reference supporting AudioClip, Wwise event name, or FMOD event path
- Custom PropertyDrawers for all types with mode dropdown and contextual fields
- `versionDefines` in asmdef for auto-detection of Unity Localization and Addressables packages
