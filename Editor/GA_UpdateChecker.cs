using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using GameAnalyticsSDK.Setup;

namespace GameAnalyticsSDK.Editor
{
    /// <summary>How the SDK was installed into this project.</summary>
    public enum GA_InstallChannel
    {
        /// <summary>UPM package from a registry (OpenUPM) - updates in place via the Package Manager.</summary>
        Registry,
        /// <summary>UPM package from git/local/embedded - the version is pinned outside Unity's control.</summary>
        GitOrLocalPackage,
        /// <summary>Imported under Assets/ (.unitypackage) - updates by re-importing the new package.</summary>
        Assets
    }

    /// <summary>
    /// Checks for SDK updates against the sources the publish flow keeps current by
    /// construction: the package registry for UPM installs, the public repo's
    /// package.json for Assets/ installs, and the public CHANGELOG.md for release
    /// notes. Replaces the previous check against hand-maintained S3 status files.
    /// </summary>
    public static class GA_UpdateChecker
    {
        private const string PublicRepoRawUrl = "https://raw.githubusercontent.com/GameAnalytics/GA-SDK-UNITY/master/";
        private const string PackageJsonUrl = PublicRepoRawUrl + "package.json";
        private const string ChangelogUrl = PublicRepoRawUrl + "CHANGELOG.md";
        private const string DownloadUrlTemplate = "https://download.gameanalytics.com/unity/{0}/GA_SDK_UNITY.unitypackage";
        private const string ReleasesUrl = "https://github.com/GameAnalytics/GA-SDK-UNITY/releases";

        private const double CheckIntervalHours = 24.0;

        private static SearchRequest _searchRequest;
        private static AddRequest _addRequest;

        private static string LastCheckPrefKey { get { return "GA_last_update_check-" + Application.dataPath; } }
        private static string SkipVersionPrefKey { get { return "ga_skip_version" + "-" + Application.dataPath; } }

        /// <summary>
        /// Testing hook: forces InstallChannel so the channel-specific update flow
        /// can be exercised in projects where the SDK is not actually installed
        /// that way. While set, ApplyUpdate only logs what it would do. Not
        /// persisted - cleared by every domain reload.
        /// </summary>
        public static GA_InstallChannel? InstallChannelOverride;

        public static GA_InstallChannel InstallChannel
        {
            get
            {
                if (InstallChannelOverride.HasValue)
                {
                    return InstallChannelOverride.Value;
                }

                UnityEditor.PackageManager.PackageInfo info =
                    UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(GA_UpdateChecker).Assembly);
                if (info == null)
                {
                    return GA_InstallChannel.Assets;
                }
                return info.source == PackageSource.Registry
                    ? GA_InstallChannel.Registry
                    : GA_InstallChannel.GitOrLocalPackage;
            }
        }

        /// <summary>True when a newer version than the installed one is known.</summary>
        public static bool IsUpdateAvailable(Settings settings)
        {
            return settings != null && IsNewVersion(settings.NewVersion, Settings.VERSION);
        }

        public static string SkippedVersion
        {
            get { return EditorPrefs.GetString(SkipVersionPrefKey, ""); }
            set { EditorPrefs.SetString(SkipVersionPrefKey, value); }
        }

        /// <summary>
        /// Kicks off the update check. Runs at most once per day unless forced;
        /// fails silently when offline.
        /// </summary>
        public static void CheckForUpdates(bool force = false)
        {
            if (Settings.CheckingForUpdates)
            {
                return;
            }
            if (!force && !CheckIntervalElapsed())
            {
                return;
            }

            Settings.CheckingForUpdates = true;
            EditorPrefs.SetString(LastCheckPrefKey, DateTime.UtcNow.Ticks.ToString());

            if (InstallChannel == GA_InstallChannel.Assets)
            {
                UnityWebRequest www = UnityWebRequest.Get(PackageJsonUrl);
                GA_ContinuationManager.StartCoroutine(FetchPackageJsonCoroutine(www), () => www.isDone);
            }
            else
            {
                _searchRequest = Client.Search(GA_EditorPaths.PackageName);
                EditorApplication.update += PollSearchRequest;
            }
        }

