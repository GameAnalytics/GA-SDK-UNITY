using UnityEngine;
using System.Collections;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper 
	{
		#if (UNITY_STANDALONE) && (!UNITY_EDITOR)

		private static void configureAvailableCustomDimensions01(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureAvailableCustomDimensions01((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureAvailableCustomDimensions02((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureAvailableCustomDimensions03((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureAvailableResourceCurrencies((string[])array.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			ArrayList array = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureAvailableResourceItemTypes((string[])array.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureSdkGameEngineVersion(unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureGameEngineVersion(unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureBuild(build);
		}

		private static void configureUserId(string userId)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.ConfigureUserId(userId);
		}


		private static void initialize(string gamekey, string gamesecret)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.Initialize(gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetCustomDimension01(customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetCustomDimension02(customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetCustomDimension03(customDimension);
		}
			
        private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
        {
			GameAnalyticsSDK.Mono.GameAnalytics.AddBusinessEvent(currency, amount, itemType, itemId, cartType);
        }

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddResourceEvent((GameAnalyticsSDK.Mono.EGAResourceFlowType)flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddProgressionEvent((GameAnalyticsSDK.Mono.EGAProgressionStatus)progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddProgressionEvent((GameAnalyticsSDK.Mono.EGAProgressionStatus)progressionStatus, progression01, progression02, progression03, score);
		}

		private static void addDesignEvent(string eventId)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddDesignEvent(eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddDesignEvent(eventId, value);
		}

		private static void addErrorEvent(int severity, string message)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.AddErrorEvent((GameAnalyticsSDK.Mono.EGAErrorSeverity)severity, message);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetEnabledInfoLog(enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetEnabledVerboseLog(enabled);
		}

		private static void setFacebookId(string facebookId)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetFacebookId(facebookId);
		}

		private static void setGender(string gender)
		{
			switch(gender)
			{
				case "male":
					GameAnalyticsSDK.Mono.GameAnalytics.SetGender(GameAnalyticsSDK.Mono.EGAGender.Male);
					break;
				case "female":
					GameAnalyticsSDK.Mono.GameAnalytics.SetGender(GameAnalyticsSDK.Mono.EGAGender.Female);
					break;
			}
		}

		private static void setBirthYear(int birthYear)
		{
			GameAnalyticsSDK.Mono.GameAnalytics.SetBirthYear(birthYear);
		}
			
		#endif
	}
}