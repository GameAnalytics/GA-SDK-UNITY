#if false

using System;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a error event message to the GameAnalytics server.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1171")]
	public class SendErrorEvent : FsmStateAction
	{

		[Tooltip("The severity of this event: critical, error, warning, info, debug")]
		public GAErrorSeverity severityType ;

		[Tooltip("The message")]
		[RequiredField]
		public FsmString Message;

		public override void Reset()
		{
			severityType = GAErrorSeverity.Error;
			Message = new FsmString() { UseVariable = false };
		}

		public override void OnEnter()
		{
			GA_Error.NewEvent(severityType, Message.Value, null, false);

			Finish();
		}
	}
}

#endif
