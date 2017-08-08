using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.Setup;
using GameAnalyticsSDK.Wrapper;
using GameAnalyticsSDK.State;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif



namespace GameAnalyticsSDK
{
	[RequireComponent(typeof(GA_SpecialEvents))]
	[ExecuteInEditMode]
	public partial class GameAnalytics : MonoBehaviour
	{
		#region public values

		private static Settings _settings;

		public static Settings SettingsGA
		{
			get
			{
				if(_settings == null)
				{
					InitAPI();
				}
				return _settings;
			}
			private set{ _settings = value; }
		}

		private static GameAnalytics _instance;

		#endregion

		#region unity derived methods

		#if UNITY_EDITOR
		void OnEnable()
		{
			EditorApplication.hierarchyWindowItemOnGUI += GameAnalytics.HierarchyWindowCallback;

			if(Application.isPlaying)
				_instance = this;
		}

		void OnDisable()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= GameAnalytics.HierarchyWindowCallback;
		}
		#endif

		public void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			if(_instance != null)
			{
				// only one system tracker allowed per scene
				Debug.LogWarning("Destroying duplicate GameAnalytics object - only one is allowed per scene!");
				Destroy(gameObject);
				return;
			}
			_instance = this;

			DontDestroyOnLoad(gameObject);

			Application.logMessageReceived += GA_Debug.HandleLog;

            Initialize();
		}

		void OnDestroy()
		{
			if(!Application.isPlaying)
				return;

			if(_instance == this)
				_instance = null;
		}

		void OnApplicationPause(bool pauseStatus)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass ga = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			if (pauseStatus) {
				ga.CallStatic("onActivityPaused", activity);
			}
			else {
				ga.CallStatic("onActivityResumed", activity);
			}
#endif
		}

		void OnApplicationQuit()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			if(!SettingsGA.UseManualSessionHandling)
			{
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaClass ga = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
				ga.CallStatic("onActivityStopped", activity);
			}
#elif (!UNITY_EDITOR && !UNITY_IOS && !UNITY_ANDROID && !UNITY_TVOS && !UNITY_WEBGL && !UNITY_TIZEN)
			if(!SettingsGA.UseManualSessionHandling)
			{
				GameAnalyticsSDK.Net.GameAnalytics.OnStop();
			}
#endif
		}