        /// <summary>
        /// Performs the channel-appropriate update action for the given version.
        /// </summary>
        public static void ApplyUpdate(string newVersion)
        {
            if (InstallChannelOverride.HasValue)
            {
                // A faked channel must not mutate the project (Client.Add would
                // install the UPM package next to an Assets/ copy of the SDK).
                Debug.Log("GameAnalytics: install channel is overridden to " + InstallChannelOverride.Value
                    + " for testing - would now '" + UpdateActionLabel + "' for " + newVersion + " (dry run).");
                return;
            }

            switch (InstallChannel)
            {
                case GA_InstallChannel.Registry:
                    if (_addRequest != null && !_addRequest.IsCompleted)
                    {
                        return;
                    }
                    Debug.Log("GameAnalytics: updating package to " + newVersion + " via the Package Manager..");
                    _addRequest = Client.Add(GA_EditorPaths.PackageName + "@" + newVersion);
                    EditorApplication.update += PollAddRequest;
                    break;
                case GA_InstallChannel.GitOrLocalPackage:
                    // The version is pinned in the manifest (git URL / local path); point
                    // the user at the releases so they can update the pin themselves.
                    Application.OpenURL(ReleasesUrl);
                    break;
                case GA_InstallChannel.Assets:
                    Application.OpenURL(string.Format(DownloadUrlTemplate, newVersion));
                    break;
            }
        }

        /// <summary>One-line instruction matching the install channel, shown in the update window.</summary>
        public static string UpdateActionLabel
        {
            get
            {
                switch (InstallChannel)
                {
                    case GA_InstallChannel.Registry: return "Update in Package Manager";
                    case GA_InstallChannel.GitOrLocalPackage: return "Open releases page";
                    default: return "Download .unitypackage";
                }
            }
        }

        public static string UpdateActionHint
        {
            get
            {
                switch (InstallChannel)
                {
                    case GA_InstallChannel.Registry:
                        return "Installed from a package registry. The update resolves in place - no manual import.";
                    case GA_InstallChannel.GitOrLocalPackage:
                        return "Installed as a git/local package. Update the version pinned in your manifest.json.";
                    default:
                        return "Installed under Assets/. Import the downloaded package over the current install.";
                }
            }
        }

        private static bool CheckIntervalElapsed()
        {
            long ticks;
            if (!long.TryParse(EditorPrefs.GetString(LastCheckPrefKey, "0"), out ticks))
            {
                return true;
            }
            return (DateTime.UtcNow - new DateTime(ticks, DateTimeKind.Utc)).TotalHours >= CheckIntervalHours;
        }

        private static void PollSearchRequest()
        {
            if (_searchRequest == null || !_searchRequest.IsCompleted)
            {
                return;
            }
            EditorApplication.update -= PollSearchRequest;

            try
            {
                if (_searchRequest.Status == StatusCode.Success && _searchRequest.Result != null && _searchRequest.Result.Length > 0)
                {
                    UnityEditor.PackageManager.PackageInfo package = _searchRequest.Result[0];
                    string latest = !string.IsNullOrEmpty(package.versions.latestCompatible)
                        ? package.versions.latestCompatible
                        : package.versions.latest;
                    OnLatestVersionKnown(latest);
                    return;
                }
            }
            catch (Exception)
            {
                // Silent: no network, or the registry is unreachable.
            }
            finally
            {
                _searchRequest = null;
            }
            Settings.CheckingForUpdates = false;
        }

        private static void PollAddRequest()
        {
            if (_addRequest == null || !_addRequest.IsCompleted)
            {
                return;
            }
            EditorApplication.update -= PollAddRequest;

            if (_addRequest.Status == StatusCode.Success)
            {
                Debug.Log("GameAnalytics: updated to " + _addRequest.Result.version + ".");
            }
            else if (_addRequest.Error != null)
            {
                Debug.LogError("GameAnalytics: package update failed: " + _addRequest.Error.message);
            }
            _addRequest = null;
        }

