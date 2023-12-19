using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Wrapper;
using GameAnalyticsSDK.Validators;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Setup
	{
		#region public methods

		public static void SetAvailableCustomDimensions01 (List<string> customDimensions)
		{
            if (GameAnalyticsSDK.Validators.GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
            {
                string json = GA_MiniJSON.Serialize(customDimensions);
                GA_Wrapper.SetAvailableCustomDimensions01(json);
            }
		}

		public static void SetAvailableCustomDimensions02 (List<string> customDimensions)
		{
			if (GameAnalyticsSDK.Validators.GAValidator.ValidateCustomDimensions (customDimensions.ToArray ())) {
				string json = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions02 (json);
			}
        }

		public static void SetAvailableCustomDimensions03 (List<string> customDimensions)
		{
			if (GameAnalyticsSDK.Validators.GAValidator.ValidateCustomDimensions (customDimensions.ToArray ())) {
				string json = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions03 (json);
			}
        }

		public static void SetAvailableResourceCurrencies (List<string> resourceCurrencies)
		{
			if (GameAnalyticsSDK.Validators.GAValidator.ValidateResourceCurrencies (resourceCurrencies.ToArray ())) {
				string json = GA_MiniJSON.Serialize(resourceCurrencies);
				GA_Wrapper.SetAvailableResourceCurrencies (json);
			}
		}

		public static void SetAvailableResourceItemTypes (List<string> resourceItemTypes)
		{
			if (GameAnalyticsSDK.Validators.GAValidator.ValidateResourceItemTypes (resourceItemTypes.ToArray ())) {
				string json = GA_MiniJSON.Serialize(resourceItemTypes);
				GA_Wrapper.SetAvailableResourceItemTypes (json);
			}
		}

		public static void SetInfoLog (bool enabled)
		{
			GA_Wrapper.SetInfoLog (enabled);
		}

		public static void SetVerboseLog (bool enabled)
		{
			GA_Wrapper.SetVerboseLog (enabled);
		}

		public static void SetCustomDimension01 (string customDimension)
		{
			GA_Wrapper.SetCustomDimension01 (customDimension);
        }

		public static void SetCustomDimension02 (string customDimension)
		{
			GA_Wrapper.SetCustomDimension02 (customDimension);
		}


		public static void SetCustomDimension03 (string customDimension)
		{
            GA_Wrapper.SetCustomDimension03(customDimension);
        }

		public static void SetGlobalCustomEventFields(IDictionary<string, object> customFields)
		{
			GA_Wrapper.SetGlobalCustomEventFields(customFields);
		}
        public static void EnableSDKInitEvent(bool flag)
        {
            GA_Wrapper.EnableSDKInitEvent(flag);
        }

        public static void EnableFpsHistogram(bool flag)
        {
            GA_Wrapper.EnableFpsHistogram(flag);
        }

        public static void EnableMemoryHistogram(bool flag)
        {
            GA_Wrapper.EnableMemoryHistogram(flag);
        }

        public static void EnableHealthHardwareInfo(bool flag)
        {
            GA_Wrapper.EnableHealthHardwareInfo(flag);
        }

		#endregion
	}
}
