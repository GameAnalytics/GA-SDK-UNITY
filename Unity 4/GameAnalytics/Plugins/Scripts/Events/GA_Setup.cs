using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Setup
	{
		#region public methods

		public static void SetAvailableCustomDimensions01(List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_Wrapper.SetAvailableCustomDimensions01(json);
		}

		public static void SetAvailableCustomDimensions02(List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_Wrapper.SetAvailableCustomDimensions02(json);
		}

		public static void SetAvailableCustomDimensions03(List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_Wrapper.SetAvailableCustomDimensions03(json);
		}

		public static void SetAvailableResourceCurrencies(List<string> resourceCurrencies)
		{
			string json = GA_MiniJSON.JsonEncode(resourceCurrencies.ToArray());

			GA_Wrapper.SetAvailableResourceCurrencies(json);
		}

		public static void SetAvailableResourceItemTypes(List<string> resourceItemTypes)
		{
			string json = GA_MiniJSON.JsonEncode(resourceItemTypes.ToArray());

			GA_Wrapper.SetAvailableResourceItemTypes(json);
		}

		public static void SetInfoLog(bool enabled)
		{
			GA_Wrapper.SetInfoLog(enabled);
		}

		public static void SetVerboseLog(bool enabled)
		{
			GA_Wrapper.SetVerboseLog(enabled);
		}

		public static void SetFacebookId(string facebookId)
		{
			GA_Wrapper.SetFacebookId(facebookId);
		}

		public static void SetGender(GAGender gender)
		{
			switch(gender)
			{
			case GAGender.Male:
				GA_Wrapper.SetGender("male");
				break;
			case GAGender.Female:
				GA_Wrapper.SetGender("female");
				break;
			}
		}

		public static void SetBirthYear(int birthYear)
		{
			GA_Wrapper.SetBirthYear(birthYear);
		}

		public static void SetCustomDimension01(string customDimension)
		{
			GA_Wrapper.SetCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			GA_Wrapper.SetCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			GA_Wrapper.SetCustomDimension03(customDimension);
		}

		#endregion
	}
}
