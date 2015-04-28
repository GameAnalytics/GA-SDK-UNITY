/// <summary>
/// Holds all the field types used by the server
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameAnalyticsSDK
{
	public static class GA_ServerFieldTypes
	{
		public enum FieldType
		{
			UserID,
			SessionID,
			TimeStamp,
			System,
			Language,
			EventID,
			Value,
			Level,
			X,
			Y,
			Z,
			CrashID,
			Message,
			Currency,
			Amount,
			PurchaseID,
			Build,
			Gender,
			Birth_year,
			Country,
			State,
			Friend_Count,
			Ios_id,
			Android_id,
			Platform,
			Device,
			Os,
			OsVersion,
			Sdk,
			InstallPublisher,
			InstallSite,
			InstallCampaign,
			InstallAdgroup,
			InstallAd,
			InstallKeyword,
			FacebookID,
			Severity,
			AndroidAdID
		}
		
		/// <summary>
		/// Matches each of all the valid field types for all the GA services to an enum value
		/// </summary>
		public static Dictionary<FieldType, string> Fields = new Dictionary<FieldType, string>() {
			{ FieldType.UserID, "user_id" },
			{ FieldType.SessionID, "session_id" },
			{ FieldType.TimeStamp, "ts" },
			{ FieldType.System, "system" },
			{ FieldType.Language, "language" },
			{ FieldType.EventID, "event_id" },
			{ FieldType.Value, "value" },
			{ FieldType.Level, "area" },
			{ FieldType.X, "x" },
			{ FieldType.Y, "y" },
			{ FieldType.Z, "z" },
			{ FieldType.CrashID, "qa_id" },
			{ FieldType.Message, "message" },
			{ FieldType.Currency, "currency" },
			{ FieldType.Amount, "amount" },
			{ FieldType.PurchaseID, "business_id" },
			{ FieldType.Build, "build" },
			{ FieldType.Gender, "gender" },
			{ FieldType.Birth_year, "birth_year" },
			{ FieldType.Country, "country" },
			{ FieldType.State, "state" },
			{ FieldType.Friend_Count, "friend_count" },
			{ FieldType.Ios_id, "ios_id" },
			{ FieldType.Android_id, "android_id" },
			{ FieldType.Platform, "platform" },
			{ FieldType.Device, "device" },
			{ FieldType.Os, "os_major" },
			{ FieldType.OsVersion, "os_minor" },
			{ FieldType.Sdk, "sdk_version" },
			{ FieldType.InstallPublisher, "install_publisher" },
			{ FieldType.InstallSite, "install_site" },
			{ FieldType.InstallCampaign, "install_campaign" },
			{ FieldType.InstallAdgroup, "install_adgroup" },
			{ FieldType.InstallAd, "install_ad" },
			{ FieldType.InstallKeyword, "install_keyword" },
			{ FieldType.FacebookID, "facebook_id" },
			{ FieldType.Severity, "severity" },
			{ FieldType.AndroidAdID, "google_aid" }
		};
	}
}