#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets the Facebook ID of the user")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1166")]
	public class SetFacebookID : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The player's Facebook ID.")]
		public FsmString FacebookID;

		public override void Reset()
		{
			FacebookID = new FsmString() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GA_Setup.SetFacebookId(FacebookID.Value);
		
			Finish();
		}
	}
}

#endif