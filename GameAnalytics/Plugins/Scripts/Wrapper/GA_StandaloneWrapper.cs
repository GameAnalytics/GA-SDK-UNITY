using UnityEngine;
#if (UNITY_4_7 || UNITY_4_6)
using Unity.IO.Compression;
#else
using System.IO.Compression;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using GameAnalyticsSDK.Utilities;

namespace GameAnalyticsSDK.Wrapper
{
	public partial class GA_Wrapper
	{

		#if ((UNITY_EDITOR && GAME_ANALYTICS_IN_EDITOR) || (UNITY_STANDALONE && !UNITY_EDITOR))

		#region Unity standalone implementation

		private const string GAME_ANALYTICS_SESSION_NUM_KEY = "GameAnalytics_sessionNum";
		private const string GAME_ANALYTICS_TRANSACTION_NUM_KEY = "GameAnalytics_transactionNum";
		private const string GAME_ANALYTICS_ATTEMPT_NUM_KEY_SUFFIX = "GameAnalytics_attemptNum_";
		private static readonly DateTime _unixEpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static bool _isEnabled = false;
		private static string _gameKey = "";
		private static string _secretKey = "";
		private static HMACSHA256 _secretKeyHasher = null;
		private static bool _canLogInfo = true;
		private static bool _canLogDebug = true;
		private static Hashtable _defaultAnnotations = new Hashtable();
		private static Hashtable _helperHashTable = new Hashtable();
		private static int _serverTimestampAtStart = 0;
		private static int _clientTimeOffset = 0;
		private static string _engine_version = "";
		private static string _build = "";
		private static string _sessionId = null;
		private static ArrayList _customDimension1Values = null;
		private static ArrayList _customDimension2Values = null;
		private static ArrayList _customDimension3Values = null;
		private static ArrayList _availableResourceCurrenciesValues = null;
		private static ArrayList _availableResourceItemTypesValues = null;
		private static HashSet<string> _progressionEventsStarted = new HashSet<string>();
		// Queue for events to be sent
		private static List<string> _eventQueue = new List<string>();
		// Queue for events that are sent but pending response from GameAnalytics
		private static List<string> _sentEventQueue = new List<string>();

		private static void _initializeGA()
		{
			// Must initialise at the start of the session
			_initialiseSession();

			// Send the init endpoint
			_defaultAnnotations["platform"] = _getPlatformString();
			_defaultAnnotations["os_version"] = _getOSVersionString();
			_defaultAnnotations["sdk_version"] = "rest api v2";     // Must be this
			string json = GA_MiniJSON.JsonEncode(_defaultAnnotations);
			_sendRestRequest("init", json, _initRequestCallback);
		}

		private static void _initialiseSession()
		{
			int sessionNum = PlayerPrefs.GetInt(GAME_ANALYTICS_SESSION_NUM_KEY, 0);
			++sessionNum;
			PlayerPrefs.SetInt(GAME_ANALYTICS_SESSION_NUM_KEY, sessionNum);
		}

		private static void _initRequestCallback(WWW www)
		{
			// check for errors
			if (www.error != null)
			{
				_logError("init error: " + www.error);
			}
			else
			{
				if (_canLogDebug)
				{
					_logInfo("init ok: " + www.text);
				}
				// Parse the response from GameAnalytics server
				object gaInitDataObject = GA_MiniJSON.JsonDecode(www.text);
				if (gaInitDataObject != null && gaInitDataObject is Hashtable)
				{
					Hashtable gaInitData = (Hashtable)gaInitDataObject;
					if (gaInitData.ContainsKey("enabled"))
					{
						_isEnabled = (bool)gaInitData["enabled"];
					}
					if (gaInitData.ContainsKey("server_ts"))
					{
						_serverTimestampAtStart = Convert.ToInt32(gaInitData["server_ts"]);
						// Get current client time in unix timestamp
						int clientTimeStamp = (int)(DateTime.UtcNow - _unixEpochTime).TotalSeconds;
						_clientTimeOffset = clientTimeStamp - _serverTimestampAtStart;
					}

					if (_isEnabled)
					{
						// Start the session
						_initialiseDefaultAnnotations();
						_sessionStartEvent();
						_startSendEventsTimer();
					}
				}
			}
		}

