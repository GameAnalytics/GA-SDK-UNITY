#if false

using System;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a progression event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1164")]
	public class SendProgressionEvent : FsmStateAction
	{
		[Tooltip("The progression status as string: 'start', 'complete', 'fail' case insensitive, any other values are invalid")]
		public FsmString ProgressionStatusAsString;

		[Tooltip("The progression status")]
		public GA_Progression.GAProgressionStatus ProgressionStatus;

		[RequiredField]
		[Tooltip("Progression layer 1")]
		public FsmString Progression01;

		[Tooltip("Progression layer 2")]
		public FsmString Progression02;

		[Tooltip("Progression layer 3")]
		public FsmString Progression03;

		[Tooltip("The player's score")]
		public FsmInt Score;

		public override void Reset()
		{
			ProgressionStatus = GA_Progression.GAProgressionStatus.GAProgressionStatusStart;
			Progression01 = new FsmString() { UseVariable = false };
			Progression02 = new FsmString() { UseVariable = false };
			Progression03 = new FsmString() { UseVariable = false };
			Score = new FsmInt() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			if (!Score.IsNone)
			{
				if (!Progression03.IsNone && !Progression02.IsNone)
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value, Progression02.Value, Progression03.Value, Score.Value);
				else if (!Progression02.IsNone)
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value, Progression02.Value, Score.Value);
				else
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value, Score.Value);
			}
			else
			{
				if (!Progression03.IsNone && !Progression02.IsNone)
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value, Progression02.Value, Progression03.Value);
				else if (!Progression02.IsNone)
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value, Progression02.Value);
				else
					GA_Progression.NewEvent(ProgressionStatus, Progression01.Value);
			}
			
			Finish();
		}
	}
}

#endif