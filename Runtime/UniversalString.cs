using System;
using UnityEngine;

namespace AnkleBreaker.Utils.Types
{
    /// <summary>
    /// Universal string type that supports plain text, i2Localize terms, and Unity Localization entries.
    /// Use in the Inspector to let users choose their preferred string source.
    /// </summary>
    [Serializable]
    public class UniversalString
    {
        public enum StringMode
        {
            PlainText,
            I2Localize,
            UnityLocalization
        }

        [SerializeField] private StringMode mode = StringMode.PlainText;
        [SerializeField] private string plainText = "";
        [SerializeField] private string i2Term = "";
        [SerializeField] private string locTableName = "";
        [SerializeField] private string locEntryKey = "";

        // ─── Properties ─────────────────────────────────────────────

        public StringMode Mode => mode;
        public string PlainText => plainText;

        /// <summary>i2Localize term key (e.g. "UI/MainMenu/Title").</summary>
        public string I2Term => i2Term;

        /// <summary>Unity Localization table name (e.g. "UI Strings").</summary>
        public string LocalizationTableName => locTableName;

        /// <summary>Unity Localization entry key (e.g. "MAIN_MENU_TITLE").</summary>
        public string LocalizationEntryKey => locEntryKey;

        // ─── Convenience ────────────────────────────────────────────

        /// <summary>
        /// Returns the raw stored value based on the current mode.
        /// For I2Localize and UnityLocalization, returns the term/entry key.
        /// Use your localization system to resolve the actual translated string.
        /// </summary>
        public string GetRawValue()
        {
            switch (mode)
            {
                case StringMode.PlainText:
                    return plainText;
                case StringMode.I2Localize:
                    return i2Term;
                case StringMode.UnityLocalization:
                    return locEntryKey;
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
                    case StringMode.I2Localize:
                        return string.IsNullOrEmpty(i2Term);
                    case StringMode.UnityLocalization:
                        return string.IsNullOrEmpty(locEntryKey);
                    default:
                        return true;
                }
            }
        }

        public override string ToString() => GetRawValue();

        public static implicit operator string(UniversalString us) => us?.GetRawValue();
    }
}