		private static void _initialiseDefaultAnnotations()
		{
			// Some values are already initialised in _initialiseGA()

			// GameAnalytics collector version, current version is 2
			_defaultAnnotations["v"] = 2;
			_defaultAnnotations["device"] = _getDevice();
			_defaultAnnotations["user_id"] = _getUserId();
			_defaultAnnotations["manufacturer"] = _getManufacturer();
			_defaultAnnotations["connection_type"] = _getConnectionType();
			_defaultAnnotations["build"] = _getBuild();
			_defaultAnnotations["engine_version"] = _getEngineVersion();
			_defaultAnnotations["session_id"] = _getSessionId();
			_defaultAnnotations["session_num"] = _getSessionNum();
		}

		private static void _sessionStartEvent()
		{
			Hashtable startData = _helperHashTable;
			startData.Add("category", "user");
			_addEvent(startData);
		}

		private static void _addProgressionEvent(int progressionStatus, string eventId, int? score = null)
		{
			string progressionPrefix = _getProgressionEventPrefix(progressionStatus);
			string progressionEvent = progressionPrefix + eventId;

			Hashtable progressionEventData = _helperHashTable;
			progressionEventData.Add("category", "progression");
			progressionEventData.Add("event_id", progressionEvent);
			// For Completed and Failed progressions, some additional data are needed
			if (progressionStatus == 2 || progressionStatus == 3)
			{
				progressionEventData.Add("attempt_num", _getNextProgressionAttemptNumber(eventId));
				if (progressionStatus == 2)
				{
					// Delete the attempt count on completion
					_deleteKey(eventId);
				}
				if (score.HasValue)
				{
					progressionEventData.Add("score", score);
				}
			}
			_addEvent(progressionEventData);
		}

		#endregion


		#region REST methods

		private const string GAME_ANALYTICS_ENDPOINT_LIVE = "http://api.gameanalytics.com/v2/";
		private const string GAME_ANALYTICS_ENDPOINT_SANDBOX = "http://sandbox-api.gameanalytics.com/v2/";
		private const string GAME_ANALYTICS_GAME_KEY_SANDBOX = "5c6bcb5402204249437fb5a7a80a4959";
		private const string GAME_ANALYTICS_SECRET_KEY_SANDBOX = "16813a12f718bc5c620f56944e1abc3ea13ccbac";

		private const float SEND_EVENTS_INTERVAL = 20;
		private const int MAX_SEND_EVENTS_SIZE = 900000;    // 0.9 mb
		private static GameAnalytics _gameAnalyticsInstance = null;
		private static Coroutine _sendEventsCoroutine = null;

		private static System.Text.UTF8Encoding _utf8Encoding = new System.Text.UTF8Encoding();

		internal static void SetGameAnalyticsInstance(GameAnalytics instance)
		{
			_gameAnalyticsInstance = instance;
		}

		private static string _generateHmac(byte[] data)
		{
			byte[] hashmessage = _secretKeyHasher.ComputeHash(data);
			return System.Convert.ToBase64String(hashmessage);
		}

		private delegate void RestRequestCallback(WWW www);

