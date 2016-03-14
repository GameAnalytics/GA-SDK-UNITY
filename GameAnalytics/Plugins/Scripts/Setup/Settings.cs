using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameAnalyticsSDK.Setup
{
	/// <summary>
	/// The Settings object contains an array of options which allows you to customize your use of GameAnalytics. Most importantly you will need to fill in your Game Key and Secret Key on the Settings object to use the service.
	/// </summary>
	/// 
	public class Settings : ScriptableObject
	{
		/// <summary>
		/// Types of help given in the help box of the GA inspector
		/// </summary>
		public enum HelpTypes
		{
			None,
			IncludeSystemSpecsHelp,
			ProvideCustomUserID}

		;

		public enum MessageTypes
		{
			None,
			Error,
			Info,
			Warning}

		;

		/// <summary>
		/// A message and message type for the help box displayed on the GUI inspector
		/// </summary>
		public struct HelpInfo
		{
			public string Message;
			public MessageTypes MsgType;
			public HelpTypes HelpType;
		}

		#region public static values

		/// <summary>
		/// The version of the GA Unity Wrapper plugin
		/// </summary>
		[HideInInspector]
		public static string VERSION = "3.3.1";

		#endregion

		#region public values

		public int TotalMessagesSubmitted;
		public int TotalMessagesFailed;
		
		public int DesignMessagesSubmitted;
		public int DesignMessagesFailed;
		public int QualityMessagesSubmitted;
		public int QualityMessagesFailed;
		public int ErrorMessagesSubmitted;
		public int ErrorMessagesFailed;
		public int BusinessMessagesSubmitted;
		public int BusinessMessagesFailed;
		public int UserMessagesSubmitted;
		public int UserMessagesFailed;

		public string CustomArea = string.Empty;

		[SerializeField]
		private string[] gameKey = CreateStringArrayWithEmptyStrings(NumberOfPlatforms);
		[SerializeField]
		private string[] secretKey = CreateStringArrayWithEmptyStrings(NumberOfPlatforms);
		[SerializeField]
		public string[] Build = CreateStringArrayWithPlatformStrings(NumberOfPlatforms);
		[SerializeField]
		public string[] SelectedPlatformStudio = CreateStringArrayWithEmptyStrings(NumberOfPlatforms);
		[SerializeField]
		public string[] SelectedPlatformGame = CreateStringArrayWithEmptyStrings(NumberOfPlatforms);
		[SerializeField]
		public int[] SelectedPlatformGameID = CreateIntArrayWithIDs(NumberOfPlatforms, -1);
		[SerializeField]
		public int[] SelectedStudio = CreateIntArrayWithIDs(NumberOfPlatforms, 0);
		[SerializeField]
		public int[] SelectedGame = CreateIntArrayWithIDs(NumberOfPlatforms, 0);

		public string NewVersion = "";
		public string Changes = "";

		public bool SignUpOpen = true;
		public string FirstName = "";
		public string LastName = "";
		public string StudioName = "";
		public string GameName = "";
		public string PasswordConfirm = "";
		public bool EmailOptIn = true;
		public string EmailGA = "";

		[System.NonSerialized]
		public string PasswordGA = "";
		[System.NonSerialized]
		public string TokenGA = "";
		[System.NonSerialized]
		public string ExpireTime = "";
		[System.NonSerialized]
		public string LoginStatus = "Not logged in.";
		[System.NonSerialized]
		public bool JustSignedUp = false;
		[System.NonSerialized]
		public bool HideSignupWarning = false;

		public bool IntroScreen = true;

		[System.NonSerialized]
		public List<Studio> Studios;

		public bool InfoLogEditor = true;
		public bool InfoLogBuild = true;
		public bool VerboseLogBuild = false;
		public bool SendExampleGameDataToMyGame = false;
		//public bool UseBundleVersion = false;

		public bool InternetConnectivity;

		public List<string> CustomDimensions01 = new List<string>();
		public List<string> CustomDimensions02 = new List<string>();
		public List<string> CustomDimensions03 = new List<string>();

		public List<string> ResourceItemTypes = new List<string>();
		public List<string> ResourceCurrencies = new List<string>();

		public GAPlatform LastCreatedGamePlatform;

		//These values are used for the GA_Inspector only
		public enum InspectorStates
		{
			Account,
			Basic,
			Debugging,
			Pref

		}

		private static string[] CreateStringArrayWithEmptyStrings(int size)
		{
			string[] result = new string[size];

			for(int i = 0; i < size; ++i)
			{
				result[i] = "";
			}

			return result;
		}

		private static string[] CreateStringArrayWithPlatformStrings(int size)
		{
			string[] result = new string[size];
			
			for(int i = 0; i < size; ++i)
			{
				result[i] = "1.0";
			}
			
			return result;
		}

		private static int[] CreateIntArrayWithIDs(int size, int value)
		{
			int[] result = new int[size];
			
			for(int i = 0; i < size; ++i)
			{
				result[i] = value;
			}
			
			return result;
		}

		public InspectorStates CurrentInspectorState;
		public List<HelpTypes> ClosedHints = new List<HelpTypes>();
		public bool DisplayHints;
		public Vector2 DisplayHintsScrollState;
		public Texture2D Logo;
		public Texture2D UpdateIcon;
		public Texture2D InfoIcon;
		public Texture2D DeleteIcon;
		public Texture2D GameIcon;
		public Texture2D HomeIcon;
		public Texture2D InstrumentIcon;
		public Texture2D QuestionIcon;
		public Texture2D UserIcon;

		public Texture2D AmazonIcon;
		public Texture2D GooglePlayIcon;
		public Texture2D iosIcon;
		public Texture2D macIcon;
		public Texture2D windowsPhoneIcon;

		[System.NonSerialized]
		public GUIStyle SignupButton;

		public bool UseCustomId = false;
		public bool SubmitErrors = true;
		public int MaxErrorCount = 10;
		public bool SubmitFpsAverage = true;
		public bool SubmitFpsCritical = true;
		public int FpsCriticalThreshold = 20;
		public int FpsCirticalSubmitInterval = 1;

		public bool[] PlatformFoldOut = new bool[NumberOfPlatforms];

		public bool CustomDimensions01FoldOut = false;
		public bool CustomDimensions02FoldOut = false;
		public bool CustomDimensions03FoldOut = false;
		
		public bool ResourceItemTypesFoldOut = false;
		public bool ResourceCurrenciesFoldOut = false;

		private const int NumberOfPlatforms = 4;

		#endregion

		#region public methods

		/// <summary>
		/// Sets a custom user ID.
		/// Make sure each user has a unique user ID. This is useful if you have your own log-in system with unique user IDs.
		/// NOTE: Only use this method if you have enabled "Custom User ID" on the GA inspector!
		/// </summary>
		/// <param name="customID">
		/// The custom user ID - this should be unique for each user
		/// </param>
		public void SetCustomUserID(string customID)
		{
			if(customID != string.Empty)
			{
				// Set custom ID native
			}
		}

		public bool IsGameKeyValid(int index, string value)
		{
			bool valid = true;

			for(int i = 0; i < NumberOfPlatforms; ++i)
			{
				if(index != i)
				{
					if(value.Equals(this.gameKey[i]))
					{
						valid = false;
						break;
					}
				}
			}

			return valid;
		}

		public bool IsSecretKeyValid(int index, string value)
		{
			bool valid = true;

			for(int i = 0; i < NumberOfPlatforms; ++i)
			{
				if(index != i)
				{
					if(value.Equals(this.secretKey[i]))
					{
						valid = false;
						break;
					}
				}
			}

			return valid;
		}

		public void UpdateGameKey(int index, string value)
		{
			if(!string.IsNullOrEmpty(value))
			{
				bool valid = this.IsGameKeyValid(index, value);

				if(valid)
				{
					this.gameKey[index] = value;
				}
				else if(this.gameKey[index].Equals(value))
				{
					this.gameKey[index] = "";
				}
			}
			else
			{
				this.gameKey[index] = value;
			}
		}

		public void UpdateSecretKey(int index, string value)
		{
			if(!string.IsNullOrEmpty(value))
			{
				bool valid = this.IsSecretKeyValid(index, value);

				if(valid)
				{
					this.secretKey[index] = value;
				}
				else if(this.secretKey[index].Equals(value))
				{
					this.secretKey[index] = "";
				}
			}
			else
			{
				this.secretKey[index] = value;
			}
		}

		public string GetGameKey(int index)
		{
			return this.gameKey[index];
		}

		public string GetSecretKey(int index)
		{
			return this.secretKey[index];
		}

		/// <summary>
		/// Sets a custom area string. An area is often just a level, but you can set it to whatever makes sense for your game. F.x. in a big open world game you will probably need custom areas to identify regions etc.
		/// By default, if no custom area is set, the Application.loadedLevelName string is used.
		/// </summary>
		/// <param name="customID">
		/// The custom area.
		/// </param>
		public void SetCustomArea(string customArea)
		{
			// Set custom area native
		}

		public void SetKeys(string gamekey, string secretkey)
		{
			// set keys native
		}

		#endregion
	}

	//[System.Serializable]
	public class Studio
	{
		public string Name { get; private set; }

		public string ID { get; private set; }
		
		//[SerializeField]
		public List<Game> Games { get; private set; }

		public Studio(string name, string id, List<Game> games)
		{
			Name = name;
			ID = id;
			Games = games;
		}

		public static string[] GetStudioNames(List<Studio> studios, bool addFirstEmpty = true)
		{
			if(studios == null)
			{
				return new string[] { "-" };
			}

			if(addFirstEmpty)
			{
				string[] names = new string[studios.Count + 1];
				names[0] = "-";

				string spaceAdd = "";
				for(int i = 0; i < studios.Count; i++)
				{
					names[i + 1] = studios[i].Name + spaceAdd;
					spaceAdd += " ";
				}
				
				return names;
			}
			else
			{
				string[] names = new string[studios.Count];

				string spaceAdd = "";
				for(int i = 0; i < studios.Count; i++)
				{
					names[i] = studios[i].Name + spaceAdd;
					spaceAdd += " ";
				}

				return names;
			}
		}

		public static string[] GetGameNames(int index, List<Studio> studios)
		{
			if(studios == null || studios[index].Games == null)
			{
				return new string[] { "-" };
			}
			
			string[] names = new string[studios[index].Games.Count + 1];
			names[0] = "-";
			
			string spaceAdd = "";
			for(int i = 0; i < studios[index].Games.Count; i++)
			{
				names[i + 1] = studios[index].Games[i].Name + spaceAdd;
				spaceAdd += " ";
			}
			
			return names;
		}
	}

	public class Game
	{
		public string Name { get; private set; }

		public int ID { get; private set; }

		public string GameKey { get; private set; }

		public string SecretKey { get; private set; }

		public Game(string name, int id, string gameKey, string secretKey)
		{
			Name = name;
			ID = id;
			GameKey = gameKey;
			SecretKey = secretKey;
		}
	}
}