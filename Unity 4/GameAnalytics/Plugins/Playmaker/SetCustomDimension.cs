#if false

using System;
using UnityEngine;
using GameAnalyticsSDK;

namespace HutongGames.PlayMaker.Actions
{
	public enum CustomDimensionNumber
	{
		CustomDimension01 = 1,
		CustomDimension02 = 2,
		CustomDimension03 = 3
	}

	[ActionCategory("GameAnalytics")]
	[Tooltip("Sets a Custom Dimension")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1166")]
	public class SetCustomDimension : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Custom Dimension to set (1/2/3).")]
		public CustomDimensionNumber CustomDimension;

		[RequiredField]
		[Tooltip("The Custom Dimension value.")]
		public FsmString CustomDimensionValue;

		public override void Reset()
		{
			CustomDimension = CustomDimensionNumber.CustomDimension01;
			CustomDimensionValue = new FsmString() { UseVariable = false };
		}
		
		public override void OnEnter()
		{
			switch (CustomDimension)
			{
			case CustomDimensionNumber.CustomDimension01:
				GA_Setup.SetCustomDimension01(CustomDimensionValue.Value);
				break;
			case CustomDimensionNumber.CustomDimension02:
				GA_Setup.SetCustomDimension02(CustomDimensionValue.Value);
				break;
			case CustomDimensionNumber.CustomDimension03:
				GA_Setup.SetCustomDimension03(CustomDimensionValue.Value);
				break;
			}
		
			Finish();
		}
	}
}

#endif