		private static void _sendRestRequest(string endpoint, string jsonData, RestRequestCallback callback)
		{
			const bool USE_GZIP = true;

			byte[] dataToSend = null;

			if (USE_GZIP)
			{
				// Compress the input data using gzip
				using (var memoryStreamInput = new MemoryStream())
				{
					using (var gzipStream = new GZipStream(memoryStreamInput, CompressionMode.Compress))
					{
						using (var writer = new StreamWriter(gzipStream))
						{
							writer.Write(jsonData);
						}
					}
					dataToSend = memoryStreamInput.ToArray();
				}
			}
			else
			{
				dataToSend = _utf8Encoding.GetBytes(jsonData);
			}


			string HmacAuth = _generateHmac(dataToSend);

			// create headers
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Content-Type", "application/json");
			headers.Add("Authorization", HmacAuth);
			headers.Add("Content-Length", jsonData.Length.ToString());
			if (USE_GZIP)
			{
				headers.Add("Content-Encoding", "gzip");
			}

			string url = GAME_ANALYTICS_ENDPOINT_LIVE + _gameKey + "/" + endpoint;
			WWW www = new WWW(url, dataToSend, headers);
			if (_canLogDebug)
			{
				_logInfo("REST " + endpoint + " to " + url);
				_logInfo("REST data: " + jsonData);
			}
			_gameAnalyticsInstance.StartCoroutine(_restRequestCoroutine(www, callback));
		}

		private static IEnumerator _restRequestCoroutine(WWW www, RestRequestCallback callback)
		{
			yield return www;

			callback(www);
		}

		private static void _startSendEventsTimer()
		{
			if (_sendEventsCoroutine == null)
			{
				_sendEventsCoroutine = _gameAnalyticsInstance.StartCoroutine(_SendEventsQueueTick());
			}
		}

		private static void _stopSendEventsTimer()
		{
			if (_sendEventsCoroutine != null)
			{
				_gameAnalyticsInstance.StopCoroutine(_sendEventsCoroutine);
				_sendEventsCoroutine = null;
			}
		}

		private static IEnumerator _SendEventsQueueTick()
		{
			if (_canLogDebug)
			{
				_logInfo("_SendEventsQueueTick() called");
			}
			// Wait for next interval before sending events
			yield return new WaitForSeconds(SEND_EVENTS_INTERVAL);

			_sendEventsCoroutine = null;
			bool eventsSent = _sendQueuedEvents();
			if (!eventsSent)
			{
				// If nothing sent out, start waiting immediately, else wait for server response first
				_startSendEventsTimer();
			}
		}

		#endregion


		#region Helper functions

		private static void _logInfo(string message)
		{
			Debug.Log("[GameAnalytics] " + message);
		}

		private static void _logWarning(string message)
		{
			Debug.LogWarning("[GameAnalytics] " + message);
		}

		private static void _logError(string message)
		{
			Debug.LogError("[GameAnalytics] " + message);
		}

		private static string _getPlatformString()
		{
			// Allowable values: "ios", "android", "windows", "windows_phone", "blackberry", "roku", "tizen", "nacl", "mac_osx", "webplayer"
			string unityPlatform;
			switch (Application.platform)
			{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					unityPlatform = "windows";
					break;
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.OSXDashboardPlayer:
				case RuntimePlatform.OSXEditor:
					unityPlatform = "mac_osx";
					break;
				case RuntimePlatform.WindowsWebPlayer:
				case RuntimePlatform.OSXWebPlayer:
					unityPlatform = "webplayer";
					break;
				case RuntimePlatform.IPhonePlayer:
					unityPlatform = "ios";
					break;
				case RuntimePlatform.Android:
					unityPlatform = "android";
					break;
				case RuntimePlatform.MetroPlayerARM:
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.WP8Player:
					unityPlatform = "windows_phone";
					break;
				case RuntimePlatform.BlackBerryPlayer:
					unityPlatform = "blackberry";
					break;
				case RuntimePlatform.TizenPlayer:
					unityPlatform = "tizen";
					break;
				case RuntimePlatform.NaCl:
					unityPlatform = "nacl";
					break;
				default:
					unityPlatform = "windows";
					break;
			}
			return unityPlatform;
		}

