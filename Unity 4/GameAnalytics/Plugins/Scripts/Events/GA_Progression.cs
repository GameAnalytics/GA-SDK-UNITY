using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Progression
	{
		#region public methods

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01)
		{
			CreateEvent(progressionStatus, progression01, null, null, null);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02)
		{
			CreateEvent(progressionStatus, progression01, progression02, null, null);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			CreateEvent(progressionStatus, progression01, progression02, progression03, null);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, int score)
		{
			CreateEvent(progressionStatus, progression01, null, null, score);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
		{
			CreateEvent(progressionStatus, progression01, progression02, null, score);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			CreateEvent(progressionStatus, progression01, progression02, progression03, score);
		}

		#endregion

		#region private methods

		private static void CreateEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int? score)
		{
			if(score.HasValue)
			{
				GA_Wrapper.AddProgressionEventWithScore(progressionStatus, progression01, progression02, progression03, score.Value);
			}
			else
			{
				GA_Wrapper.AddProgressionEvent(progressionStatus, progression01, progression02, progression03);
			}
		}

		#endregion
	}
}