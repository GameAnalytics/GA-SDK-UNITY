#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Initialize GameAnalytics SDK")]
	public class GAInitialize : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			GameAnalytics.Initialize();

			Finish();
		}
	}
}

#endif
