#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Is remote configs ready")]
	public class IsRemoteConfigsReady : FsmStateAction
	{
		[ActionSection("Store Result")]

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the result of the method call.")]
		public FsmBool storeResult;

		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			storeResult.Value = GameAnalytics.IsRemoteConfigsReady();

			Finish();
		}
	}
}

#endif
