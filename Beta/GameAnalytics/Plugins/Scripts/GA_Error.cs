/// <summary>
/// This class handles quality (QA) events, such as crashes, fps, etc.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameAnalyticsSDK
{
	public static class GA_Error
	{
		public enum GAErrorSeverity
		{
			GAErrorSeverityDebug = 1,
			GAErrorSeverityInfo = 2,
			GAErrorSeverityWarning = 3,
			GAErrorSeverityError = 4,
			GAErrorSeverityCritical = 5
		}

		#region public methods

		public static void NewEvent(GAErrorSeverity severity, string message)
		{
			CreateNewEvent(severity, message);
		}

		#endregion

		#region private methods

		private static void CreateNewEvent(GAErrorSeverity severity, string message)
		{
			GA_Wrapper.AddErrorEvent(severity, message);
		}

		#endregion
	}
}