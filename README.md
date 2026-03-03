# AnkleBreaker Utils Types

Universal wrapper types for Unity Inspector. Drop-in serializable fields that let users choose between multiple backends without changing code.

## Types

### UniversalString

A string field that supports multiple text sources:

- **PlainText** — Raw string value
- **I2Localize** — i2Localize term key (resolved via `I2.Loc.LocalizationManager.GetTranslation()`)
- **Unity Localization** — Table name + entry key (resolved via Unity Localization package)

```csharp
[SerializeField] private UniversalString title;

// Access
string raw = title.GetRawValue();
// Or use implicit conversion
string text = title;
```

### UniversalAsset\<T\>

A generic asset reference with two modes:

- **Direct** — Standard Unity object reference
- **Addressable** — String key for `Addressables.LoadAssetAsync<T>()`

```csharp
[SerializeField] private UniversalSprite icon; // alias for UniversalAsset<Sprite>

// Direct mode
Sprite sprite = icon.DirectReference;

// Addressable mode
string key = icon.AddressableKey;
// var handle = Addressables.LoadAssetAsync<Sprite>(key);
```

### UniversalSound

A sound reference supporting multiple audio middleware:

- **AudioClip** — Standard Unity AudioClip
- **Wwise** — Wwise event name string
- **FMOD** — FMOD event path string

```csharp
[SerializeField] private UniversalSound clickSound;

switch (clickSound.Mode)
{
    case UniversalSound.SoundMode.AudioClip:
        audioSource.PlayOneShot(clickSound.AudioClip);
        break;
    case UniversalSound.SoundMode.Wwise:
        // AkSoundEngine.PostEvent(clickSound.WwiseEventName, gameObject);
        break;
    case UniversalSound.SoundMode.FMOD:
        // RuntimeManager.PlayOneShot(clickSound.FMODEventPath);
        break;
}
```

## Installation

Add to your Unity project's `Packages/manifest.json`:

```json
"com.anklebreaker-studio.utils.universaltypes": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-UniversalTypes.git#Release"
```

## Requirements

- Unity 2022.3+
- No required dependencies — works standalone
- Optional: i2Localize, Unity Localization, Addressables, Wwise, FMOD

## License

MIT
