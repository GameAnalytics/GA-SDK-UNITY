using UnityEditor;

namespace GameAnalyticsSDK.Editor
{
	public class GA_AssetPostprocessor : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			#region iOS and tvOS
			{
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/tvOS/GameAnalyticsTVOS.h") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.tvOS) || importer.GetCompatibleWithPlatform(BuildTarget.iOS)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
					importer.SaveAndReimport();
				}
			}
			{
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/tvOS/GameAnalyticsTVOSUnity.m") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.tvOS) || importer.GetCompatibleWithPlatform(BuildTarget.iOS)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
					importer.SaveAndReimport();
				}
			}
			{
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/tvOS/libGameAnalyticsTVOS.a") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.tvOS) || importer.GetCompatibleWithPlatform(BuildTarget.iOS)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
					importer.SaveAndReimport();
				}
			}
            #endregion // iOS and tvOS
            #region General
            {
				PluginImporter importer = AssetImporter.GetAtPath("Assets/GameAnalytics/Plugins/GameAnalytics.dll") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinux) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinux64) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows64) ||
                    !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP") ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "ScriptingBackend").Equals("Il2Cpp")))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Tizen, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.SamsungTV, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "ScriptingBackend", "Il2Cpp");
                    importer.SaveAndReimport();
				}
			}
            #endregion // General
            #region Standalone
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/Windows/x86/sqlite3.dll") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/Windows/x64/sqlite3.dll") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows64)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/Mac/sqlite3.bundle") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
					importer.SaveAndReimport();
				}
			}
			{
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/Linux/sqlite3.so") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinux) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinux64) ||
				    !importer.GetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, true);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
					importer.SaveAndReimport();
				}
			}
			#endregion // Standalone
			#region WebGL
			{
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WebGL/GameAnalytics.WebGL.dll") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WebGL/HandleIO.jslib") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WebGL/Mono.Data.Sqlite.dll") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WebGL/sqlite.c") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
				PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WebGL/sqlite.h") as PluginImporter;
				if(importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WebGL)))
				{
					importer.SetCompatibleWithAnyPlatform(false);
					importer.SetCompatibleWithEditor(false);
					importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
					importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
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
            #region WSA
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/GameAnalytics.UWP.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP") ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "ScriptingBackend").Equals("DotNet")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "ScriptingBackend", "DotNet");
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/Microsoft.Data.Sqlite.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/MetroLog.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("AnySDK")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "AnySDK");
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/x86/sqlite3.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP") ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "CPU").Equals("X86")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "X86");
                    importer.SaveAndReimport();
                }
            }
            {
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/x64/sqlite3.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP") ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "CPU").Equals("X64")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "X64");
                    importer.SaveAndReimport();
                }
            }
			{
                PluginImporter importer = AssetImporter.GetAtPath("Assets/Plugins/WSA/ARM/sqlite3.dll") as PluginImporter;
                if (importer != null && (importer.GetCompatibleWithAnyPlatform() || !importer.GetCompatibleWithPlatform(BuildTarget.WSAPlayer) ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "SDK").Equals("UWP") ||
                    !importer.GetPlatformData(BuildTarget.WSAPlayer, "CPU").Equals("ARM")))
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    importer.SetCompatibleWithEditor(false);
                    importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinuxUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXIntel64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSXUniversal, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.tvOS, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                    importer.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "ARM");
                    importer.SaveAndReimport();
                }
            }
            #endregion // WSA
        }
	}
}