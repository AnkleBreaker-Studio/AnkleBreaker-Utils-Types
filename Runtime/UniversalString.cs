using System;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Universal string that supports plain text and localization.
    /// When a localization package is installed (I2Localize or Unity Localization),
    /// a toggle appears in the Inspector to switch between plain text and localized term.
    /// I2Localize takes priority over Unity Localization if both are present.
    /// ToString() resolves the localized value automatically.
    /// </summary>
    [Serializable]
    public class UniversalString
    {
#if AB_I2_LOCALIZE || AB_UNITY_LOCALIZATION
        [SerializeField] private bool isLocalized;
#endif
        [SerializeField] private string plainText = "";

#if AB_I2_LOCALIZE
        [SerializeField] private I2.Loc.LocalizedString i2LocalizedString;
#endif

#if AB_UNITY_LOCALIZATION
        [SerializeField] private UnityEngine.Localization.LocalizedString unityLocalizedString;
#endif

        // ─── Constructors ───────────────────────────────────────────

        /// <summary>Default constructor. Creates an empty plain text string.</summary>
        public UniversalString()
        {
            plainText = "";
        }

        /// <summary>Creates a UniversalString from plain text.</summary>
        public UniversalString(string text)
        {
            plainText = text ?? "";
        }

#if AB_I2_LOCALIZE
        /// <summary>Creates a UniversalString from an I2 LocalizedString.</summary>
        public UniversalString(I2.Loc.LocalizedString localized)
        {
            isLocalized = true;
            i2LocalizedString = localized;
        }
#endif

#if AB_UNITY_LOCALIZATION
        /// <summary>Creates a UniversalString from a Unity LocalizedString.</summary>
        public UniversalString(UnityEngine.Localization.LocalizedString localized)
        {
            isLocalized = true;
            unityLocalizedString = localized;
        }
#endif

        // ─── Properties ─────────────────────────────────────────────

        /// <summary>True if this string is in localized mode.</summary>
        public bool IsLocalized
        {
            get
            {
#if AB_I2_LOCALIZE || AB_UNITY_LOCALIZATION
                return isLocalized;
#else
                return false;
#endif
            }
        }

        /// <summary>The plain text value (used when not localized).</summary>
        public string PlainText => plainText;

#if AB_I2_LOCALIZE
        /// <summary>The I2 Localization LocalizedString.</summary>
        public I2.Loc.LocalizedString I2LocalizedString => i2LocalizedString;
#endif

#if AB_UNITY_LOCALIZATION
        /// <summary>The Unity Localization LocalizedString reference.</summary>
        public UnityEngine.Localization.LocalizedString UnityLocalizedString => unityLocalizedString;
#endif

        /// <summary>True if the current value is null or empty.</summary>
        public bool IsEmpty
        {
            get
            {
#if AB_I2_LOCALIZE || AB_UNITY_LOCALIZATION
                if (isLocalized)
                {
#if AB_I2_LOCALIZE
                    if (string.IsNullOrEmpty(i2LocalizedString.mTerm))
                    {
#if AB_UNITY_LOCALIZATION
                        return unityLocalizedString == null || unityLocalizedString.IsEmpty;
#else
                        return true;
#endif
                    }
                    return false;
#elif AB_UNITY_LOCALIZATION
                    return unityLocalizedString == null || unityLocalizedString.IsEmpty;
#endif
                }
#endif
                return string.IsNullOrEmpty(plainText);
            }
        }

        // ─── Resolution ─────────────────────────────────────────────

        /// <summary>
        /// Resolves and returns the string value.
        /// Priority: I2Localize > Unity Localization > Plain Text.
        /// Falls back to raw term/key if translation fails.
        /// </summary>
        public override string ToString()
        {
#if AB_I2_LOCALIZE || AB_UNITY_LOCALIZATION
            if (isLocalized)
            {
#if AB_I2_LOCALIZE
                if (!string.IsNullOrEmpty(i2LocalizedString.mTerm))
                {
                    string i2Result = i2LocalizedString.ToString();
                    if (!string.IsNullOrEmpty(i2Result))
                        return i2Result;
                }
#endif

#if AB_UNITY_LOCALIZATION
                if (unityLocalizedString != null && !unityLocalizedString.IsEmpty)
                    return unityLocalizedString.GetLocalizedString();
#endif
            }
#endif
            return plainText ?? "";
        }

        // ─── Operators ──────────────────────────────────────────────

        /// <summary>Implicit conversion to string. Returns the resolved value.</summary>
        public static implicit operator string(UniversalString us) => us?.ToString() ?? "";

        /// <summary>Implicit conversion from string. Creates a plain text UniversalString.</summary>
        public static implicit operator UniversalString(string s) => new UniversalString(s);

        public static bool operator ==(UniversalString us, string s) => (us?.ToString() ?? "") == s;
        public static bool operator !=(UniversalString us, string s) => (us?.ToString() ?? "") != s;

        public static bool operator ==(string s, UniversalString us) => (us?.ToString() ?? "") == s;
        public static bool operator !=(string s, UniversalString us) => (us?.ToString() ?? "") != s;

        public override bool Equals(object obj)
        {
            if (obj is string s) return ToString() == s;
            if (obj is UniversalString other) return ToString() == other.ToString();
            return false;
        }

        public override int GetHashCode() => ToString().GetHashCode();
    }
}
