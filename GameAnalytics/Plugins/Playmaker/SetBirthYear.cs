#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets the birth year of the user")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1166")]
	public class SetBirthYear : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The player's birth year as an int.")]
		public FsmInt BirthYear;

		public override void Reset()
		{
			BirthYear = new FsmInt() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			GA_Setup.SetBirthYear(BirthYear.Value);
		
			Finish();
		}
	}
}

#endif