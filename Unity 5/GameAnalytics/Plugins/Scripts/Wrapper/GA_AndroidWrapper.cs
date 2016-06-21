using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper 
	{
		#if (UNITY_ANDROID) && !(UNITY_EDITOR)

		private static readonly AndroidJavaClass GA = new AndroidJavaClass("com.gameanalytics.sdk.GameAnalytics");

		private static void configureAvailableCustomDimensions01(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions01", array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions02", array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions03", array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableResourceCurrencies", array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
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

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType);
		}

		private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType, receipt, store, signature);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId)
		{
			GA.CallStatic("addResourceEventWithFlowType", flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03, (double)score);
		}

		private static void addDesignEvent(string eventId)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId, (double)value);
		}

		private static void addErrorEvent(int severity, string message)
		{
			GA.CallStatic("addErrorEventWithSeverity", severity, message);
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
		#endif
	}
}
