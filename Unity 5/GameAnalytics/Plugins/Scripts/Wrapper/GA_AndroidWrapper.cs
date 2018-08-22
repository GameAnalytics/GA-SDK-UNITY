using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper
	{
#if (UNITY_ANDROID) && !(UNITY_EDITOR)

		private static readonly AndroidJavaClass GA = new AndroidJavaClass("com.gameanalytics.sdk.GameAnalytics");
		private static readonly AndroidJavaClass UNITY_GA = new AndroidJavaClass("com.gameanalytics.sdk.unity.UnityGameAnalytics");

		private static void configureAvailableCustomDimensions01(string list)
		{
			IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList array = new ArrayList();
			foreach(object entry in iList)
			{
				array.Add(entry);
			}

            GA.CallStatic("configureAvailableCustomDimensions01", array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList array = new ArrayList();
			foreach(object entry in iList)
			{
				array.Add(entry);
			}
			GA.CallStatic("configureAvailableCustomDimensions02", array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList array = new ArrayList();
			foreach(object entry in iList)
			{
				array.Add(entry);
			}
            GA.CallStatic("configureAvailableCustomDimensions03", array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList array = new ArrayList();
			foreach(object entry in iList)
			{
				array.Add(entry);
			}
            GA.CallStatic("configureAvailableResourceCurrencies", array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			IList<object> iList = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList array = new ArrayList();
			foreach(object entry in iList)
			{
				array.Add(entry);
			}
            GA.CallStatic("configureAvailableResourceItemTypes", array.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GA.CallStatic("configureSdkGameEngineVersion", unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GA.CallStatic("configureGameEngineVersion", unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GA.CallStatic("configureBuild", build);
		}

		private static void configureUserId(string userId)
		{
			GA.CallStatic("configureUserId", userId);
		}

		private static void initialize(string gamekey, string gamesecret)
		{
			UNITY_GA.CallStatic("initialize");

			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaClass ga = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			ga.CallStatic("initializeWithActivity", activity);
			GA.CallStatic("initializeWithGameKey", gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GA.CallStatic("setCustomDimension01", customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GA.CallStatic("setCustomDimension02", customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GA.CallStatic("setCustomDimension03", customDimension);
		}

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string fields)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType/*, fields*/);
		}

		private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature, string fields)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType, receipt, store, signature/*, fields*/);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields)
		{
			GA.CallStatic("addResourceEventWithFlowType", flowType, currency, amount, itemType, itemId/*, fields*/);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03/*, fields*/);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03, (double)score/*, fields*/);
		}

		private static void addDesignEvent(string eventId, string fields)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId/*, fields*/);
		}

		private static void addDesignEventWithValue(string eventId, float value, string fields)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId, (double)value/*, fields*/);
		}

		private static void addErrorEvent(int severity, string message, string fields)
		{
			GA.CallStatic("addErrorEventWithSeverity", severity, message/*, fields*/);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GA.CallStatic("setEnabledInfoLog", enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GA.CallStatic("setEnabledVerboseLog", enabled);
		}

		private static void setFacebookId(string facebookId)
		{
			GA.CallStatic("setFacebookId", facebookId);
		}

		private static void setGender(string gender)
		{
			switch(gender)
			{
				case "male":
					GA.CallStatic("setGender", 1);
					break;
				case "female":
					GA.CallStatic("setGender", 2);
					break;
			}

		}

		private static void setBirthYear(int birthYear)
		{
			GA.CallStatic("setBirthYear", birthYear);
		}

		private static void setManualSessionHandling(bool enabled)
		{
			GA.CallStatic("setEnabledManualSessionHandling", enabled);
		}

		private static void gameAnalyticsStartSession()
		{
			GA.CallStatic("startSession");
		}

		private static void gameAnalyticsEndSession()
		{
			GA.CallStatic("endSession");
		}

		private static string getCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA.CallStatic<string>("getCommandCenterValueAsString", key, defaultValue);
		}

		private static bool isCommandCenterReady ()
		{
			return GA.CallStatic<bool>("isCommandCenterReady");
		}

		private static string getConfigurationsContentAsString()
		{
			return GA.CallStatic<string>("getConfigurationsContentAsString");
		}
#endif
    }
}
