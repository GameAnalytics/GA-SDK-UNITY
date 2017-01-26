using UnityEngine;
using System.Collections;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper
	{
		#if (UNITY_STANDALONE || UNITY_WSA || UNITY_WP_8_1 || UNITY_SAMSUNGTV) && (!UNITY_EDITOR)

		private static void configureAvailableCustomDimensions01(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions01((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions02((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions03((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableResourceCurrencies((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableResourceItemTypes((string[])array.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureSdkGameEngineVersion(unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureGameEngineVersion(unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureBuild(build);
		}

		private static void configureUserId(string userId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureUserId(userId);
		}


		private static void initialize(string gamekey, string gamesecret)
		{
			GameAnalyticsSDK.Net.GameAnalytics.Initialize(gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension01(customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension02(customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension03(customDimension);
		}

        private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
        {
			GameAnalyticsSDK.Net.GameAnalytics.AddBusinessEvent(currency, amount, itemType, itemId, cartType);
        }

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddResourceEvent((GameAnalyticsSDK.Net.EGAResourceFlowType)flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddProgressionEvent((GameAnalyticsSDK.Net.EGAProgressionStatus)progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddProgressionEvent((GameAnalyticsSDK.Net.EGAProgressionStatus)progressionStatus, progression01, progression02, progression03, score);
		}

		private static void addDesignEvent(string eventId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddDesignEvent(eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddDesignEvent(eventId, value);
		}

		private static void addErrorEvent(int severity, string message)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddErrorEvent((GameAnalyticsSDK.Net.EGAErrorSeverity)severity, message);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledInfoLog(enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledVerboseLog(enabled);
		}

		private static void setManualSessionHandling(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledManualSessionHandling(enabled);
		}

		private static void gameAnalyticsStartSession()
		{
			GameAnalyticsSDK.Net.GameAnalytics.StartSession();
		}

		private static void gameAnalyticsEndSession()
		{
			GameAnalyticsSDK.Net.GameAnalytics.EndSession();
		}

		private static void setFacebookId(string facebookId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetFacebookId(facebookId);
		}

		private static void setGender(string gender)
		{
			switch(gender)
			{
				case "male":
					GameAnalyticsSDK.Net.GameAnalytics.SetGender(GameAnalyticsSDK.Net.EGAGender.Male);
					break;
				case "female":
					GameAnalyticsSDK.Net.GameAnalytics.SetGender(GameAnalyticsSDK.Net.EGAGender.Female);
					break;
			}
		}

		private static void setBirthYear(int birthYear)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetBirthYear(birthYear);
		}

#endif
    }
}
