using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif


namespace GameAnalyticsSDK
{
	[RequireComponent(typeof(GA_SpecialEvents))]

	[ExecuteInEditMode]
	public class GameAnalytics : MonoBehaviour
	{
		#region public values

		private static Settings _settings;

		public static Settings SettingsGA
		{
			get {
				if (_settings == null)
				{
					InitAPI ();
				}
				return _settings;
			}
			private set{ _settings = value; }
		}

		public static GameAnalytics GA;

		#endregion
		
		#region unity derived methods
		
		#if UNITY_EDITOR
		void OnEnable ()
		{
			EditorApplication.hierarchyWindowItemOnGUI += GameAnalytics.HierarchyWindowCallback;
			
			if (Application.isPlaying)
				GA = this;
		}
		
		void OnDisable ()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= GameAnalytics.HierarchyWindowCallback;
		}
		#endif

		public void Awake ()
		{
			if (!Application.isPlaying)
				return;
			
			if (GA != null)
			{
				// only one system tracker allowed per scene
				Debug.LogWarning("Destroying dublicate GameAnalytics object - only one is allowed per scene!");
				Destroy(gameObject);
				return;
			}
			GA = this;

			DontDestroyOnLoad(gameObject);

			#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
			Application.RegisterLogCallback(GA_Debug.HandleLog);
			#else
			Application.logMessageReceived += GA_Debug.HandleLog;
			#endif

			Initialize ();
		}
		
		void OnDestroy()
		{
			if (!Application.isPlaying)
				return;
			
			if (GA == this)
				GA = null;	
		}
		
		#endregion

		private static void InitAPI ()
		{
			try
			{
				_settings = (Settings)Resources.Load("GameAnalytics/Settings", typeof(Settings));
				
				#if UNITY_EDITOR
				if (_settings == null)
				{
					//If the settings asset doesn't exist, then create it. We require a resources folder
					if(!Directory.Exists(Application.dataPath+"/Resources"))
					{
						Directory.CreateDirectory(Application.dataPath+"/Resources");
					}
					if(!Directory.Exists(Application.dataPath+"/Resources/GameAnalytics"))
					{
						Directory.CreateDirectory(Application.dataPath+"/Resources/GameAnalytics");
						Debug.LogWarning("GameAnalytics: Resources/GameAnalytics folder is required to store settings. it was created ");
					}
					
					var asset = ScriptableObject.CreateInstance<Settings>();
					//some hack to mave the asset around
					string path = AssetDatabase.GetAssetPath (Selection.activeObject);
					if (path == "") 
					{
						path = "Assets";
					}
					else if (Path.GetExtension (path) != "") 
					{
						path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
					}
					string uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/GameAnalytics/Settings.asset");
					AssetDatabase.CreateAsset(asset, uniquePath);
					if(uniquePath != "Assets/Resources/GameAnalytics/Settings.asset")
					{
						Debug.LogWarning("GameAnalytics: The path Assets/Resources/GameAnalytics/Settings.asset used to save the settings file is not available.");
					}
					AssetDatabase.SaveAssets ();
					Debug.LogWarning("GameAnalytics: Settings file didn't exist and was created");
					Selection.activeObject = asset;
					
					//save reference
					_settings =	asset;
				}
				#endif
			}
			catch (System.Exception e)
			{
				Debug.Log("Error getting Settings in InitAPI: " + e.Message);
			}
		}

		private static void Initialize ()
		{
			if (!Application.isPlaying)
				return; // no need to setup anything else if we are in the editor and not playing

			if (SettingsGA.InfoLogBuild)
			{
				GA_Setup.SetInfoLog(true);
			}

			if (SettingsGA.VerboseLogBuild)
			{
				GA_Setup.SetVerboseLog(true);
			}

			GA_iOSWrapper.SetUnitySdkVersion("unity " + Settings.VERSION);
			GA_iOSWrapper.SetUnityEngineVersion("unity " + GetUnityVersion());
			GA_iOSWrapper.SetBuild(SettingsGA.Build);

			if (SettingsGA.CustomDimensions01.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions01(SettingsGA.CustomDimensions01);
			}

			if (SettingsGA.CustomDimensions02.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions02(SettingsGA.CustomDimensions02);
			}

			if (SettingsGA.CustomDimensions03.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions03(SettingsGA.CustomDimensions03);
			}

			if (SettingsGA.ResourceItemTypes.Count > 0)
			{
				GA_Setup.SetAvailableResourceItemTypes(SettingsGA.ResourceItemTypes);
			}

			if (SettingsGA.ResourceCurrencies.Count > 0)
			{
				GA_Setup.SetAvailableResourceCurrencies(SettingsGA.ResourceCurrencies);
			}

			GA_iOSWrapper.Initialize(SettingsGA.GameKey, SettingsGA.SecretKey);
		}

