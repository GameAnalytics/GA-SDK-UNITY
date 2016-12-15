#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets the gender of the user")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1166")]
	public class SetGender : FsmStateAction
	{
		[Tooltip("The player's gender as string: 'male', 'female' case insensitive, any other values are invalid")]
		public FsmString GenderAsString;

		[Tooltip("The player's gender")]
		public GAGender Gender;

		public override void Reset()
		{
			Gender = GAGender.male;
		}
		
		public override void OnEnter()
		{
			if (GenderAsString.Value.Equals("male", StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GAGender.male;
			}
			else if (GenderAsString.Value.Equals("female", StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GAGender.female;
			}

			GA_Setup.SetGender(Gender);
		
			Finish();
		}
	}
}

#endif