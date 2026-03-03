using System;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Universal string type that supports plain text, i2Localize terms, and Unity Localization entries.
    /// Available modes depend on which localization packages are installed in the project.
    /// </summary>
    [Serializable]
    public class UniversalString
    {
        public enum StringMode
        {
            PlainText = 0,
#if AB_I2_LOCALIZE
            I2Localize = 1,
#endif
#if AB_UNITY_LOCALIZATION
            UnityLocalization = 2,
#endif
        }

        [SerializeField] private StringMode mode = StringMode.PlainText;
        [SerializeField] private string plainText = "";

#if AB_I2_LOCALIZE
        [SerializeField] private string i2Term = "";
#endif

#if AB_UNITY_LOCALIZATION
        [SerializeField] private string locTableName = "";
        [SerializeField] private string locEntryKey = "";
#endif

        // ─── Properties ─────────────────────────────────────────────

        public StringMode Mode => mode;
        public string PlainText => plainText;

#if AB_I2_LOCALIZE
        /// <summary>i2Localize term key (e.g. "UI/MainMenu/Title").</summary>
        public string I2Term => i2Term;
#endif

#if AB_UNITY_LOCALIZATION
        /// <summary>Unity Localization table name (e.g. "UI Strings").</summary>
        public string LocalizationTableName => locTableName;

        /// <summary>Unity Localization entry key (e.g. "MAIN_MENU_TITLE").</summary>
        public string LocalizationEntryKey => locEntryKey;
#endif

        // ─── Convenience ────────────────────────────────────────────

        /// <summary>
        /// Returns the raw stored value based on the current mode.
        /// For localized modes, returns the term/key. Use your localization API to resolve.
        /// </summary>
        public string GetRawValue()
        {
            switch (mode)
            {
                case StringMode.PlainText:
                    return plainText;
#if AB_I2_LOCALIZE
                case StringMode.I2Localize:
                    return i2Term;
#endif
#if AB_UNITY_LOCALIZATION
                case StringMode.UnityLocalization:
                    return locEntryKey;
#endif
                default:
                    return plainText;
            }
        }

        /// <summary>
        /// Returns true if the stored value for the current mode is null or empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                switch (mode)
                {
                    case StringMode.PlainText:
                        return string.IsNullOrEmpty(plainText);
#if AB_I2_LOCALIZE
                    case StringMode.I2Localize:
                        return string.IsNullOrEmpty(i2Term);
#endif
#if AB_UNITY_LOCALIZATION
                    case StringMode.UnityLocalization:
                        return string.IsNullOrEmpty(locEntryKey);
#endif
                    default:
                        return true;
                }
            }
        }

        public override string ToString() => GetRawValue();

        public static implicit operator string(UniversalString us) => us?.GetRawValue();
    }
}