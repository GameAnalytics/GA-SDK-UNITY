#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;

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
		public GA_Setup.GAGender Gender;

		public override void Reset()
		{
			Gender = GA_Setup.GAGender.Male;
		}
		
		public override void OnEnter()
		{
			if (GenderAsString.Value.Equals("male", StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GA_Setup.GAGender.Male;
			}
			else if (GenderAsString.Value.Equals("female", StringComparison.InvariantCultureIgnoreCase) )
			{
				Gender = GA_Setup.GAGender.Female;
			}

			GA_Setup.SetGender(Gender);
		
			Finish();
		}
	}
}

#endif