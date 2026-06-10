using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
#if UNITY_STANDALONE && !(UNITY_EDITOR) && !(GA_USE_MONO_WRAPPER)
    public partial class GA_Wrapper
    {
        private static void configureCustomLogHandler(GANativeLogCallback callback)
        {
            gameAnalytics_configureCustomLogHandler(callback);
        }

        private static string[] MakeList(string list)
        {
            IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
            
            string[] array = new string[iList.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = iList[i].ToString();
            }
            
            return array;
        }

        private static void configureAvailableCustomDimensions01(string list)
        {
            string[] dim = MakeList(list);
            gameAnalytics_configureAvailableCustomDimensions01(dim, dim.Length);
        }
        
        private static void configureAvailableCustomDimensions02(string list)
        {
            string[] dim = MakeList(list);
            gameAnalytics_configureAvailableCustomDimensions02(dim, dim.Length);

        }
        private static void configureAvailableCustomDimensions03(string list)
        {
            string[] dim = MakeList(list);
            gameAnalytics_configureAvailableCustomDimensions03(dim, dim.Length);
        }

        private static void configureAvailableResourceCurrencies(string list)
        {
            string[] res = MakeList(list);
            gameAnalytics_configureAvailableResourceCurrencies(res, res.Length);
        }

        private static void configureAvailableResourceItemTypes(string list)
        {
            string[] res = MakeList(list);
            gameAnalytics_configureAvailableResourceItemTypes(res, res.Length);
        }

        private static void configureSdkGameEngineVersion(string unitySdkVersion)
        {
            gameAnalytics_configureSdkGameEngineVersion(unitySdkVersion);
        }

        private static void configureGameEngineVersion(string unityEngineVersion)
        {
            gameAnalytics_configureGameEngineVersion(unityEngineVersion);
        }

        private static void configureBuild(string build)
        {
            gameAnalytics_configureBuild(build);
        }

        private static void configureUserId(string userId)
        {
            gameAnalytics_configureUserId(userId);
        }

        private static void configureAutoDetectAppVersion(bool flag)
        {
            gameAnalytics_configureAutoDetectAppVersion(flag);
        }

        // Matches the legacy C# (Mono) SDK path so the C++ SDK reuses the existing
        // ga.sqlite3 (it appends "/<gameKey>/ga.sqlite3" itself).
        private static void configureLegacyWritablePath()
        {
            string writablePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + Path.DirectorySeparatorChar + "GameAnalytics"
                + Path.DirectorySeparatorChar + AppDomain.CurrentDomain.FriendlyName;

            // C++ SDK only creates the final "<gameKey>" dir, so the parent must exist.
            if (!Directory.Exists(writablePath))
            {
                Directory.CreateDirectory(writablePath);
            }

            gameAnalytics_configureWritablePath(writablePath);
        }

        private static void initialize(string gamekey, string gamesecret)
        {
            configureLegacyWritablePath(); // must run before initialize
            gameAnalytics_initialize(gamekey, gamesecret);
        }

        private static void setCustomDimension01(string customDimension)
        {
            gameAnalytics_setCustomDimension01(customDimension);
        }

        private static void setCustomDimension02(string customDimension)
        {
            gameAnalytics_setCustomDimension02(customDimension);
        }

        private static void setCustomDimension03(string customDimension)
        {
            gameAnalytics_setCustomDimension03(customDimension);
        }

        private static void setGlobalCustomEventFields(string customFields)
        {
            gameAnalytics_setGlobalCustomEventFields(customFields);
        }

        private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string fields, bool mergeFields)
        {
#if DEBUG || DEVELOPMENT_BUILD
            Debug.Log($"addBusinessEvent - currency: {currency}, amount: {amount}, itemType: {itemType}, itemId: {itemId}, cartType: {cartType}, fields: {fields}, mergeFields: {mergeFields}");
#endif
            gameAnalytics_addBusinessEvent(currency, (double)amount, itemType, itemId, cartType, fields, mergeFields);
        }

        private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields, bool mergeFields)
        {
#if DEBUG || DEVELOPMENT_BUILD
            Debug.Log($"addResourceEvent - flowType: {flowType}, currency: {currency}, amount: {amount}, itemType: {itemType}, itemId: {itemId}, fields: {fields}, mergeFields: {mergeFields}");
#endif
            gameAnalytics_addResourceEvent(flowType, currency, (double)amount, itemType, itemId, fields, mergeFields);
        }
        
        private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields, bool mergeFields)
        {
            gameAnalytics_addProgressionEvent(progressionStatus, progression01, progression02, progression03, fields, mergeFields);
        }

        private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields, bool mergeFields)
        {
            gameAnalytics_addProgressionEventWithScore(progressionStatus, progression01, progression02, progression03, score, fields, mergeFields);
        }

        private static void addDesignEvent(string eventId, string fields, bool mergeFields)
        {
#if DEBUG || DEVELOPMENT_BUILD
            Debug.Log($"addDesignEvent - eventId: {eventId}, fields: {fields}, mergeFields: {mergeFields}");
#endif
            gameAnalytics_addDesignEvent(eventId, fields, mergeFields);
        }

        private static void addDesignEventWithValue(string eventId, float value, string fields, bool mergeFields)
        {
#if DEBUG || DEVELOPMENT_BUILD
            Debug.Log($"addDesignEventWithValue - eventId: {eventId}, value: {value}, fields: {fields}, mergeFields: {mergeFields}");
#endif
            gameAnalytics_addDesignEventWithValue(eventId, (double)value, fields, mergeFields);
        }

        private static void addErrorEvent(int severity, string message, string fields, bool mergeFields)
        {
            gameAnalytics_addErrorEvent(severity, message, fields, mergeFields);
        }

        private static void setEnabledInfoLog(bool enabled)
        {
            gameAnalytics_setEnabledInfoLog(enabled);
        }

        private static void setEnabledVerboseLog(bool enabled)
        {
            gameAnalytics_setEnabledVerboseLog(enabled);
        }

        private static void setManualSessionHandling(bool enabled)
        {
            gameAnalytics_setEnabledManualSessionHandling(enabled);
        }

        private static void setEventSubmission(bool enabled)
        {
            gameAnalytics_setEnabledEventSubmission(enabled);
        }
        private static void gameAnalyticsStartSession()
        {
            gameAnalytics_startSession();
        }

        private static void gameAnalyticsEndSession()
        {
            gameAnalytics_endSession();
        }

        private static void gameAnalyticsOnQuit()
        {
            gameAnalytics_onQuit();
        }

        private static string getRemoteConfigsValueAsString(string key, string defaultValue)
        {
            IntPtr ptr = gameAnalytics_getRemoteConfigsValueAsStringWithDefaultValue(key, defaultValue);
            return GetNativeString(ptr);
        }

        private static bool isRemoteConfigsReady()
        {
            return gameAnalytics_isRemoteConfigsReady();
        }

        private static string getRemoteConfigsContentAsString()
        {
            IntPtr ptr = gameAnalytics_getRemoteConfigsContentAsString();
            return GetNativeString(ptr);
        }

        private static string getABTestingId()
        {
            IntPtr ptr = gameAnalytics_getABTestingId();
            return GetNativeString(ptr);
        }

        private static string getABTestingVariantId()
        {
            IntPtr ptr = gameAnalytics_getABTestingVariantId();
            return GetNativeString(ptr);
        }

        public static string getUserId()
        {
            IntPtr ptr = gameAnalytics_getUserId();
            return GetNativeString(ptr);
        }
        
        static private string GetNativeString(IntPtr ptr)
        {
            string s = Marshal.PtrToStringAnsi(ptr);
            gameAnalytics_freeString(ptr);
            return s;
        }

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_freeString(IntPtr ptr);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAvailableCustomDimensions01(string[] list, int size);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAvailableCustomDimensions02(string[] list, int size);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAvailableCustomDimensions03(string[] list, int size);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAvailableResourceCurrencies(string[] list, int size);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAvailableResourceItemTypes(string[] list, int size);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureSdkGameEngineVersion(string unitySdkVersion);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureGameEngineVersion(string unityEngineVersion);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureBuild(string build);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureUserId(string userId);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureExternalUserId(string userId);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureAutoDetectAppVersion(bool flag);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureWritablePath(string writablePath);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_initialize(string gamekey, string gamesecret);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setCustomDimension01(string customDimension);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setCustomDimension02(string customDimension);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setCustomDimension03(string customDimension);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setGlobalCustomEventFields(string customFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addBusinessEvent(string currency, double amount, string itemType, string itemId, string cartType, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addResourceEvent(int flowType, string currency, double amount, string itemType, string itemId, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addDesignEvent(string eventId, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addDesignEventWithValue(string eventId, double value, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addErrorEvent(int severity, string message, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addAdEventWithDuration(int adAction, int adType, string adSdkName, string adPlacement, long duration, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addAdEventWithReason(int adAction, int adType, string adSdkName, string adPlacement, int noAdReason, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addAdEvent(int adAction, int adType, string adSdkName, string adPlacement, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_addImpressionEvent(string adNetworkName, string adNetworkVersion, string impressionData, string fields, bool mergeFields);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setEnabledInfoLog(bool enabled);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setEnabledVerboseLog(bool enabled);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setEnabledManualSessionHandling(bool enabled);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_setEnabledEventSubmission(bool enabled);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_startSession();

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_endSession();

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_onQuit();

        [DllImport ("gameanalytics")]
        private static extern bool gameAnalytics_isRemoteConfigsReady();

        [DllImport ("gameanalytics")]
        private static extern IntPtr gameAnalytics_getRemoteConfigsContentAsString();

        [DllImport ("gameanalytics")]
        private static extern IntPtr gameAnalytics_getRemoteConfigsValueAsStringWithDefaultValue(string key, string defaultValue);

        [DllImport ("gameanalytics")]
        private static extern IntPtr gameAnalytics_getRemoteConfigsContentAsJSON();

        [DllImport ("gameanalytics")]
        private static extern IntPtr gameAnalytics_getABTestingId();

        [DllImport ("gameanalytics")]
        private static extern IntPtr gameAnalytics_getABTestingVariantId();

        [DllImport ("gameanalytics")]
        public static extern IntPtr gameAnalytics_getUserId();

        [DllImport ("gameanalytics")]
        public static extern IntPtr gameAnalytics_getExternalUserId();

        [DllImport ("gameanalytics")]
        public static extern void gameAnalytics_enableSDKInitEvent(bool flag);

        [DllImport ("gameanalytics")]
        public static extern void gameAnalytics_enableFpsHistogram(bool flag);

        [DllImport ("gameanalytics")]
        public static extern void gameAnalytics_enableMemoryHistogram(bool flag);

        [DllImport ("gameanalytics")]
        public static extern void gameAnalytics_enableHealthHardwareInfo(bool flag);

        [DllImport ("gameanalytics")]
        private static extern void gameAnalytics_configureCustomLogHandler(GANativeLogCallback handler);
    }

    #endif
}
