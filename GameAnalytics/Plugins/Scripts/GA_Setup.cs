using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameAnalyticsSDK
{
	public static class GA_Setup
	{
		public enum GAGender
		{
			Male = 1,
			Female = 2
		}

		#region public methods
		
		public static void SetAvailableCustomDimensions01 (List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_iOSWrapper.SetAvailableCustomDimensions01(json);
		}

		public static void SetAvailableCustomDimensions02 (List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_iOSWrapper.SetAvailableCustomDimensions02(json);
		}

		public static void SetAvailableCustomDimensions03 (List<string> customDimensions)
		{
			string json = GA_MiniJSON.JsonEncode(customDimensions.ToArray());

			GA_iOSWrapper.SetAvailableCustomDimensions03(json);
		}
		
		public static void SetAvailableResourceCurrencies (List<string> resourceCurrencies)
		{
			string json = GA_MiniJSON.JsonEncode(resourceCurrencies.ToArray());
			
			GA_iOSWrapper.SetAvailableResourceCurrencies(json);
		}

		public static void SetAvailableResourceItemTypes (List<string> resourceItemTypes)
		{
			string json = GA_MiniJSON.JsonEncode(resourceItemTypes.ToArray());

			GA_iOSWrapper.SetAvailableResourceItemTypes(json);
		}

		public static void SetInfoLog (bool enabled)
		{
			GA_iOSWrapper.SetInfoLog(enabled);
		}

		public static void SetVerboseLog (bool enabled)
		{
			GA_iOSWrapper.SetVerboseLog(enabled);
		}

		public static void SetFacebookId (string facebookId)
		{
			GA_iOSWrapper.SetFacebookId(facebookId);
		}

		public static void SetGender (GAGender gender)
		{
			switch (gender)
			{
			case GAGender.Male:
				GA_iOSWrapper.SetGender("male");
				break;
			case GAGender.Female:
				GA_iOSWrapper.SetGender("female");
				break;
			}
		}

		public static void SetBirthYear (int birthYear)
		{
			GA_iOSWrapper.SetBirthYear(birthYear);
		}

		public static void SetCustomDimension01 (string customDimension)
		{
			GA_iOSWrapper.SetCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02 (string customDimension)
		{
			GA_iOSWrapper.SetCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03 (string customDimension)
		{
			GA_iOSWrapper.SetCustomDimension03(customDimension);
		}

		#endregion
	}
}
