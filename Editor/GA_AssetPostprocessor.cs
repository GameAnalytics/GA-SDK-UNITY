using UnityEditor;
using UnityEngine;

namespace GameAnalyticsSDK.Editor
{
    public class GA_AssetPostprocessor : AssetPostprocessor
    {
        private static string AssetsPrependPath { get { return GA_EditorPaths.PluginsRoot; } }

        // Assets shipped by earlier SDK versions. A .unitypackage import merges instead
        // of replacing, so upgrading over an old install leaves them behind: the old
        // static libs would link next to the xcframework and fail the Xcode build, and
        // the old Dependencies.xml would keep making EDM4U a required dependency.
        private static readonly string[] LegacyAssets =
        {
            // replaced by Plugins/Apple/GameAnalytics.xcframework
            "/Plugins/iOS/libGameAnalytics.a",
            "/Plugins/iOS/GameAnalytics.h",
            "/Plugins/iOS/GameAnalytics.xcprivacy",
            "/Plugins/tvOS/libGameAnalyticsTVOS.a",
            "/Plugins/tvOS/GameAnalyticsTVOS.h",
            // superseded by the shared bridge/wrapper in Plugins/Apple
            "/Plugins/tvOS/GameAnalyticsTVOSUnity.m",
            "/Plugins/Scripts/Wrapper/GA_tvOSWrapper.cs",
            // unused static lib dropped from the desktop (shared-lib) layout
            "/Plugins/Linux/libGameAnalytics.a",
            // pre-8.0 Mono wrapper (managed GameAnalytics.dll + sqlite) replaced by the
            // native C++ SDK; the Linux "sqlite3.so" was in fact a Mach-O binary
            "/Plugins/GameAnalytics.dll",
            "/Plugins/Linux/sqlite3.so",
            "/Plugins/Windows/x64",
            "/Plugins/Windows/x86",
            // Samsung TV and Tizen support removed from the SDK
            "/Plugins/SamsungTV",
            "/Plugins/Tizen",
            "/Plugins/Scripts/Wrapper/GA_TizenWrapper.cs",
            // UWP support removed from the SDK
            "/Plugins/WSA",
            "/Plugins/Scripts/Wrapper/GA_UWPWrapper.cs",
            // EDM4U dependency declaration; the SDK no longer needs any resolved
            // Android artifact (App Set ID is picked up at runtime when present)
            "/Editor/Android/Dependencies.xml",
            "/Editor/Android",
            // icons of the pre-8.2.0 IMGUI settings inspector
            "/Gizmos/GameAnalytics/Images/active.png",
            "/Gizmos/GameAnalytics/Images/default.png",
            "/Gizmos/GameAnalytics/Images/delete.png",
            "/Gizmos/GameAnalytics/Images/game.png",
            "/Gizmos/GameAnalytics/Images/home.png",
            "/Gizmos/GameAnalytics/Images/info.png",
            "/Gizmos/GameAnalytics/Images/question.png",
            "/Gizmos/GameAnalytics/Images/update_orange.png",
            "/Gizmos/GameAnalytics/Images/user.png",
        };

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string legacyAsset in LegacyAssets)
            {
                string path = AssetsPrependPath + legacyAsset;
                if ((System.IO.File.Exists(path) || System.IO.Directory.Exists(path)) && AssetDatabase.DeleteAsset(path))
                {
                    Debug.Log("GameAnalytics: removed legacy asset " + path + " (no longer part of the SDK)");
                }
            }

            #region iOS and tvOS
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/Apple/GameAnalytics.xcframework") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.iOS) || !importer.GetCompatibleWithPlatform(BuildTarget.tvOS)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/Apple/GameAnalyticsUnity.m") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.iOS) || !importer.GetCompatibleWithPlatform(BuildTarget.tvOS)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            #endregion // iOS and tvOS
            #region General
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/GameAnalytics.dll") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() ||
                    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinux64) ||
                    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSX) ||
                    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows) ||
                    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows64) ||
                    importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            #endregion // General
            #region Standalone
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/Windows/x86/sqlite3.dll") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/Windows/x64/sqlite3.dll") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows64)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                // C++ shared libs. Saving must be conditional: SaveAndReimport from
                // inside OnPostprocessAllAssets retriggers this callback, so an
                // unconditional save loops the import forever.
                ApplyCppLibImportSettings("Windows/GameAnalytics.dll", windows: true, windows64: true, linux: false, osx: false);
                ApplyCppLibImportSettings("Linux/libGameAnalytics.so", windows: false, windows64: false, linux: true, osx: false);
                ApplyCppLibImportSettings("MacOS/libGameAnalytics.dylib", windows: false, windows64: false, linux: false, osx: true);
            }
            #endregion // Standalone
            #region WebGL
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/WebGL/GameAnalytics.WebGL.dll") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/WebGL/HandleIO.jslib") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/WebGL/Mono.Data.Sqlite.dll") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/WebGL/sqlite.c") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/WebGL/sqlite.h") as PluginImporter;
                if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
                    importer.SaveAndReimport();
                }
            }
            #endregion // WebGL
        }

        private static void ApplyCppLibImportSettings(string pathInPlugins, bool windows, bool windows64, bool linux, bool osx)
        {
            PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/" + pathInPlugins) as PluginImporter;
            if (importer == null)
            {
                return;
            }

            bool changed = false;

            if (importer.GetCompatibleWithAnyPlatform())
            {
                importer.SetCompatibleWithAnyPlatform(false);
                changed = true;
            }
            if (importer.GetCompatibleWithEditor())
            {
                importer.SetCompatibleWithEditor(false);
                changed = true;
            }

            changed |= SetCompatibility(importer, BuildTarget.Android, false);
            changed |= SetCompatibility(importer, BuildTarget.iOS, false);
            changed |= SetCompatibility(importer, BuildTarget.tvOS, false);
            changed |= SetCompatibility(importer, BuildTarget.WebGL, false);
            changed |= SetCompatibility(importer, BuildTarget.WSAPlayer, false);
            changed |= SetCompatibility(importer, BuildTarget.StandaloneWindows, windows);
            changed |= SetCompatibility(importer, BuildTarget.StandaloneWindows64, windows64);
            changed |= SetCompatibility(importer, BuildTarget.StandaloneLinux64, linux);
            changed |= SetCompatibility(importer, BuildTarget.StandaloneOSX, osx);

            if (changed)
            {
                importer.SaveAndReimport();
            }
        }

        private static bool SetCompatibility(PluginImporter importer, BuildTarget target, bool enabled)
        {
            if (importer.GetCompatibleWithPlatform(target) == enabled)
            {
                return false;
            }
            importer.SetCompatibleWithPlatform(target, enabled);
            return true;
        }
    }
}
