using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace GameAnalyticsSDK
{
	public partial class GA_Wrapper 
	{
		#if (UNITY_ANDROID) && (!UNITY_EDITOR)

		[DllImport ("GameAnalytics")]
		private static extern void configureAvailableCustomDimensions01(string list);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureAvailableCustomDimensions02(string list);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureAvailableCustomDimensions03(string list);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureAvailableResourceCurrencies(string list);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureAvailableResourceItemTypes(string list);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureSdkGameEngineVersion(string unitySdkVersion);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureGameEngineVersion(string unityEngineVersion);
		
		[DllImport ("GameAnalytics")]
		private static extern void configureBuild(string build);
		
		[DllImport ("GameAnalytics")]
		private static extern void initialize(string gamekey, string gamesecret);
		
		[DllImport ("GameAnalytics")]
		private static extern void setCustomDimension01(string customDimension);
		
		[DllImport ("GameAnalytics")]
		private static extern void setCustomDimension02(string customDimension);
		
		[DllImport ("GameAnalytics")]
		private static extern void setCustomDimension03(string customDimension);
		
		[DllImport ("GameAnalytics")]
		private static extern void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType);
		
		[DllImport ("GameAnalytics")]
		private static extern void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature);
		
		[DllImport ("GameAnalytics")]
		private static extern void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId);
		
		[DllImport ("GameAnalytics")]
		private static extern void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03);
		
		[DllImport ("GameAnalytics")]
		private static extern void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score);
		
		[DllImport ("GameAnalytics")]
		private static extern void addDesignEvent(string eventId);
		
		[DllImport ("GameAnalytics")]
		private static extern void addDesignEventWithValue(string eventId, float value);
		
		[DllImport ("GameAnalytics")]
		private static extern void addErrorEvent(int severity, string message);
		
		[DllImport ("GameAnalytics")]
		private static extern void setEnabledInfoLog(bool enabled);
		
		[DllImport ("GameAnalytics")]
		private static extern void setEnabledVerboseLog(bool enabled);
		
		[DllImport ("GameAnalytics")]
		private static extern void setFacebookId(string facebookId);
		
		[DllImport ("GameAnalytics")]
		private static extern void setGender(string gender);
		
		[DllImport ("GameAnalytics")]
		private static extern void setBirthYear(int birthYear);
		#endif
	}
}
