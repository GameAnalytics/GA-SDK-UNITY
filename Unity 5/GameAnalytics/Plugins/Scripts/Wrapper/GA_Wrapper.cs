using UnityEngine;
using System.Collections;
using GameAnalyticsSDK.Validators;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper
	{
		#if (UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID && !UNITY_TVOS && !UNITY_STANDALONE && !UNITY_WEBGL && !UNITY_WSA && !UNITY_WP_8_1 && !UNITY_TIZEN && !UNITY_SAMSUNGTV))

		private static void configureAvailableCustomDimensions01 (string list)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setAvailableCustomDimensions01(" + list + ")");
			}
		}

		private static void configureAvailableCustomDimensions02 (string list)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setAvailableCustomDimensions02(" + list + ")");
			}
		}

		private static void configureAvailableCustomDimensions03 (string list)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setAvailableCustomDimensions03(" + list + ")");
			}
		}

		private static void configureAvailableResourceCurrencies (string list)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setAvailableResourceCurrencies(" + list + ")");
			}
		}

		private static void configureAvailableResourceItemTypes (string list)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setAvailableResourceItemTypes(" + list + ")");
			}
		}

		private static void configureSdkGameEngineVersion (string unitySdkVersion)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("GameAnalytics SDK version: " + unitySdkVersion);
			}
		}

		private static void configureGameEngineVersion (string unityEngineVersion)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				//Debug.Log ("configureGameEngineVersion(" + unityEngineVersion + ")");
			}
		}

		private static void configureBuild (string build)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setBuild(" + build + ")");
			}
		}

		private static void configureUserId (string userId)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("configureUserId(" + userId + ")");
			}
		}


		private static void initialize (string gamekey, string gamesecret)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("initialize(" + gamekey + "," + gamesecret + ")");
			}
		}

		private static void setCustomDimension01 (string customDimension)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setCustomDimension01(" + customDimension + ")");
			}
		}

		private static void setCustomDimension02 (string customDimension)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setCustomDimension02(" + customDimension + ")");
			}
		}

		private static void setCustomDimension03 (string customDimension)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setCustomDimension03(" + customDimension + ")");
			}
		}

		#if UNITY_IOS || UNITY_TVOS
		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor)
			{
				Debug.Log("addBusinessEvent("+currency+","+amount+","+itemType+","+itemId+","+cartType+","+receipt+")");
			}
		}

		private static void addBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor)
			{
				Debug.Log("addBusinessEventAndAutoFetchReceipt("+currency+","+amount+","+itemType+","+itemId+","+cartType+")");
			}
		}




#elif UNITY_ANDROID
		private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor)
			{
				Debug.Log("addBusinessEventWithReceipt("+currency+","+amount+","+itemType+","+itemId+","+cartType+","+receipt+","+store+","+signature+")");
			}
		}
#endif

		#if !UNITY_IOS && !UNITY_TVOS
		private static void addBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addBusinessEvent(" + currency + "," + amount + "," + itemType + "," + itemId + "," + cartType + ")");
			}
		}
		#endif

		private static void addResourceEvent (int flowType, string currency, float amount, string itemType, string itemId)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addResourceEvent(" + flowType + "," + currency + "," + amount + "," + itemType + "," + itemId + ")");
			}
		}

		private static void addProgressionEvent (int progressionStatus, string progression01, string progression02, string progression03)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + ")");
			}
		}

		private static void addProgressionEventWithScore (int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + "," + score + ")");
			}
		}

		private static void addDesignEvent (string eventId)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addDesignEvent(" + eventId + ")");
			}
		}

		private static void addDesignEventWithValue (string eventId, float value)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addDesignEventWithValue(" + eventId + "," + value + ")");
			}
		}

		private static void addErrorEvent (int severity, string message)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("addErrorEvent(" + severity + "," + message + ")");
			}
		}

		private static void setEnabledInfoLog (bool enabled)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("GameAnalytics setInfoLog(" + enabled + ")\nInfo logs can be deactivated in the Advanced section of the Settings object.");
			}
		}

		private static void setEnabledVerboseLog (bool enabled)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("GameAnalytics setVerboseLog(" + enabled + ")");
			}
		}

		private static void setFacebookId (string facebookId)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setFacebookId(" + facebookId + ")");
			}
		}

		private static void setGender (string gender)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setGender(" + gender + ")");
			}
		}

		private static void setBirthYear (int birthYear)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setBirthYear(" + birthYear + ")");
			}
		}

		private static void setManualSessionHandling (bool enabled)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("setManualSessionHandling(" + enabled + ")");
			}
		}

		private static void setUsePlayerSettingsBundleVersionForBuild (bool enabled)
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("Using Player Settings bundle version for build(" + enabled + ")");
			}
		}

		private static void gameAnalyticsStartSession ()
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("gameAnalyticsStartSession()");
			}
		}

		private static void gameAnalyticsEndSession ()
		{
			if (GameAnalytics.SettingsGA.InfoLogEditor) {
				Debug.Log ("gameAnalyticsEndSession()");
			}
		}
			
		#endif

		public static void SetAvailableCustomDimensions01 (string list)
		{
			configureAvailableCustomDimensions01 (list);
		}

		public static void SetAvailableCustomDimensions02 (string list)
		{
			configureAvailableCustomDimensions02 (list);
		}

		public static void SetAvailableCustomDimensions03 (string list)
		{
			configureAvailableCustomDimensions03 (list);
		}

		public static void SetAvailableResourceCurrencies (string list)
		{
			configureAvailableResourceCurrencies (list);
		}

		public static void SetAvailableResourceItemTypes (string list)
		{
			configureAvailableResourceItemTypes (list);
		}

		public static void SetUnitySdkVersion (string unitySdkVersion)
		{
			configureSdkGameEngineVersion (unitySdkVersion);
		}

		public static void SetUnityEngineVersion (string unityEngineVersion)
		{
			configureGameEngineVersion (unityEngineVersion);
		}

		public static void SetBuild (string build)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateBuild (build)) {
				configureBuild (build);
			}
