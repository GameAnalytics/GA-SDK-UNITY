using UnityEditor;
using UnityEngine;

namespace GameAnalyticsSDK.Editor
{
    public class GA_AssetPostprocessor : AssetPostprocessor
    {
        private const string AssetsPrependPath = GA_SettingsInspector.IsCustomPackage ? "Packages/com.gameanalytics.sdk/Runtime" : "Assets/GameAnalytics";

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
            // UWP support removed from the SDK
            "/Plugins/WSA",
            "/Plugins/Scripts/Wrapper/GA_UWPWrapper.cs",
            // EDM4U dependency declaration; the SDK no longer needs any resolved
            // Android artifact (App Set ID is picked up at runtime when present)
            "/Editor/Android/Dependencies.xml",
            "/Editor/Android",
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
                // c++ LIB
                const int ID_WINDOWS    = 0;
                const int ID_LINUX      = 1;
                const int ID_OSX        = 2;
                const int NUM_PLATFORMS = 3;

                string[] libNames = {
                    "Windows/GameAnalytics.dll",
                    "Linux/libGameAnalytics.so",
                    "MacOS/libGameAnalytics.dylib" 
                };

                for (int i = 0; i < NUM_PLATFORMS; ++i)
                {
                    PluginImporter importer = AssetImporter.GetAtPath(AssetsPrependPath + "/Plugins/" + libNames[i]) as PluginImporter;
                    if (importer != null)
                    {
                        importer.SetCompatibleWithEditor(false);
                        importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                        importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                        importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                        importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                        importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);

                        switch (i)
                        {
                            case ID_WINDOWS:
                                {
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, true);
                                    break;
                                }

                            case ID_LINUX:
                                {
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, true);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                                    break;
                                }

                            case ID_OSX:
                                {
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, true);
                                    break;
                                }
                        }

                        importer.SaveAndReimport();
                    }
                }
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
    }
}
