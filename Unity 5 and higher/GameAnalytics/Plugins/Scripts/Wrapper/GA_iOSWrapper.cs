using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace GameAnalyticsSDK.Wrapper
{
    public partial class GA_Wrapper
    {
#if (UNITY_IOS) && (!UNITY_EDITOR)

        [DllImport ("__Internal")]
        private static extern void configureAvailableCustomDimensions01(string list);

        [DllImport ("__Internal")]
        private static extern void configureAvailableCustomDimensions02(string list);

        [DllImport ("__Internal")]
        private static extern void configureAvailableCustomDimensions03(string list);

        [DllImport ("__Internal")]
        private static extern void configureAvailableResourceCurrencies(string list);

        [DllImport ("__Internal")]
        private static extern void configureAvailableResourceItemTypes(string list);

        [DllImport ("__Internal")]
        private static extern void configureSdkGameEngineVersion(string unitySdkVersion);

        [DllImport ("__Internal")]
        private static extern void configureGameEngineVersion(string unityEngineVersion);

        [DllImport ("__Internal")]
        private static extern void configureBuild(string build);

        [DllImport ("__Internal")]
        private static extern void configureUserId(string userId);

        [DllImport ("__Internal")]
        private static extern void configureAutoDetectAppVersion(bool flag);

        [DllImport ("__Internal")]
        private static extern void initialize(string gamekey, string gamesecret);

        [DllImport ("__Internal")]
        private static extern void setCustomDimension01(string customDimension);

        [DllImport ("__Internal")]
        private static extern void setCustomDimension02(string customDimension);

        [DllImport ("__Internal")]
        private static extern void setCustomDimension03(string customDimension);

        [DllImport ("__Internal")]
        private static extern void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string fields);

        [DllImport ("__Internal")]
        private static extern void addBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType, string fields);

        [DllImport ("__Internal")]
        private static extern void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields);

        [DllImport ("__Internal")]
        private static extern void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields);

        [DllImport ("__Internal")]
        private static extern void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields);

        [DllImport ("__Internal")]
        private static extern void addDesignEvent(string eventId, string fields);

        [DllImport ("__Internal")]
        private static extern void addDesignEventWithValue(string eventId, float value, string fields);

        [DllImport ("__Internal")]
        private static extern void addErrorEvent(int severity, string message, string fields);

        [DllImport ("__Internal")]
        private static extern void addAdEventWithDuration(int adAction, int adType, string adSdkName, string adPlacement, long duration);

        [DllImport ("__Internal")]
        private static extern void addAdEventWithReason(int adAction, int adType, string adSdkName, string adPlacement, int noAdReason);

        [DllImport ("__Internal")]
        private static extern void addAdEvent(int adAction, int adType, string adSdkName, string adPlacement);

        [DllImport ("__Internal")]
        private static extern void addImpressionEvent(string adNetworkName, string impressionData);

        [DllImport ("__Internal")]
        private static extern void setEnabledInfoLog(bool enabled);

        [DllImport ("__Internal")]
        private static extern void setEnabledVerboseLog(bool enabled);

        [DllImport ("__Internal")]
        private static extern void setManualSessionHandling(bool enabled);

        [DllImport ("__Internal")]
        private static extern void setEventSubmission(bool enabled);

        [DllImport ("__Internal")]
        private static extern void gameAnalyticsStartSession();

        [DllImport ("__Internal")]
        private static extern void gameAnalyticsEndSession();

        [DllImport ("__Internal")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string getRemoteConfigsValueAsString(string key, string defaultValue);

        [DllImport ("__Internal")]
        private static extern bool isRemoteConfigsReady();

        [DllImport ("__Internal")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string getRemoteConfigsContentAsString();

        [DllImport ("__Internal")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string getABTestingId();

        [DllImport ("__Internal")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string getABTestingVariantId();

        [DllImport ("__Internal")]
        private static extern void startTimer(string key);

        [DllImport ("__Internal")]
        private static extern void pauseTimer(string key);

        [DllImport ("__Internal")]
        private static extern void resumeTimer(string key);

        [DllImport ("__Internal")]
        private static extern long stopTimer(string key);

        private static void subscribeMoPubImpressions()
        {
            GAMopubIntegration.ListenForImpressions(ImpressionHandler);
        }

        private static void ImpressionHandler(string json)
        {
            if(!string.IsNullOrEmpty(json))
            {
                addImpressionEvent("mopub", json);
            }
        }
#endif
    }
}
