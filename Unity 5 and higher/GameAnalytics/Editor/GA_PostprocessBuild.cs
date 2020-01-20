#if UNITY_IOS || UNITY_TVOS
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;

namespace GameAnalyticsSDK.Editor
{
	public class GA_PostprocessBuild
	{
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
			if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS)
			{
				string projPath = PBXProject.GetPBXProjectPath(path);

				PBXProject proj = new PBXProject();
				proj.ReadFromString(File.ReadAllText(projPath));

#if UNITY_2019_3_OR_NEWER
				string target = proj.GetUnityMainTargetGuid();
#else
				string targetName = PBXProject.GetUnityTargetName();
				string target = proj.TargetGuidByName(targetName);
#endif

				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("Frameworks/AdSupport.framework", "Frameworks/AdSupport.framework", PBXSourceTree.Sdk));
				//proj.SetBuildProperty(target, "ENABLE_BITCODE", "YES");

				File.WriteAllText(projPath, proj.WriteToString());
			}
		}
	}
}
#endif