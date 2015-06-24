using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace GameAnalyticsSDK
{
	public static class GA_EditorUtilities {

		private const string XCaller = "unity";
		private const string XCallerKey = "X-Caller";
		private const string XCallerVersionKey = "X-Caller-Version";
		private const string XCallerPlatformKey = "X-Caller-Platform";
		private const string XAuthorizationKey = "X-Authorization";

		private static string XCallerVersion
		{
			get {
				return Application.unityVersion;
			}
		}

		private static string XCallerPlatform
		{
			get {
				EPlatform platform = EPlatform.Windows;

				PlatformID platformId = Environment.OSVersion.Platform;

				if (platformId == PlatformID.MacOSX) {
					platform = EPlatform.Mac;
				}
				else if (platformId == PlatformID.Unix) {
					platform = EPlatform.Linux;
				}
				else if (platformId == PlatformID.Win32NT || 
					platformId == PlatformID.Win32S ||
					platformId == PlatformID.Win32Windows ||
					platformId == PlatformID.WinCE) {

					platform = EPlatform.Mac;
				}

				return platform.ToString ();
			}
		}

		#if UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0

		public static Hashtable WWWHeaders()
		{
			Hashtable result = new Hashtable ();
			result [XCallerKey] = XCaller;
			result [XCallerVersionKey] = XCallerVersion;
			result [XCallerPlatformKey] = XCallerPlatform;

			return result;
		}

		public static Dictionary<string, string> WWWHeadersWithAuthorization(string token)
		{
			Hashtable result = new Hashtable ();
			result [XCallerKey] = XCaller;
			result [XCallerVersionKey] = XCallerVersion;
			result [XCallerPlatformKey] = XCallerPlatform;
			result [XAuthorizationKey] = token;

			return result;
		}

		#else

		public static Dictionary<string, string> WWWHeaders()
		{
			Dictionary<string, string> result = new Dictionary<string, string> ();
			result [XCallerKey] = XCaller;
			result [XCallerVersionKey] = XCallerVersion;
			result [XCallerPlatformKey] = XCallerPlatform;

			return result;
		}

		public static Dictionary<string, string> WWWHeadersWithAuthorization(string token)
		{
			Dictionary<string, string> result = new Dictionary<string, string> ();
			result [XCallerKey] = XCaller;
			result [XCallerVersionKey] = XCallerVersion;
			result [XCallerPlatformKey] = XCallerPlatform;
			result [XAuthorizationKey] = token;

			return result;
		}

		#endif
	}

	public enum EPlatform
	{
		Windows,
		Mac,
		Linux
	}
}
