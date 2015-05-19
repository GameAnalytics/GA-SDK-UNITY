#if !UNITY_4_6

using UnityEditor.iOS.Xcode;
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
			if (buildTarget == BuildTarget.iOS)
			{
				string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
				
				PBXProject proj = new PBXProject();
				proj.ReadFromString(File.ReadAllText(projPath));
				
				string target = proj.TargetGuidByName("Unity-iPhone");
				
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("Frameworks/AdSupport.framework", "Frameworks/AdSupport.framework", PBXSourceTree.Sdk));
				
				File.WriteAllText(projPath, proj.WriteToString());
			}
		}
	}
}

#endif