#endregion

		private static void InitAPI()
		{
			try
			{
				_settings = (Settings)Resources.Load("GameAnalytics/Settings", typeof(Settings));
				GameAnalyticsSDK.State.GAState.Init();

#if UNITY_EDITOR
				if(_settings == null)
				{
					//If the settings asset doesn't exist, then create it. We require a resources folder
					if(!Directory.Exists(Application.dataPath + "/Resources"))
					{
						Directory.CreateDirectory(Application.dataPath + "/Resources");
					}
					if(!Directory.Exists(Application.dataPath + "/Resources/GameAnalytics"))
					{
						Directory.CreateDirectory(Application.dataPath + "/Resources/GameAnalytics");
						Debug.LogWarning("GameAnalytics: Resources/GameAnalytics folder is required to store settings. it was created ");
					}

					const string path = "Assets/Resources/GameAnalytics/Settings.asset";

					if(File.Exists(path))
					{
						AssetDatabase.DeleteAsset(path);
						AssetDatabase.Refresh();
					}

					var asset = ScriptableObject.CreateInstance<Settings>();
					AssetDatabase.CreateAsset(asset, path);
					AssetDatabase.Refresh();

					AssetDatabase.SaveAssets();
					Debug.LogWarning("GameAnalytics: Settings file didn't exist and was created");
					Selection.activeObject = asset;

					//save reference
					_settings =	asset;
				}
#endif
			}
			catch(System.Exception e)
			{
				Debug.Log("Error getting Settings in InitAPI: " + e.Message);
			}
		}

		private static void Initialize()
		{
			if(!Application.isPlaying)
				return; // no need to setup anything else if we are in the editor and not playing

			if(SettingsGA.InfoLogBuild)
			{
				GA_Setup.SetInfoLog(true);
			}

			if(SettingsGA.VerboseLogBuild)
			{
				GA_Setup.SetVerboseLog(true);
			}

			int platformIndex = GetPlatformIndex();

			GA_Wrapper.SetUnitySdkVersion("unity " + Settings.VERSION);
			GA_Wrapper.SetUnityEngineVersion("unity " + GetUnityVersion());

			if(platformIndex >= 0)
			{
				if (GameAnalytics.SettingsGA.UsePlayerSettingsBuildNumber) {
					for (int i = 0; i < GameAnalytics.SettingsGA.Platforms.Count; ++i) {
						if (GameAnalytics.SettingsGA.Platforms [i] == RuntimePlatform.Android || GameAnalytics.SettingsGA.Platforms [i] == RuntimePlatform.IPhonePlayer) {
							GameAnalytics.SettingsGA.Build [i] = Application.version;
						}
					}
				}
				GA_Wrapper.SetBuild (SettingsGA.Build [platformIndex]);
			}

			if(SettingsGA.CustomDimensions01.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions01(SettingsGA.CustomDimensions01);
			}

			if(SettingsGA.CustomDimensions02.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions02(SettingsGA.CustomDimensions02);
			}

			if(SettingsGA.CustomDimensions03.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions03(SettingsGA.CustomDimensions03);
			}

			if(SettingsGA.ResourceItemTypes.Count > 0)
			{
				GA_Setup.SetAvailableResourceItemTypes(SettingsGA.ResourceItemTypes);
			}

			if(SettingsGA.ResourceCurrencies.Count > 0)
			{
				GA_Setup.SetAvailableResourceCurrencies(SettingsGA.ResourceCurrencies);
			}

			if(SettingsGA.UseManualSessionHandling)
			{
				SetEnabledManualSessionHandling(true);
			}

			if(platformIndex >= 0)
			{
				if (!SettingsGA.UseCustomId)
				{
					GA_Wrapper.Initialize (SettingsGA.GetGameKey (platformIndex), SettingsGA.GetSecretKey (platformIndex));
				}
				else
				{
					Debug.Log ("Custom id is enabled. Initialize is delayed until custom id has been set.");
				}
			}
			else
			{
				Debug.LogWarning("GameAnalytics: Unsupported platform (events will not be sent in editor; or missing platform in settings): " + Application.platform);
			}
		}

        /// <summary>
        /// Track any real money transaction in-game.
        /// </summary>
        /// <param name="currency">Currency code in ISO 4217 format. (e.g. USD).</param>
        /// <param name="amount">Amount in cents (int). (e.g. 99).</param>
        /// <param name="itemType">Item Type bought. (e.g. Gold Pack).</param>
        /// <param name="itemId">Item bought. (e.g. 1000 gold).</param>
        /// <param name="cartType">Cart type.</param>
        public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
        {
            GA_Business.NewEvent(currency, amount, itemType, itemId, cartType);
        }

#if UNITY_IOS || UNITY_TVOS
		/// <summary>
		/// Track any real money transaction in-game (iOS version).
		/// </summary>
		/// <param name="currency">Currency code in ISO 4217 format. (e.g. USD).</param>
		/// <param name="amount">Amount in cents (int). (e.g. 99).</param>
		/// <param name="itemType">Item Type bought. (e.g. Gold Pack).</param>
		/// <param name="itemId">Item bought. (e.g. 1000 gold).</param>
		/// <param name="cartType">Cart type.</param>
		/// <param name="receipt">Transaction receipt string.</param>
		public static void NewBusinessEventIOS(string currency, int amount, string itemType, string itemId, string cartType, string receipt)
		{
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType, receipt, false);
		}

		/// <summary>
		/// Track any real money transaction in-game (iOS version). Additionally fetch and attach the in-app purchase receipt.
		/// </summary>
		/// <param name="currency">Currency code in ISO 4217 format. (e.g. USD).</param>
		/// <param name="amount">Amount in cents (int). (e.g. 99).</param>
		/// <param name="itemType">Item Type bought. (e.g. Gold Pack).</param>
		/// <param name="itemId">Item bought. (e.g. 1000 gold).</param>
		/// <param name="cartType">Cart type.</param>
		public static void NewBusinessEventIOSAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType)
		{
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType, null, true);
		}

#elif UNITY_ANDROID
		/// <summary>
        /// Track any real money transaction in-game (Google Play version).
        /// </summary>
        /// <param name="currency">Currency code in ISO 4217 format. (e.g. USD).</param>
        /// <param name="amount">Amount in cents (int). (e.g. 99).</param>
        /// <param name="itemType">Item Type bought. (e.g. Gold Pack).</param>
        /// <param name="itemId">Item bought. (e.g. 1000 gold).</param>
        /// <param name="cartType">Cart type.</param>
        /// <param name="receipt">Transaction receipt string.</param>
        /// <param name="signature">Signature of transaction.</param>
        public static void NewBusinessEventGooglePlay(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string signature)
        {
            GA_Business.NewEventGooglePlay(currency, amount, itemType, itemId, cartType, receipt, signature);
        }
