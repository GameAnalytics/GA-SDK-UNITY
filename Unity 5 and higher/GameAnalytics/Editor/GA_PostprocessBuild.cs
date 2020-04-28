using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Editor
{
#if UNITY_2018_1_OR_NEWER
    public class GA_PostprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
#else
    public class GA_PostprocessBuild
#endif
    {
        private static string gameanalytics_mopub = "gameanalytics_mopub_enabled";

#if UNITY_2018_1_OR_NEWER
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            Update3rdPartyIntegrations();
        }
#endif

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Update3rdPartyIntegrations();
        }

        private static void Update3rdPartyIntegrations()
        {
            UpdateMoPub();
        }

        private static void UpdateDefines(string entry, bool enabled, BuildTargetGroup[] groups)
        {
            foreach (var group in groups)
            {
                var defines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                if (enabled && !defines.Contains(entry))
                {
                    defines.Add(entry);
                }
                else if (!enabled && defines.Contains(entry))
                {
                    defines.Remove(entry);
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines.ToArray()));
            }
        }

#region 3rd Party Lib Detection

        /// <summary>
        /// Sets the scripting define symbol `gameanalytics_mopub_enabled` to true if MoPub classes are detected within the Unity project
        /// </summary>
        private static void UpdateMoPub()
        {
            var mopubTypes = new string[] { "MoPubBase", "MoPubManager" };
            if (TypeExists(mopubTypes))
            {
                UpdateDefines(gameanalytics_mopub, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(gameanalytics_mopub, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
        }

        private static bool TypeExists(params string[] types)
        {
            if (types == null || types.Length == 0)
                return false;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (types.Any(type => assembly.GetType(type) != null))
                    return true;
            }

            return false;
        }

#endregion


        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS)
            {
#if UNITY_IOS || UNITY_TVOS
                string projPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(path);

                UnityEditor.iOS.Xcode.PBXProject proj = new UnityEditor.iOS.Xcode.PBXProject();
                proj.ReadFromString(File.ReadAllText(projPath));

#if UNITY_2019_3_OR_NEWER
                string target = proj.GetUnityMainTargetGuid();
#else
                string targetName = UnityEditor.iOS.Xcode.PBXProject.GetUnityTargetName();
                string target = proj.TargetGuidByName(targetName);
#endif

                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", UnityEditor.iOS.Xcode.PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", UnityEditor.iOS.Xcode.PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("Frameworks/AdSupport.framework", "Frameworks/AdSupport.framework", UnityEditor.iOS.Xcode.PBXSourceTree.Sdk));
                //proj.SetBuildProperty(target, "ENABLE_BITCODE", "YES");

                File.WriteAllText(projPath, proj.WriteToString());
#endif
            }

        }
    }
}
