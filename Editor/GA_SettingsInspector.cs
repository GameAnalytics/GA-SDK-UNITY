/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;
using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Setup;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace GameAnalyticsSDK.Editor
{
    [CustomEditor(typeof(Settings))]
    public class GA_SettingsInspector : UnityEditor.Editor
    {
        public const bool IsCustomPackage = true;
        private const string AssetsPrependPath = IsCustomPackage ? "Packages/com.gameanalytics.sdk" : "Assets/GameAnalytics";

        private Settings settings;

        private GUIContent _publicKeyLabel = new GUIContent("Game Key", "Your GameAnalytics Game Key - copy/paste from the GA website.");
        private GUIContent _privateKeyLabel = new GUIContent("Secret Key", "Your GameAnalytics Secret Key - copy/paste from the GA website.");
        private GUIContent _emailLabel = new GUIContent("Email", "Your GameAnalytics user account email.");
        private GUIContent _passwordLabel = new GUIContent("Password", "Your GameAnalytics user account password. Must be at least 8 characters in length.");
        private GUIContent _organizationsLabel = new GUIContent("Org.", "Organizations tied to your GameAnalytics user account.");
        private GUIContent _studiosLabel = new GUIContent("Studio", "Studios tied to your GameAnalytics user account.");
        private GUIContent _gamesLabel = new GUIContent("Game", "Games tied to the selected GameAnalytics studio.");
        private GUIContent _build = new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
        private GUIContent _infoLogEditor = new GUIContent("Info Log Editor", "Show info messages from GA in the unity editor console when submitting data.");
        private GUIContent _infoLogBuild = new GUIContent("Info Log Build", "Show info messages from GA in builds (f.x. Xcode for iOS).");
        private GUIContent _verboseLogBuild = new GUIContent("Verbose Log Build", "Show full info messages from GA in builds (f.x. Xcode for iOS). Noet that this option includes long JSON messages sent to the server.");
        private GUIContent _useManualSessionHandling = new GUIContent("Use manual session handling", "Manually choose when to end and start a new session. Note initializing of the SDK will automatically start the first session.");

        private GUIContent _enableSDKInitEvent  = new GUIContent("Enable SDK Init Event (boot time on Android, iOS)", "Enable the SDK Init Event to automatically track the boot time (time from application launch to the GameAnalytics SDK initialization)");
        private GUIContent _enableHealthEvent   = new GUIContent("Enable Session Performance Metrics", "Enables automatic performance data collection across the whole session. This includes sampling fps, memory consumption & cpu usage without any noticeable performance impact.");

        private GUIContent _enableMemoryTracking     = new GUIContent("Health Memory Snapshots (Android, iOS)", "Performance & error events will take memory usage snapshots");
        private GUIContent _enableHardwareTracking   = new GUIContent("Health Hardware Info (Android, iOS)", "Memory information collected (if available) and added as properties to health events. Determining total device memory, system memory usage and app memory usage");
        private GUIContent _enableFPSHistogram       = new GUIContent("Submit Session FPS Histogram (Android, iOS)", "Enable FPS sampling across the entire session to ultimately send an FPS histogram at the end of the session. FPS insights can be reviewed in the GameAnalytics Health feature");
        private GUIContent _enableMemoryHistogram    = new GUIContent("Submit Memory Usage Histogram (Android, iOS)", "Enable memory usage sampling across the entire session to ultimately send an memory histogram at the end of the session. Memory insights can be reviewed in the GameAnalytics Health feature");

        private GUIContent _usePlayerSettingsBunldeVersionForBuild = new GUIContent("Send Version* (Android, iOS) as build number", "The SDK will automatically fetch the version* number on Android and iOS and send it as the GameAnalytics build number.");
        //private GUIContent _sendExampleToMyGame        = new GUIContent("Get Example Game Data", "If enabled data collected while playing the example tutorial game will be sent to your game (using your game key and secret key). Otherwise data will be sent to a premade GA test game, to prevent it from polluting your data.");
        private GUIContent _account = new GUIContent("Account", "This tab allows you to login and automatically retrieve your Game Key and Secret Key.");
        private GUIContent _setup = new GUIContent("Setup", "This tab shows general options which are relevant for a wide variety of messages sent to GameAnalytics.");
        private GUIContent _advanced = new GUIContent("Advanced", "This tab shows advanced and misc. options for the GameAnalytics SDK.");
        private GUIContent _customDimensions01 = new GUIContent("Custom Dimensions 01", "List of custom dimensions 01.");
        private GUIContent _customDimensions02 = new GUIContent("Custom Dimensions 02", "List of custom dimensions 02.");
        private GUIContent _customDimensions03 = new GUIContent("Custom Dimensions 03", "List of custom dimensions 03.");
        private GUIContent _resourceItemTypes = new GUIContent("Resource Item Types", "List of Resource Item Types.");
        private GUIContent _resourceCurrrencies = new GUIContent("Resource Currencies", "List of Resource Currencies.");
        private GUIContent _gaFpsAverage = new GUIContent("Submit Average FPS (Legacy)", "Submit the average frames per second. Warning: This FPS tracking approach will be replaced in a future update.");
        private GUIContent _gaFpsCritical = new GUIContent("Submit Critical FPS (Legacy)", "Submit a message whenever the frames per second falls below a certain threshold. The location of the Track Target will be used for critical FPS events. Warning: This FPS tracking approach will be replaced in a future update.");
        private GUIContent _gaFpsCriticalThreshold = new GUIContent("FPS <", "Frames per second threshold.");
        private GUIContent _gaSubmitErrors = new GUIContent("Submit Unity Errors Automatically", "Submit error and exception messages to the GameAnalytics server. Useful for getting relevant data when the game crashes, etc.");
        private GUIContent _gaNativeErrorReporting = new GUIContent("Submit Native Errors (Android, iOS) Automatically", "Submit error and exception messages from native errors and exceptions to the GameAnalytics server. Useful for getting relevant data when the game crashes, etc. from native code.");

        private GUIContent _gameSetupIcon;
        private bool _gameSetupIconOpen = false;
        private GUIContent _gameSetupIconMsg = new GUIContent("Your game and secret key will authenticate the game. Please set the build version too. All fields are required.");
        private GUIContent _customDimensionsIcon;
        private bool _customDimensionsIconOpen = false;
        private GUIContent _customDimensionsIconMsg = new GUIContent("Define your custom dimension values below. Values that are not defined will be ignored.");
        private GUIContent _resourceTypesIcon;
        private bool _resourceTypesIconOpen = false;
        private GUIContent _resourceTypesIconMsg = new GUIContent("Define all your resource currencies and resource item types. Values that are not defined will be ignored.");
        private GUIContent _advancedSettingsIcon;
        private bool _advancedSettingsIconOpen = false;
        private GUIContent _advancedSettingsIconMsg = new GUIContent("Advanced settings allows you to enable tracking of Unity errors and exceptions, and frames per second (for performance).");
        private GUIContent _debugSettingsIcon;
        private bool _debugSettingsIconOpen = false;
        private GUIContent _debugSettingsIconMsg = new GUIContent("Debug settings allows you to enable info log for the editor or for builds (Xcode, etc.). Enabling verbose logging will show additional JSON messages in builds.");

        private GUIContent  _healthEventIcon;
        private bool        _healthEventIconOpen = false;
        private GUIContent  _healthEventIconMsg  = new GUIContent("Enable automatic tracking of events to discover and address issues related to how a game is technically running on devices/clients. Tracking options include errors, fps/memory usage histograms, app boot and hardware configuration");

        private GUIContent _deleteIcon;
        private GUIContent _homeIcon;
        private GUIContent _infoIcon;
        private GUIContent _instrumentIcon;
        private GUIContent _questionIcon;

        private GUIStyle _orangeUpdateLabelStyle;
        private GUIStyle _orangeUpdateIconStyle;

        //private static readonly Texture2D _triggerAdNotEnabledTexture = new Texture2D(1, 1);
        private static bool _checkedProjectNames = false;

        private const string _unityToken = "KKy7MQNc2TEUOeK0EMtR";

        private const string _gaUrl = "https://platform.gameanalytics.com/ext/v1/";

        private const string _gaToolUrl = "https://tool.gameanalytics.com";

        private const string _gaForgotPasswordUrl = _gaToolUrl + "/forgot-password";

        private const string _gaSettingsUrl = _gaToolUrl + "/game/{0}/settings/general";

        private const string _gaOverviewUrl = _gaToolUrl + "/game/{0}/overview";

        private const string _gaLoginUrl = _gaToolUrl + "/login?";

        private const string _gaSignUpUrl = _gaToolUrl + "/signup";

        private const string _gaSupportUrl = "http://support.gameanalytics.com/";

        private const int MaxNumberOfDimensions = 20;

        private int selectedPlatformIndex = 0;
        private string[] availablePlatforms;

        private const int MAJOR_V = 0;
        private const int MINOR_V = 1;
        private const int PATCH_V = 2;
        private const int ALL_V   = 3;

        void OnEnable()
        {
            settings = target as Settings;

            if (settings.UpdateIcon == null)
            {
                settings.UpdateIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/update_orange.png", typeof(Texture2D));
            }

            if (settings.DeleteIcon == null)
            {
                settings.DeleteIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/delete.png", typeof(Texture2D));
            }

            if (settings.GameIcon == null)
            {
                settings.GameIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/game.png", typeof(Texture2D));
            }

            if (settings.HomeIcon == null)
            {
                settings.HomeIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/home.png", typeof(Texture2D));
            }

            if (settings.InfoIcon == null)
            {
                settings.InfoIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/info.png", typeof(Texture2D));
            }

            if (settings.InstrumentIcon == null)
            {
                settings.InstrumentIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/instrument.png", typeof(Texture2D));
            }

            if (settings.QuestionIcon == null)
            {
                settings.QuestionIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/question.png", typeof(Texture2D));
            }

            if (settings.UserIcon == null)
            {
                settings.UserIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/user.png", typeof(Texture2D));
            }

            if (_gameSetupIcon == null)
            {
                _gameSetupIcon = new GUIContent(settings.InfoIcon, "Game Setup.");
            }

            if (_customDimensionsIcon == null)
            {
                _customDimensionsIcon = new GUIContent(settings.InfoIcon, "Custom Dimensions.");
            }

            if (_resourceTypesIcon == null)
            {
                _resourceTypesIcon = new GUIContent(settings.InfoIcon, "Resource Types.");
            }

            if (_advancedSettingsIcon == null)
            {
                _advancedSettingsIcon = new GUIContent(settings.InfoIcon, "Advanced Settings.");
            }

            if (_debugSettingsIcon == null)
            {
                _debugSettingsIcon = new GUIContent(settings.InfoIcon, "Debug Settings.");
            }

            if (_healthEventIcon == null)
            {
                _healthEventIcon = new GUIContent(settings.InfoIcon, "Performance Metrics.");
            }

            if (_deleteIcon == null)
            {
                _deleteIcon = new GUIContent(settings.DeleteIcon, "Delete.");
            }

            if (_homeIcon == null)
            {
                _homeIcon = new GUIContent(settings.HomeIcon, "Your GameAnalytics webpage tool.");
            }

            if (_instrumentIcon == null)
            {
                _instrumentIcon = new GUIContent(settings.InstrumentIcon, "GameAnalytics setup guide.");
            }

            if (_questionIcon == null)
            {
                _questionIcon = new GUIContent(settings.QuestionIcon, "GameAnalytics support.");
            }

            if (settings.Logo == null)
            {
                settings.Logo = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/gaLogo.png", typeof(Texture2D));
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.Space();

            if (settings.SignupButton == null)
            {
                GUIStyle signupButton = new GUIStyle(GUI.skin.button);
                signupButton.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/default.png", typeof(Texture2D));
                signupButton.active.background = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetsPrependPath + "/Gizmos/GameAnalytics/Images/active.png", typeof(Texture2D));
                signupButton.normal.textColor = Color.white;
                signupButton.active.textColor = Color.white;
                signupButton.fontSize = 14;
                signupButton.fontStyle = FontStyle.Bold;
                settings.SignupButton = signupButton;
            }

            #region Header section

            GUILayout.BeginHorizontal();

            GUILayout.Label(settings.Logo, new GUILayoutOption[] {
                GUILayout.Width(32),
                GUILayout.Height(32)
            });

            GUILayout.BeginVertical();

            GUILayout.Space(8);

            GUILayout.BeginHorizontal();

            GUILayout.Label("Unity SDK v." + Settings.VERSION);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            DrawLinkButton(_homeIcon, GUI.skin.label, _gaLoginUrl, GUILayout.Width(24), GUILayout.Height(24));

            DrawLinkButton(_questionIcon, GUI.skin.label, "http://support.gameanalytics.com/", GUILayout.Width(24), GUILayout.Height(24));

            DrawLinkButton(_instrumentIcon, GUI.skin.label, _gaSignUpUrl, GUILayout.Width(24), GUILayout.Height(24));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            string updateStatus = GA_UpdateWindow.UpdateStatus(Settings.VERSION);

            if (!updateStatus.Equals(string.Empty))
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);

                _orangeUpdateLabelStyle = new GUIStyle(EditorStyles.label);
                _orangeUpdateLabelStyle.normal.textColor = new Color(0.875f, 0.309f, 0.094f);

                _orangeUpdateIconStyle = new GUIStyle(EditorStyles.label);

                if (GUILayout.Button(settings.UpdateIcon, _orangeUpdateIconStyle, GUILayout.MaxWidth(17)))
                {
                    OpenUpdateWindow();
                }

                GUILayout.Label(updateStatus, _orangeUpdateLabelStyle);

                if (settings.Organizations == null)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }
            }
            else
            {
                if (settings.Organizations != null)
                {
                    GUILayout.BeginHorizontal();
                }
                else
                {
                    GUILayout.Space(22);
                }
            }

            if (settings.Organizations != null)
            {
                GUILayout.FlexibleSpace();

                float minW = 0;
                float maxW = 0;
                GUIContent email = new GUIContent(settings.EmailGA);
                EditorStyles.miniLabel.CalcMinMaxWidth(email, out minW, out maxW);
                GUILayout.Label(email, EditorStyles.miniLabel, GUILayout.MaxWidth(maxW));

                GUILayout.BeginVertical();
                //GUILayout.Space(-1);

                if (GUILayout.Button("Log out", GUILayout.MaxWidth(67)))
                {
                    settings.Organizations = null;
                    SetLoginStatus("Not logged in.", settings);
                }

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            #endregion // Header section

            #region IntroScreen
            if (settings.IntroScreen)
            {
                bool finishIntro = false;
                for (int i = 0; i < settings.Platforms.Count; ++i)
                {
                    if (settings.GetGameKey(i).Length > 0 || settings.GetSecretKey(i).Length > 0)
                    {
                        finishIntro = true;
                        break;
                    }
                }

                if (finishIntro)
                {
                    settings.IntroScreen = false;
                }
                else
                {
                    if (!_checkedProjectNames && !EditorPrefs.GetBool("GA_Installed" + "-" + Application.dataPath, false))
                    {
                        _checkedProjectNames = true;

                        if (!PlayerSettings.companyName.Equals("DefaultCompany"))
                        {
                            settings.StudioName = PlayerSettings.companyName;
                        }
                        if (!PlayerSettings.productName.StartsWith("New Unity Project"))
                        {
                            settings.GameName = PlayerSettings.productName;
                        }
                        EditorPrefs.SetBool("GA_Installed" + "-" + Application.dataPath, true);
                        Selection.activeObject = settings;
                    }

                    GUILayout.Space(5);

                    Splitter(new Color(0.35f, 0.35f, 0.35f));

                    GUILayout.Space(10);

                    GUIStyle largeWhiteStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
                    if (!Application.HasProLicense())
                    {
                        largeWhiteStyle = new GUIStyle(EditorStyles.largeLabel);
                    }
                    largeWhiteStyle.fontSize = 16;
                    //largeWhiteStyle.fontStyle = FontStyle.Bold;

                    DrawLabelWithFlexibleSpace("Thank you for downloading!", largeWhiteStyle, 30);

                    GUILayout.Space(20);

                    GUIStyle greyStyle = new GUIStyle(EditorStyles.label);
                    greyStyle.fontSize = 12;

                    DrawLabelWithFlexibleSpace("Get started tracking your game by signing up to", greyStyle, 20);

                    GUILayout.Space(-5);

                    DrawLabelWithFlexibleSpace("GameAnalytics for FREE.", greyStyle, 20);

                    GUILayout.Space(20);

                    DrawButtonWithFlexibleSpace("Sign up", settings.SignupButton, OpenSignUp, GUILayout.Width(175), GUILayout.Height(40));

                    GUILayout.Space(15);

                    Splitter(new Color(0.35f, 0.35f, 0.35f));

                    GUILayout.Space(15);

                    DrawLabelWithFlexibleSpace("Already have an account? Please login", greyStyle, 20);

                    GUILayout.Space(15);

                    GUILayout.BeginHorizontal();
                    //GUILayout.Label("", GUILayout.Width(3));
                    GUILayout.Label(_emailLabel, GUILayout.Width(75));
                    settings.EmailGA = EditorGUILayout.TextField("", settings.EmailGA);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    //GUILayout.Label("", GUILayout.Width(3));
                    GUILayout.Label(_passwordLabel, GUILayout.Width(75));
                    settings.PasswordGA = EditorGUILayout.PasswordField("", settings.PasswordGA);
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(90));
                    if (GUILayout.Button("Login", new GUILayoutOption[] {
                        GUILayout.Width(130),
                        GUILayout.MaxHeight(30)
                    }))
                    {
                        settings.IntroScreen = false;
                        settings.SignUpOpen = false;
                        settings.CurrentInspectorState = Settings.InspectorStates.Account;

                        settings.Organizations = null;
                        SetLoginStatus("Contacting Server..", settings);
                        LoginUser(settings);
                    }
                    GUILayout.Label("", GUILayout.Width(10));
                    GUILayout.BeginVertical();
                    GUILayout.Space(8);

                    DrawLinkButton("Forgot password?", EditorStyles.label, _gaForgotPasswordUrl, GUILayout.Width(105));
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);

                    Splitter(new Color(0.35f, 0.35f, 0.35f));

                    GUILayout.Space(15);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("I want to fill in my game keys manually", EditorStyles.label, GUILayout.Width(207)))
                    {
                        settings.IntroScreen = false;
                        settings.CurrentInspectorState = Settings.InspectorStates.Basic;
                    }
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            #endregion // IntroScreen
            else
            {
                //Tabs
                GUILayout.BeginHorizontal();

                GUIStyle activeTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
                GUIStyle activeTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
                GUIStyle activeTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);

                activeTabStyle.normal = EditorStyles.miniButtonMid.active;
                activeTabStyleLeft.normal = EditorStyles.miniButtonLeft.active;
                activeTabStyleRight.normal = EditorStyles.miniButtonRight.active;

                GUIStyle inactiveTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
                GUIStyle inactiveTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
                GUIStyle inactiveTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);

                GUIStyle basicTabStyle = settings.CurrentInspectorState == Settings.InspectorStates.Basic ? activeTabStyleLeft : inactiveTabStyleLeft;

                if (settings.Organizations == null)
                {
                    if (GUILayout.Button(_account, settings.CurrentInspectorState == Settings.InspectorStates.Account ? activeTabStyleLeft : inactiveTabStyleLeft))
                    {
                        settings.CurrentInspectorState = Settings.InspectorStates.Account;
                    }

                    basicTabStyle = settings.CurrentInspectorState == Settings.InspectorStates.Basic ? activeTabStyle : inactiveTabStyle;
                }

                if (GUILayout.Button(_setup, basicTabStyle))
                {
                    settings.CurrentInspectorState = Settings.InspectorStates.Basic;
                }

                if (GUILayout.Button(_advanced, settings.CurrentInspectorState == Settings.InspectorStates.Pref ? activeTabStyleRight : inactiveTabStyleRight))
                {
                    settings.CurrentInspectorState = Settings.InspectorStates.Pref;
                }

                GUILayout.EndHorizontal();

                #region Settings.InspectorStates.Account
                if (settings.CurrentInspectorState == Settings.InspectorStates.Account)
                {
                    EditorGUILayout.Space();

                    GUILayout.Label("Already have an account with GameAnalytics?", EditorStyles.largeLabel);

                    EditorGUILayout.Space();

                    if (!string.IsNullOrEmpty(settings.LoginStatus) && !settings.LoginStatus.Equals("Not logged in."))
                    {
                        EditorGUILayout.Space();
                        if (settings.JustSignedUp && !settings.HideSignupWarning)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            EditorGUILayout.HelpBox("Please be aware that our service might take a few minutes to get ready to receive events. Click here to open Integration Status to follow the progress as you start sending events.", MessageType.Warning);
                            Rect r = GUILayoutUtility.GetLastRect();
                            if (GUI.Button(r, "", EditorStyles.label))
                            {
                                //Application.OpenURL("https://go.gameanalytics.com/login?token=" + settings.TokenGA + "&exp=" + settings.ExpireTime + "&goto=/game/" + settings.Studios[settings.SelectedStudio - 1].Games[settings.SelectedGame - 1].ID + "/initialize");
                            }
                            EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
                            if (GUILayout.Button("X"))
                            {
                                settings.HideSignupWarning = true;
                            }
                            GUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }
                        GUILayout.BeginHorizontal();
                        //GUILayout.Label("", GUILayout.Width(7));
                        GUILayout.Label("Status", GUILayout.Width(88));
                        GUILayout.Label(settings.LoginStatus);
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    if (settings.Organizations == null)
                    {
                        GUILayout.Label(_emailLabel, GUILayout.Width(75));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(-17));
                        settings.EmailGA = EditorGUILayout.TextField("", settings.EmailGA, GUILayout.MaxWidth(270));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(12);

                        GUILayout.Label(_passwordLabel, GUILayout.Width(75));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(-17));
                        settings.PasswordGA = EditorGUILayout.PasswordField("", settings.PasswordGA, GUILayout.MaxWidth(270));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(12);

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(2);
                        if (GUILayout.Button("Login", new GUILayoutOption[] {
                            GUILayout.Width(130),
                            GUILayout.MaxHeight(40)
                        }))
                        {
                            settings.Organizations = null;
                            SetLoginStatus("Contacting Server..", settings);
                            LoginUser(settings);
                        }
                        GUILayout.Label("", GUILayout.Width(10));
                        GUILayout.BeginVertical();
                        GUILayout.Space(14);
                        if (GUILayout.Button("Forgot password?", EditorStyles.label, GUILayout.Width(105)))
                        {
                            Application.OpenURL(_gaForgotPasswordUrl);
                        }
                        EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        GUILayout.Space(20);

                        Splitter(new Color(0.35f, 0.35f, 0.35f));

                        GUILayout.Space(16);

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("I want to fill in my game keys manually", EditorStyles.label, GUILayout.Width(207)))
                        {
                            settings.CurrentInspectorState = Settings.InspectorStates.Basic;
                        }
                        EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }
                }
                #endregion // Settings.InspectorStates.Account
                #region Settings.InspectorStates.Basic
                else if (settings.CurrentInspectorState == Settings.InspectorStates.Basic)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Space(-4);
                    GUILayout.Label("Game Setup", EditorStyles.largeLabel);
                    GUILayout.EndVertical();

                    #region Setup help
                    if (!_gameSetupIconOpen)
                    {
                        GUI.color = new Color(0.54f, 0.54f, 0.54f);
                    }
                    if (GUILayout.Button(_gameSetupIcon, GUIStyle.none, new GUILayoutOption[] {
                        GUILayout.Width(12),
                        GUILayout.Height(12)
                    }))
                    {
                        _gameSetupIconOpen = !_gameSetupIconOpen;
                    }
                    GUI.color = Color.white;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();

                    if (_gameSetupIconOpen)
                    {
                        GUILayout.BeginHorizontal();
                        TextAnchor tmpAnchor = GUI.skin.box.alignment;
                        GUI.skin.box.alignment = TextAnchor.UpperLeft;
                        Color tmpColor = GUI.skin.box.normal.textColor;
                        GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                        RectOffset tmpOffset = GUI.skin.box.padding;
                        GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                        GUILayout.Box(_gameSetupIconMsg);
                        GUI.skin.box.alignment = tmpAnchor;
                        GUI.skin.box.normal.textColor = tmpColor;
                        GUI.skin.box.padding = tmpOffset;
                        //GUILayout.Label("Advanced settings are pretty awesome! They allow you to do all kinds of things, such as tracking Unity errors and exceptions, and frames per second (for performance). See http://www.support.gameanalytics.com", EditorStyles.wordWrappedMiniLabel);
                        GUILayout.EndHorizontal();

                        Rect tmpRect = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                        {
                            Application.OpenURL("https://docs.gameanalytics.com/integrations/sdk/unity");
                        }
                    }
                    #endregion // Setup help

                    EditorGUILayout.Space();

                    if (!string.IsNullOrEmpty(settings.LoginStatus) && !settings.LoginStatus.Equals("Not logged in."))
                    {
                        if (settings.JustSignedUp && !settings.HideSignupWarning)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            EditorGUILayout.HelpBox("Please be aware that our service might take a few minutes to get ready to receive events. Click here to open Integration Status to follow the progress as you start sending events.", MessageType.Warning);
                            Rect r = GUILayoutUtility.GetLastRect();
                            if (GUI.Button(r, "", EditorStyles.label))
                            {
                                //Application.OpenURL("https://go.gameanalytics.com/login?token=" + settings.TokenGA + "&exp=" + settings.ExpireTime + "&goto=/game/" + settings.Studios[settings.SelectedStudio - 1].Games[settings.SelectedGame - 1].ID + "/initialize");
                            }
                            EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
                            if (GUILayout.Button("X"))
                            {
                                settings.HideSignupWarning = true;
                            }
                            GUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }

                        GUILayout.BeginHorizontal();
                        //GUILayout.Label("", GUILayout.Width(7));
                        GUILayout.Label("Status", GUILayout.Width(63));
                        GUILayout.Label(settings.LoginStatus);
                        GUILayout.EndHorizontal();
                    }

                    Splitter(new Color(0.35f, 0.35f, 0.35f));

                    // sanity check
                    if(settings.SelectedPlatformOrganization.Count != settings.Platforms.Count)
                    {
                        int diff = settings.SelectedPlatformOrganization.Count - settings.Platforms.Count;

                        if(diff < 0)
                        {
                            int absDiff = Mathf.Abs(diff);

                            for(int i = 0; i < absDiff; ++i)
                            {
                                settings.SelectedPlatformOrganization.Add("");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < diff; ++i)
                            {
                                settings.SelectedPlatformOrganization.RemoveAt(settings.SelectedPlatformOrganization.Count - 1);
                            }
                        }
                    }

                    int platformToRemove = -1;

                    for (int i = 0; i < settings.Platforms.Count; ++i)
                    {
                        settings.PlatformFoldOut[i] = EditorGUILayout.Foldout(settings.PlatformFoldOut[i], PlatformToString(settings.Platforms[i]));

                        if (settings.PlatformFoldOut[i])
                        {
                            if (settings.Organizations != null && settings.Organizations.Count > 0 && i < settings.SelectedOrganization.Count)
                            {
                                EditorGUILayout.Space();
                                //Splitter(new Color(0.35f, 0.35f, 0.35f));

                                GUILayout.BeginHorizontal();
                                //GUILayout.Label("", GUILayout.Width(7));
                                GUILayout.Label(_organizationsLabel, GUILayout.Width(50));
                                string[] organizationNames = Organization.GetOrganizationNames(settings.Organizations);
                                if (settings.SelectedOrganization[i] >= organizationNames.Length)
                                {
                                    settings.SelectedOrganization[i] = 0;
                                }
                                int tmpSelectedOrganization = settings.SelectedOrganization[i];
                                settings.SelectedOrganization[i] = EditorGUILayout.Popup("", settings.SelectedOrganization[i], organizationNames);
                                if (tmpSelectedOrganization != settings.SelectedOrganization[i])
                                {
                                    settings.SelectedStudio[i] = 0;
                                    settings.SelectedGame[i] = 0;
                                }
                                GUILayout.EndHorizontal();

                                if (settings.SelectedOrganization[i] > 0)
                                {
                                    if (tmpSelectedOrganization != settings.SelectedOrganization[i])
                                    {
                                        SelectOrganization(settings.SelectedOrganization[i], settings, i);
                                    }

                                    GUILayout.BeginHorizontal();
                                    //GUILayout.Label("", GUILayout.Width(7));
                                    GUILayout.Label(_studiosLabel, GUILayout.Width(50));
                                    string[] studioNames = Studio.GetStudioNames(settings.Organizations[settings.SelectedOrganization[i] - 1].Studios);
                                    if (settings.SelectedStudio[i] >= studioNames.Length)
                                    {
                                        settings.SelectedStudio[i] = 0;
                                    }
                                    int tmpSelectedStudio = settings.SelectedStudio[i];
                                    settings.SelectedStudio[i] = EditorGUILayout.Popup("", settings.SelectedStudio[i], studioNames);
                                    GUILayout.EndHorizontal();

                                    if (settings.SelectedStudio[i] > 0)
                                    {
                                        if (tmpSelectedStudio != settings.SelectedStudio[i])
                                        {
                                            SelectStudio(settings.SelectedStudio[i], settings, i);
                                        }

                                        GUILayout.BeginHorizontal();
                                        //GUILayout.Label("", GUILayout.Width(7));
                                        GUILayout.Label(_gamesLabel, GUILayout.Width(50));
                                        string[] gameNames = Studio.GetGameNames(settings.SelectedStudio[i] - 1, settings.Organizations[settings.SelectedOrganization[i] - 1].Studios);
                                        if (settings.SelectedGame[i] >= gameNames.Length)
                                        {
                                            settings.SelectedGame[i] = 0;
                                        }

                                        int tmpSelectedGame = settings.SelectedGame[i];
                                        settings.SelectedGame[i] = EditorGUILayout.Popup("", settings.SelectedGame[i], gameNames);
                                        GUILayout.EndHorizontal();

                                        if (settings.SelectedStudio[i] > 0 && tmpSelectedGame != settings.SelectedGame[i])
                                        {
                                            SelectGame(settings.SelectedGame[i], settings, i);
                                        }
                                    }
                                    else if (tmpSelectedStudio != settings.SelectedStudio[i])
                                    {
                                        SetLoginStatus("Please select studio..", settings);
                                    }
                                }
                                else if (tmpSelectedOrganization != settings.SelectedOrganization[i])
                                {
                                    SetLoginStatus("Please select organization..", settings);
                                }
                            }
                            else
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(_organizationsLabel, GUILayout.Width(85));
                                GUILayout.Space(-10);
                                GUILayout.Label(!string.IsNullOrEmpty(settings.SelectedPlatformOrganization[i]) ? settings.SelectedPlatformOrganization[i] : "N/A");
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label(_studiosLabel, GUILayout.Width(85));
                                GUILayout.Space(-10);
                                GUILayout.Label(!string.IsNullOrEmpty(settings.SelectedPlatformStudio[i]) ? settings.SelectedPlatformStudio[i] : "N/A");
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label(_gamesLabel, GUILayout.Width(85));
                                GUILayout.Space(-10);
                                GUILayout.Label(!string.IsNullOrEmpty(settings.SelectedPlatformGame[i]) ? settings.SelectedPlatformGame[i] : "N/A");
                                GUILayout.EndHorizontal();
                            }

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(_publicKeyLabel, GUILayout.Width(70));
                            GUILayout.Space(-10);
                            string beforeGameKey = settings.GetGameKey(i);
                            string tmpGameKey = EditorGUILayout.TextField("", settings.GetGameKey(i));

                            if (!tmpGameKey.Equals(beforeGameKey))
                            {
                                settings.SelectedPlatformOrganization[i] = "";
                                settings.SelectedPlatformStudio[i] = "";
                                settings.SelectedPlatformGame[i] = "";
                            }

                            settings.UpdateGameKey(i, tmpGameKey);

                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label(_privateKeyLabel, GUILayout.Width(70));
                            GUILayout.Space(-10);
                            string beforeSecretKey = settings.GetSecretKey(i);
                            string tmpSecretKey = EditorGUILayout.TextField("", settings.GetSecretKey(i));

                            if (!tmpSecretKey.Equals(beforeSecretKey))
                            {
                                settings.SelectedPlatformOrganization[i] = "";
                                settings.SelectedPlatformStudio[i] = "";
                                settings.SelectedPlatformGame[i] = "";
                            }

                            settings.UpdateSecretKey(i, tmpSecretKey);

                            GUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                            switch (settings.UsePlayerSettingsBuildNumber)
                            {
                                case true:
                                    if (settings.Platforms[i] != RuntimePlatform.Android && settings.Platforms[i] != RuntimePlatform.IPhonePlayer)
                                    {
                                        GUILayout.BeginHorizontal();
                                        //GUILayout.Label("", GUILayout.Width(7));
                                        GUILayout.Label(_build, GUILayout.Width(60));
                                        settings.Build[i] = EditorGUILayout.TextField("", settings.Build[i]);
                                        GUILayout.EndHorizontal();

                                        EditorGUILayout.Space();
                                    }
                                    else
                                    {
                                        if (settings.Platforms[i] == RuntimePlatform.Android)
                                        {
                                            settings.Build[i] = PlayerSettings.bundleVersion;
                                            EditorGUILayout.HelpBox("Using Android Player Settings Version* number as build number in events. \nBuild number is currently set to \"" + settings.Build[i] + "\".", MessageType.Info);
                                        }
                                        if (settings.Platforms[i] == RuntimePlatform.IPhonePlayer)
                                        {
                                            settings.Build[i] = PlayerSettings.bundleVersion;
                                            EditorGUILayout.HelpBox("Using iOS Player Settings Version* number as build number in events. \nBuild number is currently set to \"" + settings.Build[i] + "\".", MessageType.Info);
                                        }
                                    }
                                    break;
                                case false:
                                    GUILayout.BeginHorizontal();
                                    //GUILayout.Label("", GUILayout.Width(7));
                                    GUILayout.Label(_build, GUILayout.Width(60));
                                    settings.Build[i] = EditorGUILayout.TextField("", settings.Build[i]);
                                    GUILayout.EndHorizontal();

                                    EditorGUILayout.Space();
                                    break;
                            }

                            if (settings.SelectedPlatformGameID[i] >= 0)
                            {
                                EditorGUILayout.Space();
                                GUILayout.BeginHorizontal();
                                //GUILayout.Label("View", GUILayout.Width(65));
                                if (GUILayout.Button("Integration Status"))
                                {
                                    if (string.IsNullOrEmpty(settings.TokenGA))
                                    {
                                        Application.OpenURL(String.Format(_gaOverviewUrl, settings.SelectedPlatformGameID[i]));
                                    }
                                    else
                                    {
                                        Application.OpenURL(_gaLoginUrl);
                                    }
                                }
                                if (GUILayout.Button("Game Settings"))
                                {
                                    if (string.IsNullOrEmpty(settings.TokenGA))
                                    {
                                        Application.OpenURL(String.Format(_gaSettingsUrl, settings.SelectedPlatformGameID[i]));
                                    }
                                    else
                                    {
                                        Application.OpenURL(_gaLoginUrl);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                        }

                        if (GUILayout.Button("Remove platform"))
                        {
                            platformToRemove = i;
                        }

                        Splitter(new Color(0.35f, 0.35f, 0.35f));
                    }

                    if (platformToRemove >= 0)
                    {
                        settings.RemovePlatformAtIndex(platformToRemove);
                        this.availablePlatforms = settings.GetAvailablePlatforms();
                        this.selectedPlatformIndex = 0;
                    }

                    if (this.availablePlatforms == null)
                    {
                        this.availablePlatforms = settings.GetAvailablePlatforms();
                    }

                    this.selectedPlatformIndex = EditorGUILayout.Popup("Platform to add", this.selectedPlatformIndex, this.availablePlatforms);
                    if (GUILayout.Button("Add platform"))
                    {
                        settings.AddPlatform((RuntimePlatform)System.Enum.Parse(typeof(RuntimePlatform), this.availablePlatforms[this.selectedPlatformIndex]));
                        this.availablePlatforms = settings.GetAvailablePlatforms();
                        this.selectedPlatformIndex = 0;
                    }

#if UNITY_IOS || UNITY_TVOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    // Do nothing
#else

                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    EditorGUILayout.HelpBox("PLEASE NOTICE: Currently the GameAnalytics Unity SDK does not support your selected build Platform. Please refer to the GameAnalytics documentation for additional information.", MessageType.Warning);

                    if (GUI.Button(GUILayoutUtility.GetLastRect(), "", GUIStyle.none))
                    {
                        Application.OpenURL("http://www.gameanalytics.com/docs");
                    }
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);

                    GUILayout.EndHorizontal();

#endif
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Space(-4);
                    GUILayout.Label("Custom Dimensions", EditorStyles.largeLabel);
                    GUILayout.EndVertical();

                    if (!_customDimensionsIconOpen)
                    {
                        GUI.color = new Color(0.54f, 0.54f, 0.54f);
                    }
                    if (GUILayout.Button(_customDimensionsIcon, GUIStyle.none, GUILayout.Width(12), GUILayout.Height(12)))
                    {
                        _customDimensionsIconOpen = !_customDimensionsIconOpen;
                    }
                    GUI.color = Color.white;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();

                    if (_customDimensionsIconOpen)
                    {
                        GUILayout.BeginHorizontal();
                        TextAnchor tmpAnchor = GUI.skin.box.alignment;
                        GUI.skin.box.alignment = TextAnchor.UpperLeft;
                        Color tmpColor = GUI.skin.box.normal.textColor;
                        GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                        RectOffset tmpOffset = GUI.skin.box.padding;
                        GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                        GUILayout.Box(_customDimensionsIconMsg);
                        GUI.skin.box.alignment = tmpAnchor;
                        GUI.skin.box.normal.textColor = tmpColor;
                        GUI.skin.box.padding = tmpOffset;
                        //GUILayout.Label("Advanced settings are pretty awesome! They allow you to do all kinds of things, such as tracking Unity errors and exceptions, and frames per second (for performance). See http://www.support.gameanalytics.com", EditorStyles.wordWrappedMiniLabel);
                        GUILayout.EndHorizontal();

                        Rect tmpRect = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                        {
                            Application.OpenURL("https://docs.gameanalytics.com/integrations/sdk/unity/advanced-setup#custom-dimensions");
                        }
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    // Custom dimensions 1
                    settings.CustomDimensions01FoldOut = EditorGUILayout.Foldout(settings.CustomDimensions01FoldOut, new GUIContent("   " + _customDimensions01.text + " (" + settings.CustomDimensions01.Count + " / " + MaxNumberOfDimensions + " values)", _customDimensions01.tooltip));

                    if (settings.CustomDimensions01FoldOut)
                    {
                        int removeIndex = -1;

                        for (int i = 0; i < settings.CustomDimensions01.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(21));
                            GUILayout.Label("-", GUILayout.Width(10));

                            settings.CustomDimensions01[i] = ValidateCustomDimensionEditor(EditorGUILayout.TextField(settings.CustomDimensions01[i]));

                            if (GUILayout.Button(_deleteIcon, GUI.skin.label, new GUILayoutOption[] {
                                GUILayout.Width(16),
                                GUILayout.Height(16)
                            }))
                            {
                                removeIndex = i;
                            }
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }

                        if (removeIndex >= 0)
                        {
                            settings.CustomDimensions01.RemoveAt(removeIndex);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(21));
                        if (GUILayout.Button("Add", GUILayout.Width(63)))
                        {
                            if (settings.CustomDimensions01.Count < MaxNumberOfDimensions)
                            {
                                settings.CustomDimensions01.Add("New (" + (settings.CustomDimensions01.Count + 1) + ")");
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    // Custom dimensions 2
                    settings.CustomDimensions02FoldOut = EditorGUILayout.Foldout(settings.CustomDimensions02FoldOut, new GUIContent("   " + _customDimensions02.text + " (" + settings.CustomDimensions02.Count + " / " + MaxNumberOfDimensions + " values)", _customDimensions02.tooltip));

                    if (settings.CustomDimensions02FoldOut)
                    {
                        int removeIndex = -1;

                        for (int i = 0; i < settings.CustomDimensions02.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(21));
                            GUILayout.Label("-", GUILayout.Width(10));

                            settings.CustomDimensions02[i] = ValidateCustomDimensionEditor(EditorGUILayout.TextField(settings.CustomDimensions02[i]));

                            if (GUILayout.Button(_deleteIcon, GUI.skin.label, new GUILayoutOption[] {
                                GUILayout.Width(16),
                                GUILayout.Height(16)
                            }))
                            {
                                removeIndex = i;
                            }
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }

                        if (removeIndex >= 0)
                        {
                            settings.CustomDimensions02.RemoveAt(removeIndex);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(21));
                        if (GUILayout.Button("Add", GUILayout.Width(63)))
                        {
                            if (settings.CustomDimensions02.Count < MaxNumberOfDimensions)
                            {
                                settings.CustomDimensions02.Add("New (" + (settings.CustomDimensions02.Count + 1) + ")");
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    // Custom dimensions 3
                    settings.CustomDimensions03FoldOut = EditorGUILayout.Foldout(settings.CustomDimensions03FoldOut, new GUIContent("   " + _customDimensions03.text + " (" + settings.CustomDimensions03.Count + " / " + MaxNumberOfDimensions + " values)", _customDimensions03.tooltip));

                    if (settings.CustomDimensions03FoldOut)
                    {
                        int removeIndex = -1;

                        for (int i = 0; i < settings.CustomDimensions03.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(21));
                            GUILayout.Label("-", GUILayout.Width(10));

                            settings.CustomDimensions03[i] = ValidateCustomDimensionEditor(EditorGUILayout.TextField(settings.CustomDimensions03[i]));

                            if (GUILayout.Button(_deleteIcon, GUI.skin.label, new GUILayoutOption[] {
                                GUILayout.Width(16),
                                GUILayout.Height(16)
                            }))
                            {
                                removeIndex = i;
                            }
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }

                        if (removeIndex >= 0)
                        {
                            settings.CustomDimensions03.RemoveAt(removeIndex);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(21));
                        if (GUILayout.Button("Add", GUILayout.Width(63)))
                        {
                            if (settings.CustomDimensions03.Count < MaxNumberOfDimensions)
                            {
                                settings.CustomDimensions03.Add("New (" + (settings.CustomDimensions03.Count + 1) + ")");
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Space(-4);
                    GUILayout.Label("Resource Types", EditorStyles.largeLabel);
                    GUILayout.EndVertical();

                    if (!_resourceTypesIconOpen)
                    {
                        GUI.color = new Color(0.54f, 0.54f, 0.54f);
                    }
                    if (GUILayout.Button(_resourceTypesIcon, GUIStyle.none, new GUILayoutOption[] {
                        GUILayout.Width(12),
                        GUILayout.Height(12)
                    }))
                    {
                        _resourceTypesIconOpen = !_resourceTypesIconOpen;
                    }
                    GUI.color = Color.white;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();

                    if (_resourceTypesIconOpen)
                    {
                        GUILayout.BeginHorizontal();
                        TextAnchor tmpAnchor = GUI.skin.box.alignment;
                        GUI.skin.box.alignment = TextAnchor.UpperLeft;
                        Color tmpColor = GUI.skin.box.normal.textColor;
                        GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                        RectOffset tmpOffset = GUI.skin.box.padding;
                        GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                        GUILayout.Box(_resourceTypesIconMsg);
                        GUI.skin.box.alignment = tmpAnchor;
                        GUI.skin.box.normal.textColor = tmpColor;
                        GUI.skin.box.padding = tmpOffset;
                        GUILayout.EndHorizontal();

                        Rect tmpRect = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                        {
                            Application.OpenURL("https://docs.gameanalytics.com/integrations/sdk/unity/advanced-setup#resource-types");
                        }
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    // Resource types

                    settings.ResourceCurrenciesFoldOut = EditorGUILayout.Foldout(settings.ResourceCurrenciesFoldOut, new GUIContent("   " + _resourceCurrrencies.text + " (" + settings.ResourceCurrencies.Count + " / " + MaxNumberOfDimensions + " values)", _resourceCurrrencies.tooltip));

                    if (settings.ResourceCurrenciesFoldOut)
                    {
                        int removeIndex = -1;

                        for (int i = 0; i < settings.ResourceCurrencies.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(21));
                            GUILayout.Label("-", GUILayout.Width(10));
                            settings.ResourceCurrencies[i] = ValidateResourceCurrencyEditor(EditorGUILayout.TextField(settings.ResourceCurrencies[i]));

                            if (GUILayout.Button(_deleteIcon, GUI.skin.label, new GUILayoutOption[] {
                                GUILayout.Width(16),
                                GUILayout.Height(16)
                            }))
                            {
                                removeIndex = i;
                            }
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }

                        if (removeIndex >= 0)
                        {
                            settings.ResourceCurrencies.RemoveAt(removeIndex);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(21));
                        if (GUILayout.Button("Add", GUILayout.Width(63)))
                        {
                            if (settings.ResourceCurrencies.Count < MaxNumberOfDimensions)
                            {
                                settings.ResourceCurrencies.Add("NewCurrency"); // + (settings.ResourceCurrencies.Count + 1));
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    settings.ResourceItemTypesFoldOut = EditorGUILayout.Foldout(settings.ResourceItemTypesFoldOut, new GUIContent("   " + _resourceItemTypes.text + " (" + settings.ResourceItemTypes.Count + " / " + MaxNumberOfDimensions + " values)", _resourceItemTypes.tooltip));

                    if (settings.ResourceItemTypesFoldOut)
                    {
                        int removeIndex = -1;

                        for (int i = 0; i < settings.ResourceItemTypes.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(21));
                            GUILayout.Label("-", GUILayout.Width(10));
                            //string tmp = settings.ResourceTypes[i];
                            settings.ResourceItemTypes[i] = ValidateResourceItemTypeEditor(EditorGUILayout.TextField(settings.ResourceItemTypes[i]));

                            if (GUILayout.Button(_deleteIcon, GUI.skin.label, new GUILayoutOption[] {
                                GUILayout.Width(16),
                                GUILayout.Height(16)
                            }))
                            {
                                removeIndex = i;
                            }
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }

                        if (removeIndex >= 0)
                        {
                            settings.ResourceItemTypes.RemoveAt(removeIndex);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("", GUILayout.Width(21));
                        if (GUILayout.Button("Add", GUILayout.Width(63)))
                        {
                            if (settings.ResourceItemTypes.Count < MaxNumberOfDimensions)
                            {
                                settings.ResourceItemTypes.Add("New (" + (settings.ResourceItemTypes.Count + 1) + ")");
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                }
                #endregion // Settings.InspectorStates.Basic
                #region Settings.InspectorStates.Pref
                else if (settings.CurrentInspectorState == Settings.InspectorStates.Pref)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Space(-4);
                    GUILayout.Label("Advanced Settings", EditorStyles.largeLabel);
                    GUILayout.EndVertical();

                    if (!_advancedSettingsIconOpen)
                    {
                        GUI.color = new Color(0.54f, 0.54f, 0.54f);
                    }
                    if (GUILayout.Button(_advancedSettingsIcon, GUIStyle.none, new GUILayoutOption[] {
                        GUILayout.Width(12),
                        GUILayout.Height(12)
                    }))
                    {
                        _advancedSettingsIconOpen = !_advancedSettingsIconOpen;
                    }
                    GUI.color = Color.white;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();

                    if (_advancedSettingsIconOpen)
                    {
                        GUILayout.BeginHorizontal();
                        TextAnchor tmpAnchor = GUI.skin.box.alignment;
                        GUI.skin.box.alignment = TextAnchor.UpperLeft;
                        Color tmpColor = GUI.skin.box.normal.textColor;
                        GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                        RectOffset tmpOffset = GUI.skin.box.padding;
                        GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                        GUILayout.Box(_advancedSettingsIconMsg);
                        GUI.skin.box.alignment = tmpAnchor;
                        GUI.skin.box.normal.textColor = tmpColor;
                        GUI.skin.box.padding = tmpOffset;
                        GUILayout.EndHorizontal();

                        Rect tmpRect = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                        {
                            Application.OpenURL("https://docs.gameanalytics.com/integrations/sdk/unity/advanced-setup#advanced-settings");
                        }
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.UseManualSessionHandling = EditorGUILayout.Toggle("", settings.UseManualSessionHandling, GUILayout.Width(35));
                    GUILayout.Label(_useManualSessionHandling);
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.UsePlayerSettingsBuildNumber = EditorGUILayout.Toggle("", settings.UsePlayerSettingsBuildNumber, GUILayout.Width(35));
                    GUILayout.Label(_usePlayerSettingsBunldeVersionForBuild);
                    GUILayout.EndHorizontal();

                    if (settings.UsePlayerSettingsBuildNumber)
                    {
                        EditorGUILayout.HelpBox("PLEASE NOTICE: The SDK will use the Version* number (Android, iOS) from Player Settings as the build number in events.", MessageType.Info);
                    }

                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.SubmitFpsAverage = EditorGUILayout.Toggle("", settings.SubmitFpsAverage, GUILayout.Width(35));
                    GUILayout.Label(_gaFpsAverage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.SubmitFpsCritical = EditorGUILayout.Toggle("", settings.SubmitFpsCritical, GUILayout.Width(35));
                    GUILayout.Label(_gaFpsCritical, GUILayout.Width(200));
                    GUI.enabled = settings.SubmitFpsCritical;
                    GUILayout.Label(_gaFpsCriticalThreshold, GUILayout.Width(40));
                    GUILayout.Label("", GUILayout.Width(-26));

                    int tmpFpsCriticalThreshold = 0;
                    if (int.TryParse(EditorGUILayout.TextField(settings.FpsCriticalThreshold.ToString(), GUILayout.Width(45)), out tmpFpsCriticalThreshold))
                    {
                        settings.FpsCriticalThreshold = Mathf.Max(Mathf.Min(tmpFpsCriticalThreshold, 99), 5);
                    }
                    GUI.enabled = true;

                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Space(-4);
                    GUILayout.Label("Debug Settings", EditorStyles.largeLabel);
                    GUILayout.EndVertical();

                    if (!_debugSettingsIconOpen)
                    {
                        GUI.color = new Color(0.54f, 0.54f, 0.54f);
                    }
                    if (GUILayout.Button(_debugSettingsIcon, GUIStyle.none, new GUILayoutOption[] {
                        GUILayout.Width(12),
                        GUILayout.Height(12)
                    }))
                    {
                        _debugSettingsIconOpen = !_debugSettingsIconOpen;
                    }
                    GUI.color = Color.white;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();

                    if (_debugSettingsIconOpen)
                    {
                        GUILayout.BeginHorizontal();
                        TextAnchor tmpAnchor = GUI.skin.box.alignment;
                        GUI.skin.box.alignment = TextAnchor.UpperLeft;
                        Color tmpColor = GUI.skin.box.normal.textColor;
                        GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                        RectOffset tmpOffset = GUI.skin.box.padding;
                        GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                        GUILayout.Box(_debugSettingsIconMsg);
                        GUI.skin.box.alignment = tmpAnchor;
                        GUI.skin.box.normal.textColor = tmpColor;
                        GUI.skin.box.padding = tmpOffset;
                        GUILayout.EndHorizontal();

                        Rect tmpRect = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                        {
                            Application.OpenURL("https://docs.gameanalytics.com/integrations/sdk/unity/advanced-setup#advanced-settings");
                        }
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.InfoLogEditor = EditorGUILayout.Toggle("", settings.InfoLogEditor, GUILayout.Width(35));
                    GUILayout.Label(_infoLogEditor);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.InfoLogBuild = EditorGUILayout.Toggle("", settings.InfoLogBuild, GUILayout.Width(35));
                    GUILayout.Label(_infoLogBuild);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(-18));
                    settings.VerboseLogBuild = EditorGUILayout.Toggle("", settings.VerboseLogBuild, GUILayout.Width(35));
                    GUILayout.Label(_verboseLogBuild);
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    const int layoutWidth   = 35;

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            GUILayout.BeginVertical();
                            GUILayout.Space(-4);
                            GUILayout.Label("Health Tracking", EditorStyles.largeLabel);
                            GUILayout.EndVertical();

                            if (!_healthEventIconOpen)
                            {
                                GUI.color = new Color(0.54f, 0.54f, 0.54f);
                            }
                            if (GUILayout.Button(_healthEventIcon, GUIStyle.none, new GUILayoutOption[] {
                                GUILayout.Width(12),
                                GUILayout.Height(12)
                            }))
                            {
                                _healthEventIconOpen = !_healthEventIconOpen;
                            }
                            GUI.color = Color.white;
                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                            GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        if (_healthEventIconOpen)
                        {
                            GUILayout.BeginHorizontal();
                            TextAnchor tmpAnchor = GUI.skin.box.alignment;
                            GUI.skin.box.alignment = TextAnchor.UpperLeft;
                            Color tmpColor = GUI.skin.box.normal.textColor;
                            GUI.skin.box.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                            RectOffset tmpOffset = GUI.skin.box.padding;
                            GUI.skin.box.padding = new RectOffset(6, 6, 5, 32);
                            GUILayout.Box(_healthEventIconMsg);
                            GUI.skin.box.alignment = tmpAnchor;
                            GUI.skin.box.normal.textColor = tmpColor;
                            GUI.skin.box.padding = tmpOffset;
                            GUILayout.EndHorizontal();

                            Rect tmpRect = GUILayoutUtility.GetLastRect();
                            if (GUI.Button(new Rect(tmpRect.x + 5, tmpRect.y + tmpRect.height - 25, 80, 20), "Learn more"))
                            {
                                Application.OpenURL("https://docs.gameanalytics.com/features/health");
                            }
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUILayout.BeginVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(-12);
                            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.SubmitErrors = EditorGUILayout.Toggle("", settings.SubmitErrors, GUILayout.Width(35));
                            GUILayout.Label(_gaSubmitErrors);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.NativeErrorReporting = EditorGUILayout.Toggle("", settings.NativeErrorReporting, GUILayout.Width(35));
                            GUILayout.Label(_gaNativeErrorReporting);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.EnableSDKInitEvent = EditorGUILayout.Toggle("", settings.EnableSDKInitEvent, GUILayout.Width(layoutWidth));
                            GUILayout.Label(_enableSDKInitEvent);
                            GUILayout.EndHorizontal();

                        GUILayout.EndVertical();

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUILayout.BeginVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(-12);
                            EditorGUILayout.LabelField("Session Performance Event", EditorStyles.boldLabel);
                            GUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.EnableFPSHistogram = EditorGUILayout.Toggle("", settings.EnableFPSHistogram, GUILayout.Width(layoutWidth));
                            GUILayout.Label(_enableFPSHistogram);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.EnableMemoryHistogram = EditorGUILayout.Toggle("", settings.EnableMemoryHistogram, GUILayout.Width(layoutWidth));
                            GUILayout.Label(_enableMemoryHistogram);
                            GUILayout.EndHorizontal();

                        GUILayout.EndVertical();

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUILayout.BeginVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(-12);
                            EditorGUILayout.LabelField("EXPERIMENTAL", EditorStyles.boldLabel);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.EnableHardwareTracking = EditorGUILayout.Toggle("", settings.EnableHardwareTracking, GUILayout.Width(layoutWidth));
                            GUILayout.Label(_enableHardwareTracking);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(-18));
                            settings.EnableMemoryTracking = EditorGUILayout.Toggle("", settings.EnableMemoryTracking, GUILayout.Width(layoutWidth));
                            GUILayout.Label(_enableMemoryTracking);
                            GUILayout.EndHorizontal();

                        GUILayout.EndVertical();

                    GUILayout.EndVertical();
                }
                #endregion // Settings.InspectorStates.Pref
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
            }
        }

        private MessageType ConvertMessageType(Settings.MessageTypes msgType)
        {
            switch (msgType)
            {
                case Settings.MessageTypes.Error:
                    return MessageType.Error;
                case Settings.MessageTypes.Info:
                    return MessageType.Info;
                case Settings.MessageTypes.Warning:
                    return MessageType.Warning;
                default:
                    return MessageType.None;
            }
        }

        private static void LoginUser(Settings settings)
        {
            Hashtable jsonTable = new Hashtable();
            jsonTable["email"] = settings.EmailGA;
            jsonTable["password"] = settings.PasswordGA;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(GA_MiniJSON.Serialize(jsonTable));

            UnityWebRequest www = new UnityWebRequest(_gaUrl + "token", UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw uH = new UploadHandlerRaw(data)
            {
                contentType = "application/json"
            };
            www.uploadHandler = uH;
            www.downloadHandler = new DownloadHandlerBuffer();

            Dictionary<string, string> headers = GA_EditorUtilities.WWWHeaders();
            foreach (KeyValuePair<string, string> entry in headers)
            {
                www.SetRequestHeader(entry.Key, entry.Value);
            }

            GA_ContinuationManager.StartCoroutine(LoginUserFrontend(www, settings), () => www.isDone);
        }


        private static IEnumerator LoginUserFrontend(UnityWebRequest www, Settings settings)
        {

            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
                string error = "";
                IDictionary<string, object> returnParam = null;

                string text = www.downloadHandler.text;

                if (!string.IsNullOrEmpty(text))
                {
                    returnParam = GA_MiniJSON.Deserialize(text) as IDictionary<string, object>;

                    if (returnParam != null && returnParam.ContainsKey("errors"))
                    {
                        IList<object> errorList = returnParam["errors"] as IList<object>;
                        if (errorList != null && errorList.Count > 0)
                        {
                            IDictionary<string, object> errors = errorList[0] as IDictionary<string, object>;
                            if (errors.ContainsKey("msg"))
                            {
                                error = errors["msg"].ToString();
                            }
                        }
                    }
                }

                if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError))
                {
                    if (!String.IsNullOrEmpty(error))
                    {
                        SetLoginStatus("Failed to login.", settings);
                    }
                    else if (returnParam != null)
                    {
                        IList<object> resultList = returnParam["results"] as IList<object>;
                        IDictionary<string, object> results = resultList[0] as IDictionary<string, object>;
                        settings.TokenGA = results["token"].ToString();

                        SetLoginStatus("Logged in. Getting data.", settings);

                        GetUserData(settings);
                    }
                }
                else if (www.responseCode == 301 || www.responseCode == 404 || www.responseCode == 410)
                {
                    Debug.LogError("Failed to login. GameAnalytics request not successful. API was changed. Please update your SDK to the latest version: " + www.error + " " + error);
                    SetLoginStatus("Failed to login. GameAnalytics request not successful. API was changed. Please update your SDK to the latest version.", settings);
                }
                else
                {
                    Debug.LogError("Failed to login: " + www.error + " " + error);
                    SetLoginStatus("Failed to login.", settings);
                }
            }
            catch(Exception e)
            {
                Debug.LogError("Failed to login:" + e.ToString());
                Debug.LogError(e.StackTrace);
                SetLoginStatus("Failed to login.", settings);
            }
        }

        private static void GetUserData(Settings settings)
        {
            UnityWebRequest www = UnityWebRequest.Get(_gaUrl + "user");
            Dictionary<string, string> headers = GA_EditorUtilities.WWWHeadersWithAuthorization(settings.TokenGA);
            foreach (KeyValuePair<string, string> entry in headers)
            {
                www.SetRequestHeader(entry.Key, entry.Value);
            }

            GA_ContinuationManager.StartCoroutine(GetUserDataFrontend(www, settings), () => www.isDone);
        }


        private static IEnumerator GetUserDataFrontend(UnityWebRequest www, Settings settings)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
                IDictionary<string, object> returnParam = null;
                string error = "";

                string text = www.downloadHandler.text;

                if (!string.IsNullOrEmpty(text))
                {
                    returnParam = GA_MiniJSON.Deserialize(text) as IDictionary<string, object>;
                    if (returnParam.ContainsKey("errors"))
                    {
                        IList<object> errorList = returnParam["errors"] as IList<object>;
                        if (errorList != null && errorList.Count > 0)
                        {
                            IDictionary<string, object> errors = errorList[0] as IDictionary<string, object>;
                            if (errors.ContainsKey("msg"))
                            {
                                error = errors["msg"].ToString();
                            }
                        }
                    }
                }

                if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError))
                {
                    if (!String.IsNullOrEmpty(error))
                    {
                        Debug.LogError(error);
                        SetLoginStatus("Failed to get data.", settings);
                    }
                    else if (returnParam != null)
                    {
                        IList<object> resultList = returnParam["results"] as IList<object>;
                        IDictionary<string, object> results = resultList[0] as IDictionary<string, object>;
                        IDictionary<string, object> orgs = results["organizations"] as IDictionary<string, object>;
                        IList<object> studioList = results["studios"] as IList<object>;

                        Dictionary<string, Organization> organizationMap = new Dictionary<string, Organization>();
                        List<Organization> returnOrganizations = new List<Organization>();
                        foreach(KeyValuePair<string, object> pair in orgs)
                        {
                            IDictionary<string, object> organization = pair.Value as IDictionary<string, object>;
                            Organization o = new Organization(organization["name"].ToString(), organization["id"].ToString());
                            returnOrganizations.Add(o);
                            organizationMap.Add(o.ID, o);
                        }

                        for (int s = 0; s < studioList.Count; s++)
                        {
                            IDictionary<string, object> studio = studioList[s] as IDictionary<string, object>;

                            if ((!studio.ContainsKey("demo") || !((bool)studio["demo"])) && (!studio.ContainsKey("archived") || !((bool)studio["archived"])))
                            {
                                List<Game> returnGames = new List<Game>();

                                List<object> gamesList = (List<object>)studio["games"];
                                for (int g = 0; g < gamesList.Count; g++)
                                {
                                    IDictionary<string, object> game = gamesList[g] as IDictionary<string, object>;

                                    if ((!game.ContainsKey("archived") || !((bool)game["archived"])) && (!game.ContainsKey("disabled") || !((bool)game["disabled"])))
                                    {
                                        returnGames.Add(new Game(game["name"].ToString(), int.Parse(game["id"].ToString()), game["key"].ToString(), game["secret"].ToString()));
                                    }
                                }

                                Studio st = new Studio(studio["name"].ToString(), studio["id"].ToString(), studio["org_id"].ToString(), returnGames);
                                organizationMap[st.OrganizationID].Studios.Add(st);
                            }
                        }
                        settings.Organizations = returnOrganizations;

                        if (settings.Organizations.Count == 1 && settings.Organizations[0].Studios.Count == 1)
                        {
                            bool autoSelectedPlatform = false;
                            for (int i = 0; i < settings.Platforms.Count; ++i)
                            {
                                RuntimePlatform platform = settings.Platforms[i];

                                if (platform == settings.LastCreatedGamePlatform)
                                {
                                    SelectOrganization(1, settings, i);
                                    autoSelectedPlatform = true;
                                }
                            }
                            settings.LastCreatedGamePlatform = (RuntimePlatform)(-1);
                            SetLoginStatus(autoSelectedPlatform ? "Received data. Autoselected platform.." : "Received data. Add a platform..", settings);
                        }
                        else
                        {
                            SetLoginStatus("Received data. Add a platform..", settings);
                        }

                        settings.CurrentInspectorState = Settings.InspectorStates.Basic;
                    }
                }
                else if (www.responseCode == 301 || www.responseCode == 404 || www.responseCode == 410)
                {
                    Debug.LogError("Failed to get data. GameAnalytics request not successful. API was changed. Please update your SDK to the latest version: " + www.error + " " + error);
                    SetLoginStatus("Failed to get data. GameAnalytics request not successful. API was changed. Please update your SDK to the latest version.", settings);
                }
                else
                {
                    Debug.LogError("Failed to get user data: " + www.error + " " + error);
                    SetLoginStatus("Failed to get data.", settings);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to get user data: " + e.ToString() + ", " + e.StackTrace);
                SetLoginStatus("Failed to get data.", settings);
            }
        }

        private static void SelectOrganization(int index, Settings settings, int platform)
        {
            settings.SelectedOrganization[platform] = index;
            if (settings.Organizations[index - 1].Studios.Count == 1)
            {
                SelectStudio(1, settings, platform);
            }
            else
            {
                SetLoginStatus("Please select studio..", settings);
            }
        }

        private static void SelectStudio(int index, Settings settings, int platform)
        {
            settings.SelectedStudio[platform] = index;
            if (settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[index - 1].Games.Count == 1)
            {
                if (settings.IsGameKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[0].GameKey) &&
                   settings.IsSecretKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[0].SecretKey))
                {
                    SelectGame(1, settings, platform);
                }
            }
            else
            {
                SetLoginStatus("Please select game..", settings);
            }
        }

        private static void SelectGame(int index, Settings settings, int platform)
        {
            settings.SelectedGame[platform] = index;

            if (index == 0)
            {
                settings.UpdateGameKey(platform, "");
                settings.UpdateSecretKey(platform, "");
            }
            else if (settings.IsGameKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].GameKey) &&
               settings.IsSecretKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].SecretKey))
            {
                settings.SelectedPlatformOrganization[platform] = settings.Organizations[settings.SelectedOrganization[platform] - 1].Name;
                settings.SelectedPlatformStudio[platform] = settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Name;
                settings.SelectedPlatformGame[platform] = settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].Name;
                settings.SelectedPlatformGameID[platform] = settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].ID;
                settings.UpdateGameKey(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].GameKey);
                settings.UpdateSecretKey(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].SecretKey);
                SetLoginStatus("Received keys. Ready to go!", settings);
            }
            else
            {
                if (!settings.IsGameKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].GameKey))
                {
                    Debug.LogError("[GameAnalytics] Game key already exists for another platform. Platforms can't use the same key.");
                    settings.SelectedGame[platform] = 0;
                }
                else if (!settings.IsSecretKeyValid(platform, settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[settings.SelectedStudio[platform] - 1].Games[index - 1].SecretKey))
                {
                    Debug.LogError("[GameAnalytics] Secret key already exists for another platform. Platforms can't use the same key.");
                    settings.SelectedGame[platform] = 0;
                }
            }
        }

        private static void SetLoginStatus(string status, Settings settings)
        {
            settings.LoginStatus = status;
            EditorUtility.SetDirty(settings);
        }

        public static void CheckForUpdates()
        {
            if (Settings.CheckingForUpdates)
            {
                return;
            }

            Settings.CheckingForUpdates = true;

            UnityWebRequest www = UnityWebRequest.Get("https://s3.amazonaws.com/public.gameanalytics.com/sdk_status/current.json");
            GA_ContinuationManager.StartCoroutine(CheckForUpdatesCoroutine(www), () => www.isDone);
        }

        private static void GetChangeLogsAndShowUpdateWindow(string newVersion)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://s3.amazonaws.com/public.gameanalytics.com/sdk_status/change_logs.json");
            GA_ContinuationManager.StartCoroutine(GetChangeLogsAndShowUpdateWindowCoroutine(www, newVersion), () => www.isDone);
        }

        private static IEnumerator CheckForUpdatesCoroutine(UnityWebRequest www)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            bool changeLogRequested = false;

            try
            {
                if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError))
                {
                    string text;
                    text = www.downloadHandler.text;
                    IDictionary<string, object> returnParam = GA_MiniJSON.Deserialize(text) as IDictionary<string, object>;
                    if (returnParam.ContainsKey("unity"))
                    {
                        IDictionary<string, object> unityParam = returnParam["unity"] as IDictionary<string, object>;
                        if (unityParam.ContainsKey("version"))
                        {
                            string newVersion = (returnParam["unity"] as IDictionary<string, object>)["version"].ToString();

                            if (IsNewVersion(newVersion, Settings.VERSION))
                            {
                                changeLogRequested = true;
                                GetChangeLogsAndShowUpdateWindow(newVersion);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (!changeLogRequested)
                {
                    Settings.CheckingForUpdates = false;
                }
            }
        }

        private static IEnumerator GetChangeLogsAndShowUpdateWindowCoroutine(UnityWebRequest www, string newVersion)
        {
            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            try
            {
                if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError))
                {
                    string text;
                    text = www.downloadHandler.text;
                    IDictionary<string, object> returnParam = GA_MiniJSON.Deserialize(text) as IDictionary<string, object>;

                    IList<object> unity = (returnParam["unity"] as IList<object>);
                    string newChanges = "";
                    for (int i = 0; i < unity.Count; i++)
                    {
                        IDictionary<string, object> unityHash = unity[i] as IDictionary<string, object>;
                        IList<object> changes = (unityHash["changes"] as IList<object>);

                        if (unityHash["version"].ToString() == Settings.VERSION)
                        {
                            break;
                        }

                        if (string.IsNullOrEmpty(newChanges))
                        {
                            newChanges = unityHash["version"].ToString();
                        }
                        else
                        {
                            newChanges += "\n\n" + unityHash["version"].ToString();
                        }

                        for (int u = 0; u < changes.Count; u++)
                        {
                            if (string.IsNullOrEmpty(newChanges))
                            {
                                newChanges = "- " + changes[u].ToString();
                            }
                            else
                            {
                                newChanges += "\n- " + changes[u].ToString();
                            }
                        }

                        if (unityHash["version"].ToString() == newVersion)
                        {
                            GA_UpdateWindow.SetNewVersion(newVersion);
                        }
                    }

                    string skippedVersion = EditorPrefs.GetString("ga_skip_version" + "-" + Application.dataPath, "");

                    GA_UpdateWindow.SetChanges(newChanges);
                    if (!skippedVersion.Equals(newVersion))
                    {
                        OpenUpdateWindow();
                    }

                    Settings.CheckingForUpdates = false;
                }
            }
            catch
            {
                Settings.CheckingForUpdates = false;
            }
        }

        private static void OpenUpdateWindow()
        {
            if(!Application.isBatchMode)
            {
                // TODO: possible to close existing window if already there?
                //GA_UpdateWindow updateWindow = ScriptableObject.CreateInstance<GA_UpdateWindow> ();
                GA_UpdateWindow updateWindow = (GA_UpdateWindow)EditorWindow.GetWindow(typeof(GA_UpdateWindow), utility: true);
                updateWindow.position = new Rect(150, 150, 415, 340);
                updateWindow.titleContent = new GUIContent("An update for GameAnalytics is available!");
                updateWindow.Show();
            }
        }

        public static void Splitter(Color rgb, float thickness = 1, int margin = 0)
        {
            GUIStyle splitter = new GUIStyle();
            splitter.normal.background = EditorGUIUtility.whiteTexture;
            splitter.stretchWidth = true;
            splitter.margin = new RectOffset(margin, margin, 7, 7);

            Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

            if(Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = rgb;
                splitter.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        private static string PlatformToString(RuntimePlatform platform)
        {
            string result = platform.ToString();

            if (platform == RuntimePlatform.IPhonePlayer)
            {
                result = "iOS";
            }
            if (platform == RuntimePlatform.tvOS) {
                result = "tvOS";
            }

            return result;
        }

        // versionstring is:
        // [majorVersion].[minorVersion].[patchnumber]
        static bool IsNewVersion(string newVersion, string currentVersion)
        {

            int[] newVersionInts = GetVersionIntegersFromString(newVersion);
            int[] currentVersionInts = GetVersionIntegersFromString(currentVersion);

            if(newVersionInts == null || currentVersionInts == null)
            {
                return false;
            }

            // compare majorVersion
            if(newVersionInts[MAJOR_V] > currentVersionInts[MAJOR_V])
            {
                return true;
            }
            else if(newVersionInts[MAJOR_V] < currentVersionInts[MAJOR_V])
            {
                return false;
            }

            // compare minorVersion (majorVersion is unchanged)
            if(newVersionInts[MINOR_V] > currentVersionInts[MINOR_V])
            {
                return true;
            }
            else if(newVersionInts[MINOR_V] < currentVersionInts[MINOR_V])
            {
                return false;
            }

            // compare patchnumber (majorVersion, minorVersion is unchanged)
            if(newVersionInts[PATCH_V] > currentVersionInts[PATCH_V])
            {
                return true;
            }

            // not valid new version
            return false;
        }

        // version string need to be: x.y.z
        // return validated ints in array or null
        static int[] GetVersionIntegersFromString(string versionString)
        {
            string[] versionNumbers = versionString.Split('.');
            if(versionNumbers.Length != ALL_V)
            {
                return null;
            }

            // container for validated version integers
            int[] validatedVersionNumbers = new int[ALL_V];

            // verify int parsing
            bool isIntMajorVersion = int.TryParse(versionNumbers[MAJOR_V], out validatedVersionNumbers[MAJOR_V]);
            bool isIntMinorVersion = int.TryParse(versionNumbers[MINOR_V], out validatedVersionNumbers[MINOR_V]);
            bool isIntPatchnumber  = int.TryParse(versionNumbers[PATCH_V], out validatedVersionNumbers[PATCH_V]);

            if(isIntMajorVersion && isIntMinorVersion && isIntPatchnumber)
            {
                return validatedVersionNumbers;
            }
            else
            {
                return null;
            }
        }

#region Helper functions

        private static void OpenSignUp()
        {
            Application.OpenURL(_gaSignUpUrl);
        }

        private static void OpenSignUpSwitchToGuideStep()
        {
            GA_SignUp signup = ScriptableObject.CreateInstance<GA_SignUp>();
            signup.maxSize = new Vector2(640, 600);
            signup.minSize = new Vector2(640, 600);
            signup.titleContent = new GUIContent("GameAnalytics - Setup Guide");
            signup.ShowUtility();
            signup.Opened();

            signup.SwitchToGuideStep();
        }

        private static void DrawLinkButton(string text, GUIStyle style, string url, params GUILayoutOption[] options)
        {
            DrawLinkButton(new GUIContent(text), style, url, options);
        }

        private static void DrawLinkButton(GUIContent content, GUIStyle style, string url, params GUILayoutOption[] options)
        {
            Action action = () => Application.OpenURL(url);
            DrawButton(content, style, action, options);
        }

        private static void DrawButton(string text, GUIStyle style, Action action, params GUILayoutOption[] options)
        {
            DrawButton(new GUIContent(text), style, action, options);
        }

        private static void DrawButton(GUIContent content, GUIStyle style, Action action, params GUILayoutOption[] options)
        {
            if(GUILayout.Button(content, style, options))
            {
                action();
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
        }

        private static void DrawButtonWithFlexibleSpace(string text, GUIStyle style, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawButton(text, style, action, options);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawLabelWithFlexibleSpace(string text, GUIStyle style, int height)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(text, style, new GUILayoutOption[] { GUILayout.Height(height) });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

#endregion // Helper functions

#region UIvalidation

		/// <summary>
		/// Check if a string matches a defined pattern
		/// </summary>
		/// <returns><c>true</c>, if match <c>false</c> otherwise.</returns>
		/// <param name="s">Given string</param>
		/// <param name="pattern">Pattern.</param>
		public static bool StringMatch(string s, string pattern)
		{
			if(s == null || pattern == null)
			{
				return false;
			}

			return Regex.IsMatch(s, pattern);
		}

		private string ValidateResourceCurrencyEditor(string currency)
		{
			if (!StringMatch (currency, "^[A-Za-z]+$")) {
				if (currency != null) {
					Debug.LogError ("Validation fail - resource currency: Cannot contain other characters than 'A-Za-z'. String:'" + currency + "'");
				}
				return "Empty";
			}
			if (ConsistsOfWhiteSpace(currency)) {
				return "Empty";
			}
			return currency;
		}

		private string ValidateResourceItemTypeEditor (string itemType)
		{
			if (itemType.Length > 64) {
				Debug.LogError ("Validation fail - resource itemType cannot be longer than 64 chars.");
				return "Empty";
			}
			if (!StringMatch (itemType, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,64}$")) {
				if (itemType != null) {
					Debug.LogError ("Validation fail - resource itemType: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: '" + itemType + "'");
				}
				return "Empty";
			}
			if (ConsistsOfWhiteSpace(itemType)) {
				return "Empty";
			}
			return itemType;
		}

		private string ValidateCustomDimensionEditor(string customDimension)
		{
			if (customDimension.Length > 32) {
				Debug.LogError ("Validation fail - custom dimension cannot be longer than 32 chars.");
				return "Empty";
			}
			if (!StringMatch (customDimension, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,32}$")) {
				if (customDimension != null) {
					Debug.LogError ("Validation fail - custom dimension: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: '" + customDimension + "'");
				}
				return "Empty";
			}
			if (ConsistsOfWhiteSpace(customDimension)) {
				return "Empty";
			}
			return customDimension;
		}

		private bool ConsistsOfWhiteSpace(string s)
		{
			foreach (char c in s) {
				if (c != ' ')
					return false;
			}
			return true;
		}

#endregion
    }
}