		private static string _getOSVersionString()
		{
			// Unity's SystemInfo.operatingSystem returns
			//   "Windows 7 (6.1.7601) 64bit" on 64 bit Windows 7
			//   "Mac OS X 10.10.4" on OS X Yosemite
			//   "iPhone OS 8.4" on iOS 8.4
			//   "Android OS API-22" on Android 5.1
			//   But on older versions of Unity on Windows 10, it returns "Windows 8.1  (6.3.10586) 64bit" for Windows 10.0.10586 64 bit
			// Required pattern for GameAnalytics is
			//   "^(ios|android|windows|windows_phone|blackberry|roku|tizen|nacl|mac_osx|webplayer) [0-9]{0,5}(\\.[0-9]{0,5}){0,2}$"
			string osVersion = SystemInfo.operatingSystem;

			// Capture and process OS version information
			// For Windows
			System.Text.RegularExpressions.Match regexResult = Regex.Match(osVersion, @"Windows.*?\((\d{0,5}\.\d{0,5}\.(\d{0,5}))\)");
			if (regexResult.Success)
			{
				string versionNumberString = regexResult.Groups[1].Value;
				string buildNumberString = regexResult.Groups[2].Value;
				// Fix a bug in older versions of Unity where Windows 10 isn't recognised properly
				int buildNumber = 0;
				Int32.TryParse(buildNumberString, out buildNumber);
				if (buildNumber > 10000)
				{
					versionNumberString = "10.0." + buildNumberString;
				}
				return "windows " + versionNumberString;
			}
			// For OS X
			regexResult = Regex.Match(osVersion, @"Mac OS X (\d{0,5}\.\d{0,5}\.\d{0,5})");
			if (regexResult.Success)
			{
				return "mac_osx " + regexResult.Captures[0].Value;
			}
			// Not supporting other OS yet. The default version string won't be accepted by GameAnalytics
			return osVersion;
		}

		private static string _getDevice()
		{
			return SystemInfo.deviceModel;
		}

		private static string _getUserId()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		private static string _getManufacturer()
		{
			// As far as I know, there's no way in mono to get this information
			return "unknown";
		}

		private static string _getBuild()
		{
			return _build;
		}

		private static string _getEngineVersion()
		{
			return _engine_version;
		}

		private static string _getSessionId()
		{
			if (_sessionId == null)
			{
				_sessionId = Guid.NewGuid().ToString();
			}
			return _sessionId;
		}

		private static int _getSessionNum()
		{
			return PlayerPrefs.GetInt(GAME_ANALYTICS_SESSION_NUM_KEY);
		}

		private static string _getConnectionType()
		{
			// Only "offline", "wwan", "wifi", "lan" are allowed
			string connectionType = "offline";

			switch (Application.internetReachability)
			{
				case NetworkReachability.NotReachable:
					connectionType = "offline";
					break;
				case NetworkReachability.ReachableViaCarrierDataNetwork:
					connectionType = "wwan";
					break;
				case NetworkReachability.ReachableViaLocalAreaNetwork:
					// Try to find whether "lan" or "wlan" is available
					if (NetworkInterface.GetIsNetworkAvailable())
					{
						int bestNetworkTypeFound = 9999;    // 0 = lan, 1 = wlan
						NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
						foreach (NetworkInterface networkInterface in allNetworkInterfaces)
						{
							// Note: The fact that an interface is up doesn't mean it is connected to the Internet;
							// It may be connected only to the local lan
							// The only way is to ping the game server through the interface to test
							// But for simplicity, just assume that if it is up, it is connected to the Internet
							if (networkInterface.OperationalStatus == OperationalStatus.Up)
							{
								switch (networkInterface.NetworkInterfaceType)
								{
									case NetworkInterfaceType.Ethernet:
									case NetworkInterfaceType.GigabitEthernet:
										// Assume ethernet is the best
										bestNetworkTypeFound = 0;
										break;
									case NetworkInterfaceType.Wireless80211:
										// Assume wlan is second best
										if (bestNetworkTypeFound > 1)
										{
											bestNetworkTypeFound = 1;
										}
										break;
									case NetworkInterfaceType.Loopback:
									case NetworkInterfaceType.Tunnel:
										// Skip these 2 because they're present on Windows but usually not for Internet connectivity
										continue;
										// Note: wwan (mobile data) should already by handled by NetworkReachability.ReachableViaCarrierDataNetwork
								}
							}
						}
						// Return lan or wlan if found
						switch (bestNetworkTypeFound)
						{
							case 0:
								connectionType = "lan";
								break;
							case 1:
								connectionType = "wifi";
								break;
						}
					}
					break;
			}
			return connectionType;
		}