#endif

		/// <summary>
		/// Track any type of design event that you want to measure i.e. GUI elements or tutorial steps. Custom dimensions are not supported.
		/// </summary>
		/// <param name="eventName">String can consist of 1 to 5 segments. Segments are seperated by ':' and segments can have a max length of 16. (e.g. segment1:anotherSegment:gold).</param>
		public static void NewDesignEvent(string eventName)
		{
			GA_Design.NewEvent(eventName);
		}

		/// <summary>
		/// Track any type of design event that you want to measure i.e. GUI elements or tutorial steps. Custom dimensions are not supported.
		/// </summary>
		/// <param name="eventName">String can consist of 1 to 5 segments. Segments are seperated by ':' and segments can have a max length of 16. (e.g. segment1:anotherSegment:gold).</param>
		/// <param name="eventValue">Number value of event.</param>
		public static void NewDesignEvent(string eventName, float eventValue)
		{
			GA_Design.NewEvent(eventName, eventValue);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01)
		{
			GA_Progression.NewEvent(progressionStatus, progression01);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// <param name="progression02">2nd progression (e.g. level01).</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// <param name="progression02">2nd progression (e.g. level01).</param>
		/// <param name="progression03">3rd progression (e.g. phase01).</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// /// <param name="score">The player's score.</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, score);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// <param name="progression02">2nd progression (e.g. level01).</param>
		/// /// <param name="score">The player's score.</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, score);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// <param name="progression02">2nd progression (e.g. level01).</param>
		/// <param name="progression03">3rd progression (e.g. phase01).</param>
		/// <param name="score">The player's score.</param>
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, score);
		}

		/// <summary>
		/// Analyze your in-game economy (virtual currencies). You will be able to see the flow (sink/source) for each virtual currency.
		/// </summary>
		/// <param name="flowType">Add or substract resource.</param>
		/// <param name="currency">One of the available currencies set in Settings (Setup tab).</param>
		/// <param name="amount">Amount sourced or sinked.</param>
		/// <param name="itemType">One of the available currencies set in Settings (Setup tab).</param>
		/// <param name="itemId">Item id (string max length=16).</param>
		public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			GA_Resource.NewEvent(flowType, currency, amount, itemType, itemId);
		}

		/// <summary>
		/// Set up custom error events in the game. You can group the events by severity level and attach a message.
		/// </summary>
		/// <param name="severity">Severity of error.</param>
		/// <param name="message">Error message (Optional, can be nil).</param>
		public static void NewErrorEvent(GAErrorSeverity severity, string message)
		{
			GA_Error.NewEvent(severity, message);
		}

		/// <summary>
		/// Set user facebook id.
		/// </summary>
		/// <param name="facebookId">Facebook id of user (Persists cross session).</param>
		public static void SetFacebookId(string facebookId)
		{
			GA_Setup.SetFacebookId(facebookId);
		}

		/// <summary>
		/// Set user gender.
		/// </summary>
		/// <param name="gender">Gender of user (Persists cross session).</param>
		public static void SetGender(GAGender gender)
		{
			GA_Setup.SetGender(gender);
		}

		/// <summary>
		/// Set user birth year.
		/// </summary>
		/// <param name="birthYear">Birth year of user (Persists cross session).</param>
		public static void SetBirthYear(int birthYear)
		{
			GA_Setup.SetBirthYear(birthYear);
		}

		/// <summary>
		/// Sets the custom identifier.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		public static void SetCustomId(string userId)
		{
			if (SettingsGA.UseCustomId)
			{
				Debug.Log ("Initializing with custom id: " + userId);
				GA_Wrapper.SetCustomUserId (userId);
				int index = GetPlatformIndex();
				if(index >= 0)
				{
					GA_Wrapper.Initialize (SettingsGA.GetGameKey (index), SettingsGA.GetSecretKey (index));
				}
				else
				{
					Debug.LogWarning("Unsupported platform (or missing platform in settings): " + Application.platform);
				}
			}
			else
			{
				Debug.LogWarning ("Custom id is not enabled");
			}
		}

		/// <summary>
		/// Sets the enabled manual session handling.
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.SetEnabledManualSessionHandling(enabled);
		}

		/// <summary>
		/// Starts the session.
		/// </summary>
		public static void StartSession()
		{
			GA_Wrapper.StartSession();
		}

		/// <summary>
		/// Ends the session.
		/// </summary>
		public static void EndSession()
		{
			GA_Wrapper.EndSession();
		}

		/// <summary>
		/// Set 1st custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension01(string customDimension)
		{
			GA_Setup.SetCustomDimension01(customDimension);
		}

		/// <summary>
		/// Set 2nd custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension02(string customDimension)
		{
			GA_Setup.SetCustomDimension02(customDimension);
		}

		/// <summary>
		/// Set 3rd custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension03(string customDimension)
		{
			GA_Setup.SetCustomDimension03(customDimension);
		}

		private static string GetUnityVersion()
		{
			string unityVersion = "";
			string[] splitUnityVersion = Application.unityVersion.Split('.');
			for(int i = 0; i < splitUnityVersion.Length; i++)
			{
				int result;
				if(int.TryParse(splitUnityVersion[i], out result))
				{
					if(i == 0)
						unityVersion = splitUnityVersion[i];
					else
						unityVersion += "." + splitUnityVersion[i];
				}
				else
				{
					string[] regexVersion = System.Text.RegularExpressions.Regex.Split(splitUnityVersion[i], "[^\\d]+");
					if(regexVersion.Length > 0 && int.TryParse(regexVersion[0], out result))
					{
						unityVersion += "." + regexVersion[0];
					}
				}
			}

			return unityVersion;
		}

		private static int GetPlatformIndex()
		{
			int result = -1;

			RuntimePlatform platform = Application.platform;

			if(platform == RuntimePlatform.IPhonePlayer)
			{
				if(!SettingsGA.Platforms.Contains(platform))
				{
					result = SettingsGA.Platforms.IndexOf(RuntimePlatform.tvOS);
				}
				else
				{
					result = SettingsGA.Platforms.IndexOf(platform);
				}
			}
			else if(platform == RuntimePlatform.tvOS)
			{
				if(!SettingsGA.Platforms.Contains(platform))
				{
					result = SettingsGA.Platforms.IndexOf(RuntimePlatform.IPhonePlayer);
				}
				else
				{
					result = SettingsGA.Platforms.IndexOf(platform);
				}
			}
			// HACK: To also check for RuntimePlatform.MetroPlayerARM, RuntimePlatform.MetroPlayerX64 and RuntimePlatform.MetroPlayerX86 which are deprecated but have same value as the WSA enums
            else if (platform == RuntimePlatform.WSAPlayerARM || platform == RuntimePlatform.WSAPlayerX64 || platform == RuntimePlatform.WSAPlayerX86 ||
                ((int)platform == (int)RuntimePlatform.WSAPlayerARM) || ((int)platform == (int)RuntimePlatform.WSAPlayerX64) || ((int)platform == (int)RuntimePlatform.WSAPlayerX86))
            {
				result = SettingsGA.Platforms.IndexOf(RuntimePlatform.WSAPlayerARM);
            }
            else
			{
				result = SettingsGA.Platforms.IndexOf(platform);
			}

			return result;
		}

