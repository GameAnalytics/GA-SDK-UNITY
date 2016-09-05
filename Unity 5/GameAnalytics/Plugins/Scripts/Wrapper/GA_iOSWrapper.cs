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
		private static extern void initialize(string gamekey, string gamesecret);

		[DllImport ("__Internal")]
		private static extern void setCustomDimension01(string customDimension);

		[DllImport ("__Internal")]
		private static extern void setCustomDimension02(string customDimension);

		[DllImport ("__Internal")]
		private static extern void setCustomDimension03(string customDimension);

		[DllImport ("__Internal")]
		private static extern void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt);

		[DllImport ("__Internal")]
		private static extern void addBusinessEventAndAutoFetchReceipt(string currency, int amount, string itemType, string itemId, string cartType);

		[DllImport ("__Internal")]
		private static extern void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId);

		[DllImport ("__Internal")]
		private static extern void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03);

		[DllImport ("__Internal")]
		private static extern void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score);

		[DllImport ("__Internal")]
		private static extern void addDesignEvent(string eventId);

		[DllImport ("__Internal")]
		private static extern void addDesignEventWithValue(string eventId, float value);

		[DllImport ("__Internal")]
		private static extern void addErrorEvent(int severity, string message);
		
		[DllImport ("__Internal")]
		private static extern void setEnabledInfoLog(bool enabled);

		[DllImport ("__Internal")]
		private static extern void setEnabledVerboseLog(bool enabled);

		[DllImport ("__Internal")]
		private static extern void setManualSessionHandling(bool enabled);

		[DllImport ("__Internal")]
		private static extern void gameAnalyticsStartSession();

		[DllImport ("__Internal")]
		private static extern void gameAnalyticsEndSession();

		[DllImport ("__Internal")]
		private static extern void setFacebookId(string facebookId);

		[DllImport ("__Internal")]
		private static extern void setGender(string gender);

		[DllImport ("__Internal")]
		private static extern void setBirthYear(int birthYear);

		#endif
	}
}
