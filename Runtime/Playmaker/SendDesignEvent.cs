#if false

using System;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a design event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1164")]
	public class SendDesignEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event ID")]
		public FsmString EventID;

		[Tooltip("The event value")]
		public FsmFloat EventValue;

		public override void Reset()
		{
			EventID = new FsmString() { UseVariable = false };
			EventValue = new FsmFloat() { UseVariable = true };
		}

		public override void OnEnter()
		{
			if (!EventValue.IsNone)
				GA_Design.NewEvent(EventID.Value, EventValue.Value, null);
			else
				GA_Design.NewEvent(EventID.Value, null);

			Finish();
		}
	}
}

#endif