        private static IEnumerator FetchPackageJsonCoroutine(UnityWebRequest www)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            bool changelogRequested = false;
            try
            {
                if (www.result == UnityWebRequest.Result.Success)
                {
                    IDictionary<string, object> packageJson =
                        GameAnalyticsSDK.Utilities.GA_MiniJSON.Deserialize(www.downloadHandler.text) as IDictionary<string, object>;
                    if (packageJson != null && packageJson.ContainsKey("version"))
                    {
                        changelogRequested = OnLatestVersionKnown(packageJson["version"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                // Silent: offline or unexpected payload.
            }
            finally
            {
                if (!changelogRequested)
                {
                    Settings.CheckingForUpdates = false;
                }
            }
        }

        /// <summary>Returns true when a changelog fetch was started (check still in flight).</summary>
        private static bool OnLatestVersionKnown(string latestVersion)
        {
            if (!IsNewVersion(latestVersion, Settings.VERSION))
            {
                Settings.CheckingForUpdates = false;
                return false;
            }

            UnityWebRequest www = UnityWebRequest.Get(ChangelogUrl);
            GA_ContinuationManager.StartCoroutine(FetchChangelogCoroutine(www, latestVersion), () => www.isDone);
            return true;
        }

        private static IEnumerator FetchChangelogCoroutine(UnityWebRequest www, string newVersion)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
                string changes = "";
                if (www.result == UnityWebRequest.Result.Success)
                {
                    changes = ExtractReleaseNotes(www.downloadHandler.text, newVersion);
                }

                GA_UpdateWindow.SetNewVersion(newVersion);
                GA_UpdateWindow.SetChanges(changes);

                if (!SkippedVersion.Equals(newVersion))
                {
                    GA_UpdateWindow.Open();
                }
            }
            catch (Exception)
            {
                // Silent: the changelog is informational; the version badge still works.
            }
            finally
            {
                Settings.CheckingForUpdates = false;
            }
        }

        /// <summary>
        /// Extracts the release notes of exactly the given version from a Keep a
        /// Changelog document ("## [x.y.z] - date" sections) as plain text for the
        /// update window. The window already shows the version, so the section
        /// header itself is not included.
        /// </summary>
        public static string ExtractReleaseNotes(string changelogMarkdown, string version)
        {
            if (string.IsNullOrEmpty(changelogMarkdown) || string.IsNullOrEmpty(version))
            {
                return "";
            }

            Regex versionHeader = new Regex(@"^##\s*\[(\d+\.\d+\.\d+)\]");
            // The section ends at the next "##" heading, a horizontal rule, a
            // link-reference definition ("[x.y.z]: https://..") or a legacy
            // bold-version heading ("**x.y.z**") - the public changelog keeps
            // its pre-Keep-a-Changelog history in that format.
            Regex sectionEnd = new Regex(@"^(##\s|---|\*\*|\[[^\]]+\]:)");
            StringBuilder result = new StringBuilder();
            bool collecting = false;

            foreach (string rawLine in changelogMarkdown.Split('\n'))
            {
                string line = rawLine.TrimEnd('\r');

                Match match = versionHeader.Match(line);
                if (match.Success)
                {
                    if (collecting)
                    {
                        break;
                    }
                    collecting = match.Groups[1].Value == version;
                    continue;
                }
                if (collecting && sectionEnd.IsMatch(line))
                {
                    break;
                }

                if (!collecting || line.Trim().Length == 0)
                {
                    continue;
                }

                if (result.Length > 0)
                {
                    result.Append("\n");
                }
                if (line.StartsWith("### "))
                {
                    result.Append(line.Substring(4)).Append(":");
                }
                else
                {
                    result.Append(line);
                }
            }

            return result.ToString();
        }

        // Version strings are [major].[minor].[patch].
        public static bool IsNewVersion(string newVersion, string currentVersion)
        {
            Version parsedNew, parsedCurrent;
            return Version.TryParse(newVersion, out parsedNew)
                && Version.TryParse(currentVersion, out parsedCurrent)
                && parsedNew > parsedCurrent;
        }
    }
}
