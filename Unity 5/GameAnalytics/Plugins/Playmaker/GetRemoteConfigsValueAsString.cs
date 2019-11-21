#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("GameAnalytics")]
    [Tooltip("Get remote config value as string")]
    public class GetRemoteConfigsValueAsString : FsmStateAction
    {
        [Tooltip("The remote config key")]
        [RequiredField]
        public FsmString Key;

        [Tooltip("The remote config default value if the key is present in remote configs")]
        public FsmString DefaultValue;

        [ActionSection("Store Result")]

        [UIHint(UIHint.Variable)]
		[RequiredField]
        [Tooltip("Store the result of the method call.")]
        public FsmString storeResult;

        public override void Reset()
        {
        }

        public override void OnEnter()
        {
			if(DefaultValue.IsNone)
			{
				storeResult.Value = GameAnalytics.GetRemoteConfigsValueAsString(Key.Value);
			}
			else
			{
				storeResult.Value = GameAnalytics.GetRemoteConfigsValueAsString(Key.Value, DefaultValue.Value);
			}

            Finish();
        }
    }
}

#endif
