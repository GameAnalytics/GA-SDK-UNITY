using UnityEngine;
using System.Collections;

namespace GameAnalyticsSDK
{
	public enum GAErrorSeverity
	{
		Debug = 1,
		Info = 2,
		Warning = 3,
		Error = 4,
		Critical = 5
	}

	public enum GAProgressionStatus
	{
		// User started progression
		Start = 1,
		// User succesfully ended a progression
		Complete = 2,
		// User failed a progression
		Fail = 3
	}

	public enum GAResourceFlowType
	{
		// Source: Used when adding resource to a user
		Source = 1,
		// Sink: Used when removing a resource from a user
		Sink = 2
	}

	public enum GAGender
	{
		Male = 1,
		Female = 2
	}

	public enum GAPlatform
	{
		None = 0,
		iOS = 1,
		Android = 2
	}

	public enum GAPlatformSignUp
	{
		iOS = 0,
		Android = 1
	}
}
