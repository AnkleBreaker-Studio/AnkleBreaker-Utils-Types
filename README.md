# AnkleBreaker Utils UniversalTypes

Universal wrapper types for Unity Inspector. Drop-in serializable fields that let users choose between multiple backends without changing code.

## Types

### UniversalString

A string field that supports multiple text sources:

- **PlainText** — Raw string value
- **I2Localize** — `I2.Loc.LocalizedString` (resolved via I2L natively)
- **Unity Localization** — `UnityEngine.Localization.LocalizedString` (resolved via Unity Localization package)

Priority when both localization packages are present: **I2L > Unity Localization > PlainText**.

```csharp
[SerializeField] private UniversalString title;

// Implicit conversion to string (resolves localization automatically)
string text = title;

// Or explicit ToString()
Debug.Log(title.ToString());

// Create from code
UniversalString plain = "Hello World";
UniversalString fromCode = new UniversalString("Some text");
```

### UniversalAsset\<T\>

A generic asset reference with two modes:

- **Direct** — Standard Unity object reference
- **Addressable** — `AssetReference` for `Addressables.LoadAssetAsync<T>()`

```csharp
[SerializeField] private UniversalSprite icon; // alias for UniversalAsset<Sprite>

// Direct mode
Sprite sprite = icon.DirectReference;

// Addressable mode
var handle = Addressables.LoadAssetAsync<Sprite>(icon.AddressableReference);
```

### UniversalSound

A sound reference supporting multiple audio middleware:

- **AudioClip** — Standard Unity AudioClip
- **Wwise** — `AK.Wwise.Event`
- **FMOD** — `FMODUnity.EventReference`

```csharp
[SerializeField] private UniversalSound clickSound;

switch (clickSound.Mode)
{
    case UniversalSound.SoundMode.AudioClip:
        audioSource.PlayOneShot(clickSound.AudioClip);
        break;
#if AB_WWISE
    case UniversalSound.SoundMode.Wwise:
        clickSound.WwiseEvent.Post(gameObject);
        break;
#endif
#if AB_FMOD
    case UniversalSound.SoundMode.FMOD:
        RuntimeManager.PlayOneShot(clickSound.FMODEvent);
        break;
#endif
}
```

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
"com.anklebreaker-studio.utils.universaltypes": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-UniversalTypes.git#Release"
```

## I2 Localization Setup

If you use **I2 Localization** (Asset Store version without asmdef), the package will detect it automatically on first import and offer to create the required Assembly Definitions (`I2.Loc.asmdef` + `I2.Loc.Editor.asmdef`).

You can also trigger this manually via: **AnkleBreaker > UniversalTypes > Create I2L Assembly Definitions**

This is required for `UniversalString` to reference `I2.Loc.LocalizedString` directly.

## Requirements

- Unity 2022.3+
- No required dependencies — works standalone
- Optional: I2 Localization, Unity Localization, Addressables, Wwise, FMOD

## Scripting Defines (auto-detected)

| Define | Source |
|---|---|
| `AB_I2_LOCALIZE` | I2 Localization detected (via DefineManager) |
| `AB_UNITY_LOCALIZATION` | `com.unity.localization` installed (via versionDefines) |
| `AB_ADDRESSABLES` | `com.unity.addressables` installed (via versionDefines) |
| `AB_WWISE` | Wwise SDK detected (via DefineManager) |
| `AB_FMOD` | FMOD SDK detected (via DefineManager) |

## License

MIT
