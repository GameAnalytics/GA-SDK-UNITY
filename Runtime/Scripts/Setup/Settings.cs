using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameAnalyticsSDK.Setup
{
    /// <summary>
    /// The Settings object contains an array of options which allows you to customize your use of GameAnalytics. Most importantly you will need to fill in your Game Key and Secret Key on the Settings object to use the service.
    /// </summary>
    ///
    public class Settings : ScriptableObject
    {
        #region public static values

        /// <summary>
        /// The version of the GA Unity Wrapper plugin
        /// </summary>
        [HideInInspector]
        public static string VERSION = "8.2.0";

        [HideInInspector]
        public static bool CheckingForUpdates = false;

        #endregion

        #region public values

        [SerializeField]
        private List<string> gameKey = new List<string>();
        [SerializeField]
        private List<string> secretKey = new List<string>();
        [SerializeField]
        public List<string> Build = new List<string>();
        [SerializeField]
        public List<string> SelectedPlatformOrganization = new List<string>();
        [SerializeField]
        public List<string> SelectedPlatformStudio = new List<string>();
        [SerializeField]
        public List<string> SelectedPlatformGame = new List<string>();
        [SerializeField]
        public List<int> SelectedPlatformGameID = new List<int>();
        [SerializeField]
        public List<int> SelectedOrganization = new List<int>();
        [SerializeField]
        public List<int> SelectedStudio = new List<int>();
        [SerializeField]
        public List<int> SelectedGame = new List<int>();

        public string NewVersion = "";
        public string Changes = "";

        public string StudioName = "";
        public string GameName = "";
        public string EmailGA = "";

        [System.NonSerialized]
        public string PasswordGA = "";
        [System.NonSerialized]
        public string TokenGA = "";
        [System.NonSerialized]
        public string LoginStatus = "Not logged in.";
        [System.NonSerialized]
        public bool JustSignedUp = false;
        [System.NonSerialized]
        public bool HideSignupWarning = false;

        [System.NonSerialized]
        public List<GameAnalyticsSDK.Setup.Organization> Organizations;

        public bool InfoLogEditor = true;
        public bool InfoLogBuild = true;
        public bool VerboseLogBuild = false;
        public bool UseManualSessionHandling = false;
        public List<string> CustomDimensions01 = new List<string>();
        public List<string> CustomDimensions02 = new List<string>();
        public List<string> CustomDimensions03 = new List<string>();

        public List<string> ResourceItemTypes = new List<string>();
        public List<string> ResourceCurrencies = new List<string>();

        public RuntimePlatform LastCreatedGamePlatform;

        public List<RuntimePlatform> Platforms = new List<RuntimePlatform>();

        [System.NonSerialized]
        public Texture2D Logo;
        [System.NonSerialized]
        public Texture2D InstrumentIcon;

        [System.NonSerialized]
        public Texture2D AmazonIcon;
        [System.NonSerialized]
        public Texture2D GooglePlayIcon;
        [System.NonSerialized]
        public Texture2D iosIcon;
        [System.NonSerialized]
        public Texture2D macIcon;
        [System.NonSerialized]
        public Texture2D windowsPhoneIcon;

        /// <summary>
        /// Legacy global flag, superseded by the per-platform BuildNumberAutoDetect
        /// list. Kept serialized so existing Settings.asset files migrate: see
        /// EnsureBuildNumberAutoDetectList().
        /// </summary>
        public bool UsePlayerSettingsBuildNumber = false;

        /// <summary>
        /// Per-platform build number source, parallel to Platforms. True means the
        /// build number is auto-detected from Player Settings > Version (supported on
        /// iOS, tvOS and Android); false means the Build list entry is reported as typed.
        /// </summary>
        [SerializeField]
        public List<bool> BuildNumberAutoDetect = new List<bool>();
        public bool SubmitErrors = true;
        public bool NativeErrorReporting = false;
        public int MaxErrorCount = 10;
        public bool SubmitFpsAverage = false;
        public bool SubmitFpsCritical = false;
        public int FpsCriticalThreshold = 20;
        public int FpsCirticalSubmitInterval = 1;

        public bool EnableMemoryHistogram = false;

        public bool EnableFPSHistogram = false;

        public bool EnableSDKInitEvent = false;

        public bool EnableHardwareTracking = false;

        public bool EnableMemoryTracking = false;


        #endregion

        #region public methods

        public void RemovePlatformAtIndex(int index)
        {
            if (index >= 0 && index < this.Platforms.Count)
            {
                this.EnsureBuildNumberAutoDetectList();
                this.gameKey.RemoveAt(index);
                this.secretKey.RemoveAt(index);
                this.Build.RemoveAt(index);
                this.SelectedPlatformOrganization.RemoveAt(index);
                this.SelectedPlatformStudio.RemoveAt(index);
                this.SelectedPlatformGame.RemoveAt(index);
                this.SelectedPlatformGameID.RemoveAt(index);
                this.SelectedOrganization.RemoveAt(index);
                this.SelectedStudio.RemoveAt(index);
                this.SelectedGame.RemoveAt(index);
                this.BuildNumberAutoDetect.RemoveAt(index);
                this.Platforms.RemoveAt(index);
            }
        }

        public void AddPlatform(RuntimePlatform platform)
        {
            this.EnsureBuildNumberAutoDetectList();
            this.gameKey.Add("");
            this.secretKey.Add("");
            this.Build.Add("0.1");
            this.SelectedPlatformOrganization.Add("");
            this.SelectedPlatformStudio.Add("");
            this.SelectedPlatformGame.Add("");
            this.SelectedPlatformGameID.Add(-1);
            this.SelectedOrganization.Add(0);
            this.SelectedStudio.Add(0);
            this.SelectedGame.Add(0);
            this.BuildNumberAutoDetect.Add(SupportsBuildNumberAutoDetect(platform));
            this.Platforms.Add(platform);
        }

        /// <summary>
        /// Platforms where the native SDK can report the app version itself
        /// (GA_Wrapper.SetAutoDetectAppVersion).
        /// </summary>
        public static bool SupportsBuildNumberAutoDetect(RuntimePlatform platform)
        {
            return platform == RuntimePlatform.Android
                || platform == RuntimePlatform.IPhonePlayer
                || platform == RuntimePlatform.tvOS;
        }

        /// <summary>
        /// Pads/trims BuildNumberAutoDetect to match Platforms. Entries added for
        /// pre-existing platforms take the legacy global UsePlayerSettingsBuildNumber
        /// flag, so settings saved by older SDK versions keep their behavior.
        /// </summary>
        public void EnsureBuildNumberAutoDetectList()
        {
            while (this.BuildNumberAutoDetect.Count < this.Platforms.Count)
            {
                RuntimePlatform platform = this.Platforms[this.BuildNumberAutoDetect.Count];
                // The legacy toggle was documented as Android + iOS only, so a migrated
                // tvOS entry keeps reporting its manual Build value.
                bool legacyAuto = this.UsePlayerSettingsBuildNumber
                    && (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer);
                this.BuildNumberAutoDetect.Add(legacyAuto);
            }
            while (this.BuildNumberAutoDetect.Count > this.Platforms.Count)
            {
                this.BuildNumberAutoDetect.RemoveAt(this.BuildNumberAutoDetect.Count - 1);
            }
        }

        /// <summary>
        /// True when the platform at this index reports its build number from
        /// Player Settings > Version instead of the manual Build entry.
        /// </summary>
        public bool IsBuildNumberAutoDetected(int index)
        {
            this.EnsureBuildNumberAutoDetectList();
            return index >= 0 && index < this.Platforms.Count
                && SupportsBuildNumberAutoDetect(this.Platforms[index])
                && this.BuildNumberAutoDetect[index];
        }

        public static readonly RuntimePlatform[] AvailablePlatforms = new RuntimePlatform[]
        {
            RuntimePlatform.Android,
            RuntimePlatform.IPhonePlayer,
            RuntimePlatform.LinuxPlayer,
            RuntimePlatform.OSXPlayer,
            RuntimePlatform.tvOS,
            RuntimePlatform.WebGLPlayer,
            RuntimePlatform.WindowsPlayer
        };

        public string[] GetAvailablePlatforms()
        {
            List<string> result = new List<string>();

            for(int i = 0; i < AvailablePlatforms.Length; ++i)
            {
                RuntimePlatform value = AvailablePlatforms[i];

                if(value == RuntimePlatform.IPhonePlayer)
                {
                    if(!this.Platforms.Contains(RuntimePlatform.tvOS) && !this.Platforms.Contains(value))
                    {
                        result.Add(value.ToString());
                    }
                    else
                    {
                        if(!this.Platforms.Contains(value))
                        {
                            result.Add(value.ToString());
                        }
                    }
                }
                else if(value == RuntimePlatform.tvOS)
                {
                    if(!this.Platforms.Contains(RuntimePlatform.IPhonePlayer) && !this.Platforms.Contains(value))
                    {
                        result.Add(value.ToString());
                    }
                    else
                    {
                        if(!this.Platforms.Contains(value))
                        {
                            result.Add(value.ToString());
                        }
                    }
                }
                else
                {
                    if(!this.Platforms.Contains(value))
                    {
                        result.Add(value.ToString());
                    }
                }
            }

            return result.ToArray();
        }

        public bool IsGameKeyValid(int index, string value)
        {
            bool valid = true;

            for(int i = 0; i < this.Platforms.Count; ++i)
            {
                if(index != i)
                {
                    if(value.Equals(this.gameKey[i]))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }

        public bool IsSecretKeyValid(int index, string value)
        {
            bool valid = true;

            for(int i = 0; i < this.Platforms.Count; ++i)
            {
                if(index != i)
                {
                    if(value.Equals(this.secretKey[i]))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }

        public static void UpdateKeys(int index, string gameKey, string secretKey)
        {
            GameAnalytics.SettingsGA.gameKey[index] = gameKey;
            GameAnalytics.SettingsGA.secretKey[index] = secretKey;
        }

        public void UpdateGameKey(int index, string value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                bool valid = this.IsGameKeyValid(index, value);

                if(valid)
                {
                    this.gameKey[index] = value;
                }
                else if(this.gameKey[index].Equals(value))
                {
                    this.gameKey[index] = "";
                }
            }
            else
            {
                this.gameKey[index] = value;
            }
        }

        public void UpdateSecretKey(int index, string value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                bool valid = this.IsSecretKeyValid(index, value);

                if(valid)
                {
                    this.secretKey[index] = value;
                }
                else if(this.secretKey[index].Equals(value))
                {
                    this.secretKey[index] = "";
                }
            }
            else
            {
                this.secretKey[index] = value;
            }
        }

        public string GetGameKey(int index)
        {
            return this.gameKey[index];
        }

        public string GetSecretKey(int index)
        {
            return this.secretKey[index];
        }

#endregion
    }

    public class Organization
    {
        public string Name { get; private set; }
        public string ID { get; private set; }
        public List<GameAnalyticsSDK.Setup.Studio> Studios { get; private set; }

        public Organization(string name, string id)
        {
            Name = name;
            ID = id;
            Studios = new List<GameAnalyticsSDK.Setup.Studio>();
        }

        public static string[] GetOrganizationNames(List<GameAnalyticsSDK.Setup.Organization> organizations, bool addFirstEmpty = true)
        {
            if (organizations == null)
            {
                return new string[] { "-" };
            }

            if (addFirstEmpty)
            {
                string[] names = new string[organizations.Count + 1];
                names[0] = "-";

                string spaceAdd = "";
                for (int i = 0; i < organizations.Count; i++)
                {
                    names[i + 1] = organizations[i].Name + spaceAdd;
                    spaceAdd += " ";
                }

                return names;
            }
            else
            {
                string[] names = new string[organizations.Count];

                string spaceAdd = "";
                for (int i = 0; i < organizations.Count; i++)
                {
                    names[i] = organizations[i].Name + spaceAdd;
                    spaceAdd += " ";
                }

                return names;
            }
        }
    }

    //[System.Serializable]
    public class Studio
    {
        public string Name { get; private set; }

        public string ID { get; private set; }

        public string OrganizationID { get; private set; }

        //[SerializeField]
        public List<GameAnalyticsSDK.Setup.Game> Games { get; private set; }

        public Studio(string name, string id, string orgId, List<GameAnalyticsSDK.Setup.Game> games)
        {
            Name = name;
            ID = id;
            OrganizationID = orgId;
            Games = games;
        }

        public static string[] GetStudioNames(List<GameAnalyticsSDK.Setup.Studio> studios, bool addFirstEmpty = true)
        {
            if(studios == null)
            {
                return new string[] { "-" };
            }

            if(addFirstEmpty)
            {
                string[] names = new string[studios.Count + 1];
                names[0] = "-";

                for(int i = 0; i < studios.Count; i++)
                {
                    int j = i + 1;
                    names[j] = j + ". " + studios[i].Name;
                }

                return names;
            }
            else
            {
                string[] names = new string[studios.Count];

                for(int i = 0; i < studios.Count; i++)
                {
                    int j = i + 1;
                    names[i] = j + ". " + studios[i].Name;
                }

                return names;
            }
        }

        public static string[] GetGameNames(int index, List<GameAnalyticsSDK.Setup.Studio> studios)
        {
            if(studios == null || studios[index].Games == null)
            {
                return new string[] { "-" };
            }

            string[] names = new string[studios[index].Games.Count + 1];
            names[0] = "-";

            for(int i = 0; i < studios[index].Games.Count; i++)
            {
                int j = i + 1;
                names[j] = j + ". " + studios[index].Games[i].Name;
            }

            return names;
        }
    }

    public class Game
    {
        public string Name { get; private set; }

        public int ID { get; private set; }

        public string GameKey { get; private set; }

        public string SecretKey { get; private set; }

        public Game(string name, int id, string gameKey, string secretKey)
        {
            Name = name;
            ID = id;
            GameKey = gameKey;
            SecretKey = secretKey;
        }
    }
}
