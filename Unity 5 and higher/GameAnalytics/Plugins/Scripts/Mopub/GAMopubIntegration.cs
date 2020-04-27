using System;
using UnityEngine;

public class GAMopubIntegration
{
#if gameanalytics_mopub_enabled
    private static bool _subscribed = false;
#endif

    public static void ListenForImpressions(Action<string> callback)
    {
#if gameanalytics_mopub_enabled
        if (_subscribed)
        {
            Debug.Log("Ignoring duplicate gameanalytics subscription");
            return;
        }

        MoPubManager.OnImpressionTrackedEvent += (arg1, arg2) => callback(arg2.JsonRepresentation);
        _subscribed = true;
#endif
    }
}
