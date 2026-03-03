using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    /// <summary>
    /// Auto-detects non-UPM packages (Wwise, FMOD, I2Localize) and sets scripting define symbols.
    /// UPM packages (Addressables, Unity Localization) are handled via versionDefines in asmdef.
    /// </summary>
    [InitializeOnLoad]
    public static class UniversalTypesDefineManager
    {
        private static readonly DefineMapping[] Mappings =
        {
            new DefineMapping("AB_WWISE", "AK.Wwise.Unity.API.WwiseTypes"),
            new DefineMapping("AB_FMOD", "FMODUnity"),
            new DefineMapping("AB_I2_LOCALIZE", "I2.Loc.LocalizationManager", true),
        };

        static UniversalTypesDefineManager()
        {
            UpdateDefines();
        }

        private static void UpdateDefines()
        {
            var namedTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string currentDefines = PlayerSettings.GetScriptingDefineSymbols(namedTarget);
            var defineList = new List<string>(currentDefines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            bool changed = false;

            foreach (var mapping in Mappings)
            {
                bool packagePresent = mapping.CheckByType
                    ? IsTypePresent(mapping.AssemblyOrTypeName)
                    : IsAssemblyPresent(mapping.AssemblyOrTypeName);

                bool definePresent = defineList.Contains(mapping.Define);

                if (packagePresent && !definePresent)
                {
                    defineList.Add(mapping.Define);
                    changed = true;
                }
                else if (!packagePresent && definePresent)
                {
                    defineList.Remove(mapping.Define);
                    changed = true;
                }
            }

            if (changed)
            {
                string newDefines = string.Join(";", defineList);
                PlayerSettings.SetScriptingDefineSymbols(namedTarget, newDefines);
            }
        }

        private static bool IsAssemblyPresent(string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (assemblies[i].GetName().Name == assemblyName)
                    return true;
            }
            return false;
        }

        private static bool IsTypePresent(string fullTypeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (assemblies[i].GetType(fullTypeName) != null)
                    return true;
            }
            return false;
        }

        private struct DefineMapping
        {
            public string Define;
            public string AssemblyOrTypeName;
            public bool CheckByType;

            public DefineMapping(string define, string assemblyOrTypeName, bool checkByType = false)
            {
                Define = define;
                AssemblyOrTypeName = assemblyOrTypeName;
                CheckByType = checkByType;
            }
        }
    }
}