		private static int _getClientTimestampNow()
		{
			// Get current client time in unix timestamp
			int nowtimeStamp = (int)(DateTime.UtcNow - _unixEpochTime).TotalSeconds;
			return nowtimeStamp - _clientTimeOffset;
		}

		private static void _addRemoveDefaultAnnotation(string field, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				if (_defaultAnnotations.ContainsKey(field))
				{
					_defaultAnnotations.Remove(field);
				}
			}
			else
			{
				_defaultAnnotations[field] = value;
			}
		}

		private static void _addEvent(Hashtable eventData)
		{
			// Add timestamp
			eventData["client_ts"] = _getClientTimestampNow();
			// Combine with default annotations
			foreach (DictionaryEntry entry in _defaultAnnotations)
			{
				eventData[entry.Key] = entry.Value;
			}

			// Convert to string and add to the queue
			string eventDataJson = GA_MiniJSON.JsonEncode(eventData);
			_eventQueue.Add(eventDataJson);

			// Clear the hashtable so that it can be reused
			eventData.Clear();
		}

		private static bool _sendQueuedEvents()
		{
			bool anythingSent = false;
			int queueItemsToSend = 0;

			if (_eventQueue.Count > 0)
			{
				// Join up the events into a json array
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('[');
				int sentDataSize = 0;
				for (; queueItemsToSend < _eventQueue.Count; queueItemsToSend++)
				{
					string eventQueueItem = _eventQueue[queueItemsToSend];
					// Send data if total data size is within limits, or if the first item is larger then the size limit
					if ((sentDataSize + eventQueueItem.Length) < MAX_SEND_EVENTS_SIZE ||
						queueItemsToSend == 0)
					{
						if (queueItemsToSend > 0)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(eventQueueItem);
						sentDataSize += eventQueueItem.Length;
					}
					else
					{
						break;
					}
				}
				stringBuilder.Append(']');

				// Send off the data
				string dataToSend = stringBuilder.ToString();
				_sendRestRequest("events", dataToSend, SendEventsQueueCallback);

				// Copy the sent items to the sent list
				for (int eventQueueIndex = 0; eventQueueIndex < queueItemsToSend; eventQueueIndex++)
				{
					_sentEventQueue.Add(_eventQueue[eventQueueIndex]);
				}

				// Remove the sent items from event queue
				_eventQueue.RemoveRange(0, queueItemsToSend);

				anythingSent = true;
			}

			if (_canLogDebug)
			{
				_logInfo("_sendEventsQueue() sending " + queueItemsToSend + " items, remaining: " + _eventQueue.Count);
			}

			return anythingSent;
		}

		private static void SendEventsQueueCallback(WWW www)
		{
			// check for errors
			if (www.error == null)
			{
				if (_canLogDebug)
				{
					_logInfo("Events sent successfully: " + www.text);
				}
			}
			else
			{
				_logError("Errors sending events: " + www.error);
			}
			// Clear off the sent data queue, even if there's an error. Until there's a better error recovery method
			_sentEventQueue.Clear();
			// Start timer to send events again
			_startSendEventsTimer();
		}

		private static int _getNextIncrementalNumber(string incrementalNumberKey)
		{
			int incrementalNumber = 0;
			if (PlayerPrefs.HasKey(incrementalNumberKey))
			{
				incrementalNumber = PlayerPrefs.GetInt(incrementalNumberKey);
			}
			incrementalNumber++;
			PlayerPrefs.SetInt(incrementalNumberKey, incrementalNumber);
			return incrementalNumber;
		}