#else
				configureBuild (build);
#endif
        }

		public static void SetCustomUserId (string userId)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateUserId (userId)) {
				configureUserId (userId);
			}
#else
				configureUserId (userId);
#endif
        }

		public static void SetEnabledManualSessionHandling (bool enabled)
		{
			setManualSessionHandling (enabled);
		}

		public static void StartSession ()
		{
			if (GameAnalyticsSDK.State.GAState.IsManualSessionHandlingEnabled()) {
				gameAnalyticsStartSession ();
			} else {
				Debug.Log ("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void EndSession ()
		{
			if (GameAnalyticsSDK.State.GAState.IsManualSessionHandlingEnabled()) {
				gameAnalyticsEndSession ();
			} else {
				Debug.Log ("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void Initialize (string gamekey, string gamesecret)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateKeys (gamekey, gamesecret)) {
				initialize (gamekey, gamesecret);
			}
#else
				initialize (gamekey, gamesecret);
#endif
        }

		public static void SetCustomDimension01 (string customDimension)
		{
			setCustomDimension01 (customDimension);
        }

		public static void SetCustomDimension02 (string customDimension)
		{

			setCustomDimension02 (customDimension);
        }

		public static void SetCustomDimension03 (string customDimension)
		{
			setCustomDimension03 (customDimension);
        }
		
#if UNITY_IOS || UNITY_TVOS
		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt)
		{
			addBusinessEvent(currency, amount, itemType, itemId, cartType, receipt);
		}

		public static void AddBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType)
		{
			addBusinessEventAndAutoFetchReceipt(currency, amount, itemType, itemId, cartType);
		}


#elif UNITY_ANDROID
		public static void AddBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature)
		{
			addBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, store, signature);
		}
#endif

#if !UNITY_IOS && !UNITY_TVOS 
		public static void AddBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateBusinessEvent (currency, amount, cartType, itemType, itemId)) {
				addBusinessEvent (currency, amount, itemType, itemId, cartType);
			}
#else
				addBusinessEvent (currency, amount, itemType, itemId, cartType);
#endif
        }
#endif

        public static void AddResourceEvent (GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateResourceEvent (flowType, currency, amount, itemType, itemId)) {
				addResourceEvent ((int)flowType, currency, amount, itemType, itemId);
			}
#else
				addResourceEvent ((int)flowType, currency, amount, itemType, itemId);
#endif
        }

		public static void AddProgressionEvent (GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateProgressionEvent (progressionStatus, progression01, progression02, progression03)) {
				addProgressionEvent ((int)progressionStatus, progression01, progression02, progression03);
			}
#else
				addProgressionEvent ((int)progressionStatus, progression01, progression02, progression03);
#endif
        }

		public static void AddProgressionEventWithScore (GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateProgressionEvent (progressionStatus, progression01, progression02, progression03)) {
				addProgressionEventWithScore ((int)progressionStatus, progression01, progression02, progression03, score);
			}
#else
				addProgressionEventWithScore ((int)progressionStatus, progression01, progression02, progression03, score);
#endif
        }

		public static void AddDesignEvent (string eventID, float eventValue)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateDesignEvent (eventID)) {
				addDesignEventWithValue (eventID, eventValue);
			}
#else
				addDesignEventWithValue (eventID, eventValue);
#endif
        }

		public static void AddDesignEvent (string eventID)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateDesignEvent (eventID)) {
				addDesignEvent (eventID);
			}
#else
				addDesignEvent (eventID);
#endif
        }

		public static void AddErrorEvent (GAErrorSeverity severity, string message)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateErrorEvent(severity,message)) {
				addErrorEvent ((int)severity, message);
			}
#else
				addErrorEvent ((int)severity, message);
#endif
        }

		public static void SetInfoLog (bool enabled)
		{
			setEnabledInfoLog (enabled);
		}

		public static void SetVerboseLog (bool enabled)
		{
			setEnabledVerboseLog (enabled);
		}

		public static void SetFacebookId (string facebookId)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateFacebookId (facebookId)) {
				setFacebookId (facebookId);
			}
#else
				setFacebookId (facebookId);
#endif
        }

		public static void SetGender (string gender)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateGender (gender)) {
				setGender (gender);
			}
#else
				setGender (gender);
#endif
        }

		public static void SetBirthYear (int birthYear)
		{
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateBirthyear (birthYear)) {
				setBirthYear (birthYear);
			}
#else
				setBirthYear (birthYear);
#endif
        }
	}
}