using System.IO;
using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    /// <summary>
    /// Automatically detects I2 Localization without an asmdef and offers to create one.
    /// This is required for UniversalString to directly reference I2.Loc.LocalizedString
    /// instead of relying on reflection.
    /// </summary>
    [InitializeOnLoad]
    public static class I2LocAsmdefSetup
    {
        private const string SessionKey = "AB_I2LocAsmdefSetup_Asked";

        static I2LocAsmdefSetup()
        {
            // Only run once per editor session
            if (SessionState.GetBool(SessionKey, false))
                return;

            // Delay to avoid running during import
            EditorApplication.delayCall += CheckAndPrompt;
        }

        private static void CheckAndPrompt()
        {
            if (!IsI2LocPresent())
                return;

            if (HasI2LocAsmdef())
                return;

            SessionState.SetBool(SessionKey, true);

            bool create = EditorUtility.DisplayDialog(
                "AnkleBreaker UniversalTypes - I2 Localization Setup",
                "I2 Localization was detected without an Assembly Definition.\n\n" +
                "UniversalString requires an asmdef for I2L to use LocalizedString directly.\n\n" +
                "Create I2.Loc.asmdef and I2.Loc.Editor.asmdef now?",
                "Create asmdefs",
                "Skip (use reflection fallback)");

            if (create)
                CreateI2LocAsmdefs();
        }

        private static bool IsI2LocPresent()
        {
            // Check common I2L locations
            string[] possiblePaths =
            {
                "Assets/I2/Localization/Scripts",
                "Assets/Plugins/I2/Localization/Scripts",
            };

            foreach (string path in possiblePaths)
            {
                string fullPath = Path.Combine(Application.dataPath, "..", path);
                if (Directory.Exists(fullPath))
                    return true;
            }

            return false;
        }

        private static bool HasI2LocAsmdef()
        {
            string[] guids = AssetDatabase.FindAssets("I2.Loc t:asmdef");
            return guids.Length > 0;
        }

        /// <summary>
        /// Creates I2.Loc.asmdef and I2.Loc.Editor.asmdef in the I2 Localization Scripts folder.
        /// Can be called manually from menu.
        /// </summary>
        [MenuItem("AnkleBreaker/UniversalTypes/Create I2L Assembly Definitions")]
        public static void CreateI2LocAsmdefs()
        {
            string scriptsPath = FindI2ScriptsPath();
            if (string.IsNullOrEmpty(scriptsPath))
            {
                EditorUtility.DisplayDialog("Error", "Could not find I2 Localization Scripts folder.", "OK");
                return;
            }

            // Detect which optional references I2L needs
            var runtimeRefs = new System.Collections.Generic.List<string>();
            if (IsPackageInstalled("com.unity.textmeshpro") || IsPackageInstalled("com.unity.ugui"))
                runtimeRefs.Add("Unity.TextMeshPro");
            if (IsPackageInstalled("com.unity.inputsystem"))
                runtimeRefs.Add("Unity.InputSystem");

            string refsJson = runtimeRefs.Count > 0
                ? "\"" + string.Join("\",\"", runtimeRefs) + "\""
                : "";

            // Runtime asmdef
            string runtimeAsmdef = $@"{{
    ""name"": ""I2.Loc"",
    ""rootNamespace"": ""I2.Loc"",
    ""references"": [{refsJson}],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}";

            string runtimeAsmdefPath = Path.Combine(scriptsPath, "I2.Loc.asmdef");
            File.WriteAllText(runtimeAsmdefPath, runtimeAsmdef);

            // Editor asmdef
            string editorAsmdef = $@"{{
    ""name"": ""I2.Loc.Editor"",
    ""rootNamespace"": ""I2.Loc"",
    ""references"": [""I2.Loc""{(runtimeRefs.Count > 0 ? ",\"" + string.Join("\",\"", runtimeRefs) + "\"" : "")}],
    ""includePlatforms"": [""Editor""],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}";

            string editorDir = Path.Combine(scriptsPath, "Editor");
            if (Directory.Exists(editorDir))
            {
                string editorAsmdefPath = Path.Combine(editorDir, "I2.Loc.Editor.asmdef");
                File.WriteAllText(editorAsmdefPath, editorAsmdef);
            }

            AssetDatabase.Refresh();
            Debug.Log("[UniversalTypes] Created I2.Loc assembly definitions. Recompiling...");
        }

        private static string FindI2ScriptsPath()
        {
            string[] possiblePaths =
            {
                Path.Combine(Application.dataPath, "I2", "Localization", "Scripts"),
                Path.Combine(Application.dataPath, "Plugins", "I2", "Localization", "Scripts"),
            };

            foreach (string path in possiblePaths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            return null;
        }

        private static bool IsPackageInstalled(string packageName)
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath)) return false;
            string manifest = File.ReadAllText(manifestPath);
            return manifest.Contains(packageName);
        }
    }
}