		/// <summary>
		/// Track any real money transaction in-game. Additionally fetch and attach the in-app purchase receipt.
		/// </summary>
		/// <param name="currency">Currency code in ISO 4217 format. (e.g. USD).</param>
		/// <param name="amount">Amount in cents (int). (e.g. 99).</param>
		/// <param name="itemType">Item Type bought. (e.g. Gold Pack).</param>
		/// <param name="itemId">Item bought. (e.g. 1000 gold).</param>
		/// <param name="cartType">Cart type.</param>
		/// <param name="receipt">Transaction receipt string. (Optional, can be nil).</param>
		/// <param name="autoFetchReceipt">If true the SDK will ignore the receipt parameter and attempt to automatically get the receipt.</param>
		public static void NewBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType, string receipt, bool autoFetchReceipt)
		{
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType, receipt, autoFetchReceipt);
		}

		/// <summary>
		/// Track any type of design event that you want to measure i.e. GUI elements or tutorial steps. Custom dimensions are not supported.
		/// </summary>
		/// <param name="eventName">String can consist of 1 to 5 segments. Segments are seperated by ':' and segments can have a max length of 16. (e.g. segment1:anotherSegment:gold).</param>
		public static void NewDesignEvent (string eventName)
		{
			GA_Design.NewEvent(eventName);
		}

		/// <summary>
		/// Track any type of design event that you want to measure i.e. GUI elements or tutorial steps. Custom dimensions are not supported.
		/// </summary>
		/// <param name="eventName">String can consist of 1 to 5 segments. Segments are seperated by ':' and segments can have a max length of 16. (e.g. segment1:anotherSegment:gold).</param>
		/// <param name="eventValue">Number value of event.</param>
		public static void NewDesignEvent (string eventName, float eventValue)
		{
			GA_Design.NewEvent(eventName, eventValue);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01)
		{
			GA_Progression.NewEvent(progressionStatus, progression01);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// <param name="progression02">2nd progression (e.g. level01).</param>
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02)
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
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03);
		}

		/// <summary>
		/// Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.
		/// </summary>
		/// <param name="progressionStatus">Status of added progression.</param>
		/// <param name="progression01">1st progression (e.g. world01).</param>
		/// /// <param name="score">The player's score.</param>
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, int score)
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
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
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
		public static void NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
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
		public static void NewResourceEvent (GA_Resource.GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			GA_Resource.NewEvent(flowType, currency, amount, itemType, itemId);
		}

		/// <summary>
		/// Set up custom error events in the game. You can group the events by severity level and attach a message.
		/// </summary>
		/// <param name="severity">Severity of error.</param>
		/// <param name="message">Error message (Optional, can be nil).</param>
		public static void NewErrorEvent (GA_Error.GAErrorSeverity severity, string message)
		{
			GA_Error.NewEvent(severity, message);
		}

		/// <summary>
		/// Set user facebook id.
		/// </summary>
		/// <param name="facebookId">Facebook id of user (Persists cross session).</param>
		public static void SetFacebookId (string facebookId)
		{
			GA_Setup.SetFacebookId(facebookId);
		}

		/// <summary>
		/// Set user gender.
		/// </summary>
		/// <param name="gender">Gender of user (Persists cross session).</param>
		public static void SetGender (GA_Setup.GAGender gender)
		{
			GA_Setup.SetGender(gender);
		}

		/// <summary>
		/// Set user birth year.
		/// </summary>
		/// <param name="birthYear">Birth year of user (Persists cross session).</param>
		public static void SetBirthYear (int birthYear)
		{
			GA_Setup.SetBirthYear(birthYear);
		}

		/// <summary>
		/// Set 1st custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension01 (string customDimension)
		{
			GA_Setup.SetCustomDimension01(customDimension);
		}

		/// <summary>
		/// Set 2nd custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension02 (string customDimension)
		{
			GA_Setup.SetCustomDimension02(customDimension);
		}

		/// <summary>
		/// Set 3rd custom dimension.
		/// </summary>
		/// <param name="customDimension">One of the available dimension values set in Settings (Setup tab). Will persist cross session. Set to null to remove again.</param>
		public static void SetCustomDimension03 (string customDimension)
		{
			GA_Setup.SetCustomDimension03(customDimension);
		}

		private static string GetUnityVersion ()
		{
			string unityVersion = "";
			string[] splitUnityVersion = Application.unityVersion.Split('.');
			for (int i = 0; i < splitUnityVersion.Length; i++)
			{
				int result;
				if (int.TryParse(splitUnityVersion[i], out result))
				{
					if (i == 0)
						unityVersion = splitUnityVersion[i];
					else
						unityVersion += "." + splitUnityVersion[i];
				}
				else
				{
					string[] regexVersion = System.Text.RegularExpressions.Regex.Split(splitUnityVersion[i], "[^\\d]+");
					if (regexVersion.Length > 0 && int.TryParse(regexVersion[0], out result))
					{
						unityVersion += "." + regexVersion[0];
					}
				}
			}

			return unityVersion;
		}

		#if UNITY_EDITOR
		
		public static void HierarchyWindowCallback (int instanceID, Rect selectionRect)
		{
			GameObject go = (GameObject)EditorUtility.InstanceIDToObject(instanceID);
			if (go != null && go.GetComponent<GameAnalytics>() != null)
			{
				float addX = 0;
				if (go.GetComponent("PlayMakerFSM") != null)
					addX = selectionRect.height + 2;
				
				if (GameAnalytics.SettingsGA.Logo == null)
				{
					GameAnalytics.SettingsGA.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/Gizmos/GameAnalytics/gaLogo.png", typeof(Texture2D));
				}
				
				Graphics.DrawTexture(new Rect(GUILayoutUtility.GetLastRect().width - selectionRect.height - 5 - addX, selectionRect.y, selectionRect.height, selectionRect.height), GameAnalytics.SettingsGA.Logo);
			}
		}
		
		#endif
	}
}