		private static void _deleteKey(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				PlayerPrefs.DeleteKey(key);
			}
		}

		private static int _getNextTransactionNumber()
		{
			return _getNextIncrementalNumber(GAME_ANALYTICS_TRANSACTION_NUM_KEY);
		}

		private static int _getNextProgressionAttemptNumber(string progressionEventId)
		{
			return _getNextIncrementalNumber(GAME_ANALYTICS_ATTEMPT_NUM_KEY_SUFFIX + progressionEventId);
		}

		private static string _getProgressionEventPrefix(int progressionStatus)
		{
			string progressionStatusString = null;
			switch (progressionStatus)
			{
				case 1:
					progressionStatusString = "Start:";
					break;
				case 2:
					progressionStatusString = "Complete:";
					break;
				case 3:
					progressionStatusString = "Fail:";
					break;
			}
			return progressionStatusString;
		}

		private static string _buildProgressionEventId(string progression01, string progression02, string progression03)
		{
			StringBuilder eventIdBuilder = new StringBuilder();
			eventIdBuilder.Append(progression01);

			if (!string.IsNullOrEmpty(progression02))
			{
				eventIdBuilder.Append(':');
				eventIdBuilder.Append(progression02);
			}

			if (!string.IsNullOrEmpty(progression03))
			{
				eventIdBuilder.Append(':');
				eventIdBuilder.Append(progression03);
			}
			return eventIdBuilder.ToString();
		}

		private static void addProgressionEventInternal(int progressionStatus,
			string progression01, string progression02, string progression03, int? score = null)
		{
			string eventId = _buildProgressionEventId(progression01, progression02, progression03);

			switch (progressionStatus)
			{
			case 1:
				if (_progressionEventsStarted.Contains(eventId))
				{
					// Add fail event
					_addProgressionEvent(3, eventId, score);
				}
				else
				{
					_progressionEventsStarted.Add(eventId);
				}
				break;
			case 2:
			case 3:
				if (_progressionEventsStarted.Contains(eventId))
				{
					_progressionEventsStarted.Remove(eventId);
				}
				break;
			}

			_addProgressionEvent(progressionStatus, eventId, score);
		}

		private static void addDesignEventInternal(string eventId, float? value = null)
		{
			Hashtable designEventData = _helperHashTable;
			designEventData.Add("category", "design");
			designEventData.Add("event_id", eventId);
			if (value.HasValue)
			{
				designEventData.Add("value", value.Value);
			}
			_addEvent(designEventData);
		}

		private static string _getSeverityString(int severity)
		{
			string severityString = null;
			switch (severity)
			{
			case 1:
				severityString = "debug";
				break;
			case 2:
				severityString = "info";
				break;
			case 3:
				severityString = "warning";
				break;
			case 4:
				severityString = "error";
				break;
			case 5:
				severityString = "critical";
				break;
			default:
				// This is an error in itself!
				severityString = "error";
				break;
			}
			return severityString;
		}

		#endregion

		#region GA_Wrapper implementations

		private static void configureAvailableCustomDimensions01(string list)
		{
			if (_canLogDebug)
			{
				_logInfo("setAvailableCustomDimensions01(" + list + ")");
			}
			_customDimension1Values = GA_MiniJSON.JsonDecode(list) as ArrayList;
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			if (_canLogDebug)
			{
				_logInfo("setAvailableCustomDimensions02(" + list + ")");
			}
			_customDimension2Values = GA_MiniJSON.JsonDecode(list) as ArrayList;
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			if (_canLogDebug)
			{
				_logInfo("setAvailableCustomDimensions03(" + list + ")");
			}
			_customDimension3Values = GA_MiniJSON.JsonDecode(list) as ArrayList;
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			if (_canLogDebug)
			{
				_logInfo("setAvailableResourceCurrencies(" + list + ")");
			}
			_availableResourceCurrenciesValues = GA_MiniJSON.JsonDecode(list) as ArrayList;
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			if (_canLogDebug)
			{
				_logInfo("setAvailableResourceItemTypes(" + list + ")");
			}
			_availableResourceItemTypesValues = GA_MiniJSON.JsonDecode(list) as ArrayList;
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			if (_canLogDebug)
			{
				_logInfo("setUnitySdkVersion(" + unitySdkVersion + ")");
			}
			// This REST API implementation must always be set to "rest api v2",
			// so ignoring the value passed in here
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			if (_canLogDebug)
			{
				_logInfo("setUnityEngineVersion(" + unityEngineVersion + ")");
			}
			if (_isEnabled)
			{
				_defaultAnnotations["engine_version"] = unityEngineVersion;
			}
			else
			{
				_engine_version = unityEngineVersion;
			}
		}

		private static void configureBuild(string build)
		{
			if (_canLogDebug)
			{
				_logInfo("setBuild(" + build + ")");
			}
			if (_isEnabled)
			{
				_defaultAnnotations["build"] = build;
			}
			else
			{
				_build = build;
			}
		}

		private static void configureUserId(string userId)
		{
			if (_canLogDebug)
			{
				_logInfo("configureUserId(" + userId + ")");
			}
		}

		private static void initialize(string gamekey, string gamesecret)
		{
			_gameKey = gamekey;
			_secretKey = gamesecret;
			// Generate the hasher that will be used for hasing data to send to GameAnalytics
			byte[] secretKeyBytes = _utf8Encoding.GetBytes(_secretKey);
			_secretKeyHasher = new HMACSHA256(secretKeyBytes);

			if (_canLogDebug)
			{
				_logInfo("initialize(" + gamekey + "," + gamesecret + ")");
			}
			_initializeGA();
		}

		private static void setCustomDimension01(string customDimension)
		{
			if (_canLogDebug)
			{
				_logInfo("setCustomDimension01(" + customDimension + ")");
			}
			setCustomDimensionInternal("custom_01", customDimension, _customDimension1Values);
		}

		private static void setCustomDimension02(string customDimension)
		{
			if (_canLogDebug)
			{
				_logInfo("setCustomDimension02(" + customDimension + ")");
			}
			setCustomDimensionInternal("custom_02", customDimension, _customDimension2Values);
		}

		private static void setCustomDimension03(string customDimension)
		{
			if (_canLogDebug)
			{
				_logInfo("setCustomDimension03(" + customDimension + ")");
			}
			setCustomDimensionInternal("custom_03", customDimension, _customDimension3Values);
		}

		private static void setCustomDimensionInternal(string dimension, string customDimension, ArrayList availableCustomDimensionValues)
		{
			if (_isEnabled)
			{
				if (customDimension == null ||
					availableCustomDimensionValues.IndexOf(customDimension) >= 0)
				{
					_addRemoveDefaultAnnotation(dimension, customDimension);
				}
				else
				{
					_logWarning("setCustomDimension: '" + customDimension + "' not valid for dimension " + dimension);
				}
			}
		}

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			if (_canLogDebug)
			{
				_logInfo("addBusinessEvent(" + currency + "," + amount + "," + itemType + "," + itemId + "," + cartType + "," + ")");
			}
			Hashtable businessEventData = _helperHashTable;
			businessEventData.Add("category", "business");
			businessEventData.Add("event_id", itemType + ":" + itemId);
			businessEventData.Add("amount", amount);
			businessEventData.Add("currency", currency);
			businessEventData.Add("transaction_num", _getNextTransactionNumber());
			businessEventData.Add("cart_type", cartType);
			_addEvent(businessEventData);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId)
		{
			if (_canLogDebug)
			{
				_logInfo("addResourceEvent(" + flowType + "," + currency + "," + amount + "," + itemType + "," + itemId + ")");
			}

			if (_availableResourceCurrenciesValues.IndexOf(currency) < 0)
			{
				_logWarning("addResourceEvent() unrecognised currency type: " + currency);
			}
			if (_availableResourceItemTypesValues.IndexOf(itemType) < 0)
			{
				_logWarning("addResourceEvent() unrecognised item type: " + itemType);
			}

			StringBuilder eventIdBuilder = new StringBuilder();
			eventIdBuilder.Append(flowType == 1 ? "Source" : "Sink");
			eventIdBuilder.Append(':');
			eventIdBuilder.Append(currency);
			eventIdBuilder.Append(':');
			eventIdBuilder.Append(itemType);
			eventIdBuilder.Append(':');
			eventIdBuilder.Append(itemId);

			Hashtable resourceEventData = _helperHashTable;
			resourceEventData.Add("category", "resource");
			resourceEventData.Add("event_id", eventIdBuilder.ToString());
			resourceEventData.Add("amount", amount);
			_addEvent(resourceEventData);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03)
		{
			if (_canLogDebug)
			{
				_logInfo("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + ")");
			}

			addProgressionEventInternal(progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			if (_canLogDebug)
			{
				_logInfo("addProgressionEvent(" + progressionStatus + "," + progression01 + "," + progression02 + "," + progression03 + "," + score + ")");
			}

			addProgressionEventInternal(progressionStatus, progression01, progression02, progression03, score);
		}

		private static void addDesignEvent(string eventId)
		{
			if (_canLogDebug)
			{
				_logInfo("addDesignEvent(" + eventId + ")");
			}
			addDesignEventInternal(eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value)
		{
			if (_canLogDebug)
			{
				_logInfo("addDesignEventWithValue(" + eventId + "," + value + ")");
			}
			addDesignEventInternal(eventId, value);
		}

		private static void addErrorEvent(int severity, string message)
		{
			if (_canLogDebug)
			{
				_logInfo("addErrorEvent(" + severity + "," + message + ")");
			}

			string severityString = _getSeverityString(severity);
			// Length is limited to 8192
			string messageToSend = message.Length <= 8192 ?
				message : message.Substring(0, 8192);

			Hashtable errorEventData = _helperHashTable;
			errorEventData.Add("category", "business");
			errorEventData.Add("severity", severityString);
			errorEventData.Add("message", messageToSend);
			_addEvent(errorEventData);
		}

		private static void addSessionEndEvent()
		{
			if (_canLogDebug)
			{
				_logInfo("addSessionEndEvent");
			}
			Hashtable sessionEndData = _helperHashTable;
			sessionEndData.Add("category", "session_end");
			int now = _getClientTimestampNow();
			int sessionLength = now - _serverTimestampAtStart;
			sessionEndData.Add("length", sessionLength);
			_addEvent(sessionEndData);

			// Since the app is terminating, fire off the event now
			_stopSendEventsTimer();
			_sendQueuedEvents();
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			if (_canLogDebug)
			{
				_logInfo("setInfoLog(" + enabled + ")");
			}
			_canLogInfo = enabled;
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			if (_canLogDebug)
			{
				_logInfo("setVerboseLog(" + enabled + ")");
			}
			_canLogDebug = enabled;
		}

		private static void setFacebookId(string facebookId)
		{
			if (_canLogDebug)
			{
				_logInfo("setFacebookId(" + facebookId + ")");
			}
			if (_isEnabled)
			{
				_addRemoveDefaultAnnotation("facebook_id", facebookId);
			}
		}

		private static void setGender(string gender)
		{
			if (_canLogDebug)
			{
				_logInfo("setGender(" + gender + ")");
			}
			if (_isEnabled)
			{
				_addRemoveDefaultAnnotation("gender", gender);
			}
		}

		private static void setBirthYear(int birthYear)
		{
			if (_canLogDebug)
			{
				_logInfo("setBirthYear(" + birthYear + ")");
			}
			_defaultAnnotations["birth_year"] = birthYear;
		}

		public static void AddSessionEndEvent()
		{
			addSessionEndEvent();
		}

		#endregion

		#endif
	}
}
