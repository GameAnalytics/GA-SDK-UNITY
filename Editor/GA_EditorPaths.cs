using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAnalyticsSDK.Editor
{
    /// <summary>
    /// Resolves where the SDK lives in the current project: installed as the UPM
    /// package (public repo layout, sources under Runtime/) or under Assets/
    /// (this dev project, or a .unitypackage import). All editor asset loads go
    /// through here so both layouts work without a compile-time switch.
    /// </summary>
    public static class GA_EditorPaths
    {
        public const string PackageName = "com.gameanalytics.sdk";

        private const string PackageRoot = "Packages/" + PackageName;
        private const string AssetsRoot = "Assets/GameAnalytics";

        public static bool IsPackage
        {
            get { return AssetDatabase.IsValidFolder(PackageRoot); }
        }

        /// <summary>Folder containing Editor/, Gizmos/ and the runtime sources.</summary>
        public static string Root
        {
            get { return IsPackage ? PackageRoot : AssetsRoot; }
        }

        /// <summary>
        /// Folder that contains the Plugins/ tree. The public package keeps it under
        /// Runtime/; in an Assets install Plugins/ sits directly under the root.
        /// </summary>
        public static string PluginsRoot
        {
            get { return IsPackage ? PackageRoot + "/Runtime" : AssetsRoot; }
        }

        public static Texture2D LoadTexture(string pathInGizmosFolder)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "/Gizmos/GameAnalytics/" + pathInGizmosFolder);
        }

        public static StyleSheet LoadStyleSheet(string fileName)
        {
            return AssetDatabase.LoadAssetAtPath<StyleSheet>(Root + "/Editor/" + fileName);
        }
    }
}
