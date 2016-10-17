using UnityEngine;
using System.Collections;

namespace GameAnalyticsSDK
{
	public enum GAErrorSeverity
	{
		Undefined = 0,
		Debug = 1,
		Info = 2,
		Warning = 3,
		Error = 4,
		Critical = 5
	}

	public enum GAProgressionStatus
	{
		//Undefined progression
		Undefined = 0,
		// User started progression
		Start = 1,
		// User succesfully ended a progression
		Complete = 2,
		// User failed a progression
		Fail = 3
	}

	public enum GAResourceFlowType
	{
		//Undefined progression
		Undefined = 0,
		// Source: Used when adding resource to a user
		Source = 1,
		// Sink: Used when removing a resource from a user
		Sink = 2
	}

	public enum GAGender
	{
		Undefined = 0,
		male = 1,
		female = 2
	}
}
