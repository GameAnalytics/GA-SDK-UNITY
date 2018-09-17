#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Initialize GameAnalytics SDK")]
	public class IsCommandCenterReady : FsmStateAction
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
			storeResult.Value = GameAnalytics.IsCommandCenterReady();

			Finish();
		}
	}
}

#endif
