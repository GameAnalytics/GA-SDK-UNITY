#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER || UNITY_5

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GameAnalyticsSDK.Editor
{
	public class GA_AssetPostprocessor : AssetPostprocessor 
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			// Fix for wrong tvOS platform imports
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
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayer, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayerStreamed, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
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
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayer, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayerStreamed, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
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
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayer, false);
					importer.SetCompatibleWithPlatform(BuildTarget.WebPlayerStreamed, false);
					importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
					importer.SetCompatibleWithPlatform(BuildTarget.tvOS, true);
					importer.SaveAndReimport();
				}
			}
		}
	}
}

#endif