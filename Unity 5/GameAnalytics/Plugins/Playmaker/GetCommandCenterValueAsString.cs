#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("GameAnalytics")]
    [Tooltip("Initialize GameAnalytics SDK")]
    public class GetCommandCenterValueAsString : FsmStateAction
    {
        [Tooltip("The command center key")]
        [RequiredField]
        public FsmString Key;

        [Tooltip("The command center default value if the key is present in command center")]
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
				storeResult.Value = GameAnalytics.GetCommandCenterValueAsString(Key.Value);
			}
			else
			{
				storeResult.Value = GameAnalytics.GetCommandCenterValueAsString(Key.Value, DefaultValue.Value);
			}

            Finish();
        }
    }
}

#endif
