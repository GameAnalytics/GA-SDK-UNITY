using UnityEngine;
using System.Collections;
using GameAnalyticsSDK.Validators;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;

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
        private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor)
            {
                Debug.Log("addBusinessEvent("+currency+","+amount+","+itemType+","+itemId+","+cartType+","+receipt+")");
            }
        }

        private static void addBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor)
            {
                Debug.Log("addBusinessEventAndAutoFetchReceipt("+currency+","+amount+","+itemType+","+itemId+","+cartType+")");
            }
        }




#elif UNITY_ANDROID
        private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor)
            {
                Debug.Log("addBusinessEventWithReceipt("+currency+","+amount+","+itemType+","+itemId+","+cartType+","+receipt+","+store+","+signature+")");
            }
        }
#endif

#if !UNITY_IOS && !UNITY_TVOS
        private static void addBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addBusinessEvent(" + currency + "," + amount + "," + itemType + "," + itemId + "," + cartType + ")");
            }
        }
        #endif

        private static void addResourceEvent (int flowType, string currency, float amount, string itemType, string itemId, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addResourceEvent(" + flowType + "," + currency + "," + amount + "," + itemType + "," + itemId + ")");
            }
        }

        private static void addProgressionEvent (int progressionStatus, string progression01, string progression02, string progression03, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + ")");
            }
        }

        private static void addProgressionEventWithScore (int progressionStatus, string progression01, string progression02, string progression03, int score, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + "," + score + ")");
            }
        }

        private static void addDesignEvent (string eventId, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addDesignEvent(" + eventId + ")");
            }
        }

        private static void addDesignEventWithValue (string eventId, float value, string fields)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("addDesignEventWithValue(" + eventId + "," + value + ")");
            }
        }

        private static void addErrorEvent (int severity, string message, string fields)
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

        // ----------------------- COMMAND CENTER ---------------------- //
        private static string getCommandCenterValueAsString(string key, string defaultValue)
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("getCommandCenterValueAsString()");
            }
            return defaultValue;
        }

        private static bool isCommandCenterReady ()
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("isCommandCenterReady()");
            }
            return false;
        }

        private static string getConfigurationsContentAsString()
        {
            if (GameAnalytics.SettingsGA.InfoLogEditor) {
                Debug.Log ("getConfigurationsContentAsString()");
            }
            return "";
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
        public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
            addBusinessEvent(currency, amount, itemType, itemId, cartType, receipt, fieldsAsString);
        }

        public static void AddBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
            addBusinessEventAndAutoFetchReceipt(currency, amount, itemType, itemId, cartType, fieldsAsString);
        }


#elif UNITY_ANDROID
        public static void AddBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
            addBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, store, signature, fieldsAsString);
        }
#endif

#if !UNITY_IOS && !UNITY_TVOS
        public static void AddBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateBusinessEvent (currency, amount, cartType, itemType, itemId)) {
                addBusinessEvent (currency, amount, itemType, itemId, cartType, fieldsAsString);
            }
#else
                addBusinessEvent (currency, amount, itemType, itemId, cartType, fieldsAsString);
#endif
        }
#endif

        public static void AddResourceEvent (GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateResourceEvent (flowType, currency, amount, itemType, itemId)) {
                addResourceEvent ((int)flowType, currency, amount, itemType, itemId, fieldsAsString);
            }
#else
                addResourceEvent ((int)flowType, currency, amount, itemType, itemId, fieldsAsString);
#endif
        }

        public static void AddProgressionEvent (GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateProgressionEvent (progressionStatus, progression01, progression02, progression03)) {
                addProgressionEvent ((int)progressionStatus, progression01, progression02, progression03, fieldsAsString);
            }
#else
                addProgressionEvent ((int)progressionStatus, progression01, progression02, progression03, fieldsAsString);
#endif
        }

        public static void AddProgressionEventWithScore (GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateProgressionEvent (progressionStatus, progression01, progression02, progression03)) {
                addProgressionEventWithScore ((int)progressionStatus, progression01, progression02, progression03, score, fieldsAsString);
            }
#else
                addProgressionEventWithScore ((int)progressionStatus, progression01, progression02, progression03, score, fieldsAsString);
#endif
        }

        public static void AddDesignEvent (string eventID, float eventValue, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateDesignEvent (eventID)) {
                addDesignEventWithValue (eventID, eventValue, fieldsAsString);
            }
#else
                addDesignEventWithValue (eventID, eventValue, fieldsAsString);
#endif
        }

        public static void AddDesignEvent (string eventID, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateDesignEvent (eventID)) {
                addDesignEvent (eventID, fieldsAsString);
            }
#else
                addDesignEvent (eventID, fieldsAsString);
#endif
        }

        public static void AddErrorEvent (GAErrorSeverity severity, string message, IDictionary<string, object> fields)
        {
            string fieldsAsString = DictionaryToJsonString(fields);
#if UNITY_EDITOR
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateErrorEvent(severity,message)) {
                addErrorEvent ((int)severity, message, fieldsAsString);
            }
#else
                addErrorEvent ((int)severity, message, fieldsAsString);
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

        // ----------------------- COMMAND CENTER ---------------------- //
        public static string GetCommandCenterValueAsString(string key, string defaultValue)
        {
            return getCommandCenterValueAsString(key, defaultValue);
        }

        public static bool IsCommandCenterReady()
        {
            return isCommandCenterReady();
        }

        public static string GetConfigurationsContentAsString()
        {
            return getConfigurationsContentAsString();
        }

        private static string DictionaryToJsonString(IDictionary<string, object> dict)
        {
            Hashtable table = new Hashtable();
            if (dict != null)
            {
                foreach (KeyValuePair<string, object> pair in dict)
                {
                    table.Add(pair.Key, pair.Value);
                }
            }
            return GA_MiniJSON.Serialize(table);
        }
    }
}
