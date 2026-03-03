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
        [SerializeField] private string localizedTerm = "";
#elif AB_UNITY_LOCALIZATION
        [SerializeField] private UnityEngine.Localization.LocalizedString localizedString;
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
        /// <summary>The I2Localize term key.</summary>
        public string LocalizedTerm => localizedTerm;
#elif AB_UNITY_LOCALIZATION
        /// <summary>The Unity Localization LocalizedString reference.</summary>
        public UnityEngine.Localization.LocalizedString LocalizedString => localizedString;
#endif
        /// <summary>True if the current value is null or empty.</summary>
        public bool IsEmpty
        {
            get
            {
#if AB_I2_LOCALIZE
                if (isLocalized) return string.IsNullOrEmpty(localizedTerm);
#elif AB_UNITY_LOCALIZATION
                if (isLocalized) return localizedString == null || localizedString.IsEmpty;
#endif
                return string.IsNullOrEmpty(plainText);
            }
        }

        // ─── Resolution ─────────────────────────────────────────────

        /// <summary>
        /// Resolves and returns the string value.
        /// For localized mode: I2Localize resolves via reflection, Unity Localization via API.
        /// Falls back to raw term/key if translation fails.
        /// </summary>
        public override string ToString()
        {
#if AB_I2_LOCALIZE
            if (isLocalized && !string.IsNullOrEmpty(localizedTerm))
                return ResolveI2Term(localizedTerm);
#elif AB_UNITY_LOCALIZATION
            if (isLocalized && localizedString != null && !localizedString.IsEmpty)
                return localizedString.GetLocalizedString();
#endif
            return plainText ?? "";
        }

        // ─── Operators ──────────────────────────────────────────────

        public static implicit operator string(UniversalString us) => us?.ToString() ?? "";

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
#if AB_I2_LOCALIZE
        // ─── I2Localize Reflection ──────────────────────────────────

        private static System.Reflection.MethodInfo _cachedGetTranslation;
        private static bool _reflectionAttempted;

        private static string ResolveI2Term(string term)
        {
            if (!_reflectionAttempted)
            {
                _reflectionAttempted = true;
                var type = FindTypeInAllAssemblies("I2.Loc.LocalizationManager");
                if (type != null)
                {
                    _cachedGetTranslation = type.GetMethod(
                        "GetTranslation",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                        null,
                        new Type[] { typeof(string), typeof(bool), typeof(int), typeof(bool), typeof(bool), typeof(GameObject), typeof(string), typeof(bool) },
                        null);
                }
            }

            if (_cachedGetTranslation != null)
            {
                var result = _cachedGetTranslation.Invoke(null, new object[] { term, true, 0, true, false, null, null, true }) as string;
                if (!string.IsNullOrEmpty(result))
                    return result;
            }

            return term; // Fallback: return raw term
        }

        private static Type FindTypeInAllAssemblies(string fullTypeName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = asm.GetType(fullTypeName);
                if (type != null) return type;
            }
            return null;
        }
#endif
    }
}