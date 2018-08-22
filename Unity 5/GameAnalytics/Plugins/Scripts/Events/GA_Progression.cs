using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Progression
	{
		#region public methods

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, null, null, null, fields);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, progression02, null, null, fields);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, progression02, progression03, null, fields);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, int score, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, null, null, score, fields);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, progression02, null, score, fields);
		}

		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
		{
			CreateEvent(progressionStatus, progression01, progression02, progression03, score, fields);
		}

		#endregion

		#region private methods

		private static void CreateEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int? score, IDictionary<string, object> fields)
		{
			if(score.HasValue)
			{
				GA_Wrapper.AddProgressionEventWithScore(progressionStatus, progression01, progression02, progression03, score.Value, fields);
			}
			else
			{
				GA_Wrapper.AddProgressionEvent(progressionStatus, progression01, progression02, progression03, fields);
			}
		}

		#endregion
	}
}