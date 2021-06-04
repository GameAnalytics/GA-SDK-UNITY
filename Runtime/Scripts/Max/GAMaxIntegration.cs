using System;
using UnityEngine;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;

public class GAMaxIntegration
{
#if gameanalytics_max_enabled && !(UNITY_EDITOR)
    private static bool _subscribed = false;

    private static void runCallback(string format, MaxSdkBase.AdInfo adInfo, Action<string> callback)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("country", MaxSdk.GetSdkConfiguration().CountryCode);
        dict.Add("network_name", adInfo.NetworkName);
        dict.Add("adunit_id", adInfo.AdUnitIdentifier);
        dict.Add("adunit_format", format);
        dict.Add("placement", adInfo.Placement);
        dict.Add("creative_id", adInfo.CreativeIdentifier);
        dict.Add("revenue", adInfo.Revenue);
        string json = GA_MiniJSON.Serialize(dict);
        callback(json);
    }
#endif

    public static void ListenForImpressions(Action<string> callback)
    {
#if gameanalytics_max_enabled && !(UNITY_EDITOR)
        if (_subscribed)
        {
            Debug.Log("Ignoring duplicate gameanalytics subscription");
            return;
        }
        
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("INTER", adInfo, callback);
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("BANNER", adInfo, callback);
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("REWARDED", adInfo, callback);
        MaxSdkCallbacks.CrossPromo.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("XPROMO", adInfo, callback);
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("MREC", adInfo, callback);
        MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("REWARDED_INTER", adInfo, callback);
        _subscribed = true;
#endif
    }
}
