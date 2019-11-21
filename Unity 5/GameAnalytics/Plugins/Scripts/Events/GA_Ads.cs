// This class handles game design events, such as kills, deaths, high scores, etc.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
    public static class GA_Ads
    {
        #region public methods

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="adAction">Action of ad (for example loaded, show).</param>
        /// <param name="adType">Type of ad (for video, interstitial).</param>
        /// <param name="adSdkName">Name of ad SDK.</param>
        /// <param name="adPlacement">Placement of ad or identifier of the ad in the app</param>
        /// <param name="duration">Duration of ad video</param>
        public static void NewEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, long duration)
        {
            GA_Wrapper.AddAdEventWithDuration(adAction, adType, adSdkName, adPlacement, duration);
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="adAction">Action of ad (for example loaded, show).</param>
        /// <param name="adType">Type of ad (for video, interstitial).</param>
        /// <param name="adSdkName">Name of ad SDK.</param>
        /// <param name="adPlacement">Placement of ad or identifier of the ad in the app</param>
        /// <param name="noAdReason">Error reason for no ad available</param>
        public static void NewEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, GAAdError noAdReason)
        {
            GA_Wrapper.AddAdEventWithReason(adAction, adType, adSdkName, adPlacement, noAdReason);
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="adAction">Action of ad (for example loaded, show).</param>
        /// <param name="adType">Type of ad (for video, interstitial).</param>
        /// <param name="adSdkName">Name of ad SDK.</param>
        /// <param name="adPlacement">Placement of ad or identifier of the ad in the app</param>
        public static void NewEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement)
        {
            GA_Wrapper.AddAdEvent(adAction, adType, adSdkName, adPlacement);
        }

        #endregion
    }
}