#if UNITY_EDITOR

		/// <summary>
		/// Dynamic search for a file.
		/// </summary>
		/// <returns>Returns the Unity path to a specified file.</returns>
		/// <param name="">File name including extension e.g. image.png</param>
		public static string WhereIs(string _file)
		{
#if UNITY_SAMSUNGTV
			return "";
#else
			string[] assets = { Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar};
			FileInfo[] myFile = new DirectoryInfo ("Assets").GetFiles (_file, SearchOption.AllDirectories);
			string[] temp = myFile [0].ToString ().Split (assets, 2, System.StringSplitOptions.None);
			return "Assets" + Path.DirectorySeparatorChar + temp [1];
#endif
		}

		public static void HierarchyWindowCallback(int instanceID, Rect selectionRect)
		{
			GameObject go = (GameObject)EditorUtility.InstanceIDToObject(instanceID);
			if(go != null && go.GetComponent<GameAnalytics>() != null)
			{
				float addX = 0;
				if(go.GetComponent("PlayMakerFSM") != null)
					addX = selectionRect.height + 2;

				if(GameAnalytics.SettingsGA.Logo == null)
				{
					GameAnalytics.SettingsGA.Logo = (Texture2D)AssetDatabase.LoadAssetAtPath(WhereIs("gaLogo.png"), typeof(Texture2D));
				}

				Graphics.DrawTexture(new Rect(GUILayoutUtility.GetLastRect().width - selectionRect.height - 5 - addX, selectionRect.y, selectionRect.height, selectionRect.height), GameAnalytics.SettingsGA.Logo);
			}
		}

#endif
        /// <summary>
		/// Sets the build for all platforms.
		/// </summary>
		/// <param name="build">Build.</param>
		public static void SetBuildAllPlatforms(string build)
        {
            for (int i = 0; i < GameAnalytics.SettingsGA.Build.Count; i++)
            {
                GameAnalytics.SettingsGA.Build[i] = build;
            }
        }
    }
}
