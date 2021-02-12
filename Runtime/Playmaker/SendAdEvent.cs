#if false

using System;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends an ad event message to the GameAnalytics server")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1164")]
	public class SendAdEvent : FsmStateAction
	{
		[Tooltip("The ad action")]
		public GAAdAction AdAction;

		[Tooltip("The ad type")]
		public GAAdType AdType;

		[RequiredField]
		[Tooltip("Ad SDK name")]
		public FsmString AdSdkName;

		[RequiredField]
		[Tooltip("Ad placement")]
		public FsmString AdPlacement;

		[RequiredField]
		[Tooltip("Ad error reason")]
		public GAAdError AdErrorReason;

		[Tooltip("Ad video duration watched")]
		public FsmInt Duration;

		public override void Reset()
		{
			AdAction = GAAdAction.Show;
			AdType = GAAdType.Interstitial;
			AdSdkName = new FsmString() { UseVariable = false };
			AdPlacement = new FsmString() { UseVariable = false };
			AdErrorReason = GAAdError.Undefined;
			Duration = new FsmInt() { UseVariable = false };
		}

		public override void OnEnter()
		{
			if(!Duration.IsNone)
            {
				GA_Ads.NewEvent(AdAction, AdType, AdSdkName.Value, AdPlacement.Value, Duration.Value);
			}
			else if (AdErrorReason != GAAdError.Undefined)
			{
				GA_Ads.NewEvent(AdAction, AdType, AdSdkName.Value, AdPlacement.Value, AdErrorReason);
			}
			else
            {
				GA_Ads.NewEvent(AdAction, AdType, AdSdkName.Value, AdPlacement.Value);
			}

			Finish();
		}
	}
}

#endif
