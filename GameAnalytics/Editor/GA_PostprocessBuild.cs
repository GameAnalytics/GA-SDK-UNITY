﻿using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;

namespace GameAnalyticsSDK
{
	public class GA_PostprocessBuild
	{
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
#if UNITY_4_6
			if (buildTarget == BuildTarget.iPhone)
#else
			if (buildTarget == BuildTarget.iOs)
#endif
			{
				string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
				
				PBXProject proj = new PBXProject();
				proj.ReadFromString(File.ReadAllText(projPath));
				
				string target = proj.TargetGuidByName("Unity-iPhone");
				
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));
				if (!proj.HasFramework("AdSupport.framework")) {
#if UNITY_4_6
					proj.AddFrameworkToProject(target, "AdSupport.framework", false); // can't this be true ?
#else
					proj.AddFileToBuild(target, proj.AddFile("Frameworks/AdSupport.framework", "Frameworks/AdSupport.framework", PBXSourceTree.Sdk));
#endif
				}
				File.WriteAllText(projPath, proj.WriteToString());
			}
		}
	}
}
