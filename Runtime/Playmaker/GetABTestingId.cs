#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("GameAnalytics")]
    [Tooltip("Get AB testing id")]
    public class GetABTestingId : FsmStateAction
    {
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
            storeResult.Value = GameAnalytics.GetABTestingId();

            Finish();
        }
    }
}

#endif
