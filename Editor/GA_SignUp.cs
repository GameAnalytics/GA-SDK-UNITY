using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine.Networking;

namespace GameAnalyticsSDK.Editor
{
    public class GA_SignUp : EditorWindow
    {
        public enum TourSteps
        {
            SetupGameKeys = 0,
            AddObject,
            StartTracking,
            TrackBusiness,
            TrackResources,
            TrackProgression,
            TrackDesign,
            LogErrors,
            CustomDimensions,
            FinishGuide,
            AllSteps

        }

        public TourSteps TourStep = TourSteps.SetupGameKeys;

        private bool _showGuidedTour = true;
    
        private Vector2 _appScrollPos;

        private const int INPUT_WIDTH = 230;

        private static GA_SignUp _instance;

        private bool _signUpInProgress = false;
        private bool _createGameInProgress = false;
        private string _googlePlayPublicKey = "";
        private RuntimePlatform _selectedPlatform;
        private int _selectedOrganization;
        private int _selectedStudio;

        private enum StringType
        {
            Label,
            TextBox,
            Link

        }

        private struct StringWithType
        {
            public string Text;
            public StringType Type;
            public string Link;
        }

        public void Opened()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Close();
            }
        }

        void OnDisable()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        void OnGUI()
        {
            if(_showGuidedTour)
            {
                #region guide gui

                    TourSteps guideStep = TourStep;

                    GUILayout.Space(20);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(GameAnalytics.SettingsGA.InstrumentIcon, new GUILayoutOption[] {
                        GUILayout.Width(40),
                        GUILayout.Height(40)
                    });
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Start instrumenting", EditorStyles.whiteLargeLabel);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Let us guide you through getting properly setup with GameAnalytics.");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GA_SettingsInspector.Splitter(new Color(0.35f, 0.35f, 0.35f), 1, 30);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (guideStep == TourSteps.FinishGuide)
                        GUILayout.Label(GetGuideStepTitle(guideStep), EditorStyles.whiteLargeLabel, GUILayout.Width(464));
                    else
                        GUILayout.Label(GetGuideStepTitle(guideStep), EditorStyles.whiteLargeLabel, GUILayout.Width(470));
                    GUILayout.BeginVertical();
                    GUILayout.Space(7);
                    if (guideStep == TourSteps.FinishGuide)
                        GUILayout.Label("STEP " + (guideStep) + " OF 10", GUILayout.Width(87));
                    else
                        GUILayout.Label("STEP " + (guideStep) + " OF 10", GUILayout.Width(80));
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    StringWithType[] guideStepTexts = GetGuideStepText(guideStep);
                    foreach (StringWithType s in guideStepTexts)
                    {
                        if (s.Type == StringType.Label)
                        {
                            GUILayout.Label(s.Text, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(550));
                        }
                        else if (s.Type == StringType.TextBox)
                        {
                            TextAnchor tmpA = GUI.skin.textField.alignment;
                            int tmpFS = GUI.skin.textField.fontSize;
                            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
                            GUI.skin.textField.fontSize = 12;
                            GUI.skin.textField.padding = new RectOffset(10, 1, 10, 10);
                            GUILayout.TextField(s.Text, new GUILayoutOption[] {
                                GUILayout.MaxWidth(550),
                                GUILayout.Height(34)
                            });
                            GUI.skin.textField.alignment = tmpA;
                            GUI.skin.textField.fontSize = tmpFS;
                            GUI.skin.textField.padding = new RectOffset(3, 3, 1, 2);
                        }
                        else if (s.Type == StringType.Link)
                        {
                            GUI.skin.label.fontStyle = FontStyle.Bold;
                            float sl = GUI.skin.button.CalcSize(new GUIContent(s.Text)).x;
                            if (GUILayout.Button(s.Text, EditorStyles.whiteLabel, GUILayout.MaxWidth(sl)))
                            {
                                Application.OpenURL(s.Link);
                            }
                            GUI.skin.label.fontStyle = FontStyle.Normal;

                            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    GA_SettingsInspector.Splitter(new Color(0.35f, 0.35f, 0.35f), 1, 30);

                    // create game button

                    string buttonText = "Next step";
                    if (TourStep == TourSteps.FinishGuide)
                    {
                        buttonText = "Done";
                    }

                    GUI.BeginGroup(new Rect(0, 420, 640, 50));
                    //GUILayout.BeginHorizontal();
                    //GUILayout.FlexibleSpace();
                    //GUILayout.BeginVertical();
                    //GUILayout.Space(7);
                    GUI.Label(new Rect(43, 7, 500, 50), GetGuideStepNext(guideStep));
                    //GUILayout.EndVertical();
                    if (guideStep > TourSteps.SetupGameKeys && GUI.Button(new Rect(454, 0, 30, 30), "<"))
                    {
                        TourStep--;
                    }
                    if (GUI.Button(new Rect(489, 0, 100, 30), buttonText))
                    {
                        if (TourStep < TourSteps.FinishGuide)
                            TourStep++;
                        else
                            Close();
                    }
                    //GUILayout.FlexibleSpace();
                    //GUILayout.EndHorizontal();
                    GUI.EndGroup();

                #endregion // guide
            }
        }

        public void SwitchToGuideStep()
        {
            TourStep = TourSteps.SetupGameKeys;
        }

        private string GetGuideStepTitle(TourSteps step)
        {
            switch (step)
            {
                case TourSteps.SetupGameKeys:
                    return "1. SETUP GAME KEYS";
                case TourSteps.AddObject:
                    return "2. ADD GAMEANALYTICS OBJECT";
                case TourSteps.StartTracking:
                    return "3. START TRACKING EVENTS";
                case TourSteps.TrackBusiness:
                    return "4. TRACK REAL MONEY TRANSACTIONS";
                case TourSteps.TrackResources:
                    return "5. BALANCE VIRTUAL ECONOMY";
                case TourSteps.TrackProgression:
                    return "6. TRACK PLAYER PROGRESSION";
                case TourSteps.TrackDesign:
                    return "7. USE CUSTOM DESIGN EVENTS";
                case TourSteps.LogErrors:
                    return "8. LOG ERROR EVENTS";
                case TourSteps.CustomDimensions:
                    return "9. USE CUSTOM DIMENSIONS";
                case TourSteps.FinishGuide:
                    return "10. BUILD AND COMPLETE INTEGRATION";
                default:
                    return "-";
            }
        }

        private StringWithType[] GetGuideStepText(TourSteps step)
        {
            switch (step)
            {
                case TourSteps.SetupGameKeys:
                    return new StringWithType[] {
                        new StringWithType { Text = "The unique game and secret key are used to authenticate your game. If you're logged into GameAnalytics, in Settings under the Account tab, choose your studio and game to sync your keys with your Unity project. If you don't have an account, visit \"https://tool.gameanalytics.com/signup\" to create your account and game." },
                        new StringWithType { Text = "You can also input your keys manually in Settings under the Setup tab. The keys can always be found under Game Settings in the webtool." },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to create an account.",
                            Type = StringType.Link,
                            Link = "https://tool.gameanalytics.com/signup"
                        },
                    };
                case TourSteps.AddObject:
                    return new StringWithType[] {
                        new StringWithType { Text = "To use GameAnalytics you need to add the GameAnalytics object to your starting scene. To add this object go to Window/GameAnalytics/Create GameAnalytics Object." },
                        new StringWithType { Text = "Now you're set up to start tracking data - it's that easy! Out of the box GameAnalytics will give you access to lots of core metrics, such as daily active users (DAU), without implementing any custom events." },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the full list of core metrics and dimensions.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/metrics-dimensions"
                        }
                    };
                case TourSteps.StartTracking:
                    return new StringWithType[] {
                        new StringWithType { Text = "GameAnalytics supports 5 different types of events: Business, Resource, Progression, Error and Design." },
                        new StringWithType { Text = "To send an event, remember to include the namespace GameAnalyticsSDK:" },
                        new StringWithType {
                            Text = "using GameAnalyticsSDK;",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType { Text = "The next steps will guide you through the instrumentation of each of the different event types." },
                    };
                case TourSteps.TrackBusiness:
                    return new StringWithType[] {
                        new StringWithType { Text = "With the Business event, you can include information on the specific type of in-app item purchased, and where in the game the purchase was made. Additionally, the GameAnalytics SDK captures the app store receipt to validate the purchases." },
                        new StringWithType { Text = "To add a business event call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.NewBusinessEvent (string currency, int amount, string itemType, string itemId, string cartType, string receipt, bool autoFetchReceipt);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the Business event and purchase validation.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/event-tracking#business-events"
                        }
                    };
                case TourSteps.TrackResources:
                    return new StringWithType[] {
                        new StringWithType { Text = "Resources events are used to track your in-game economy. From setting up the event you will be able to see three types of events in the tool. Flow, the total balance from currency spent and rewarded. Sink is all currency spent on items, and lastly source, being all currency rewarded in game." },
                        new StringWithType { Text = "To add a resource event call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.NewResourceEvent (GA_Resource.GAResourceFlowType flowType, string resourceType, float amount, string itemType, string itemId);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "Please note that any Resource Currencies and Resource Item Types you want to use must first be defined in Settings, under the Setup tab. Any value which is not defined will be ignored." },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the Resource event.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/event-tracking#resource-events"
                        }
                    };
                case TourSteps.TrackProgression:
                    return new StringWithType[] {
                        new StringWithType { Text = "Use this event to track when players start and finish levels in your game. This event follows a 3 tier hierarchy structure (World, Level and Phase) to indicate a player's path or place in the game." },
                        new StringWithType { Text = "To add a progression event call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the Progression event.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/event-tracking#progression-events"
                        }
                    };
                case TourSteps.TrackDesign:
                    return new StringWithType[] {
                        new StringWithType { Text = "Track any other concept in your game using this event type. For example, you could use this event to track GUI elements or tutorial steps. Custom dimensions are not supported on this event type." },
                        new StringWithType { Text = "To add a design event call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.NewDesignEvent (string eventName, float eventValue);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the Design event.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/event-tracking#design-events"
                        }
                    };
                case TourSteps.LogErrors:
                    return new StringWithType[] {
                        new StringWithType { Text = "You can use the Error event to log errors or warnings that players generate in your game. You can group the events by severity level and attach a message, such as the stack trace." },
                        new StringWithType { Text = "To add a custom error event call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.NewErrorEvent (GA_Error.GAErrorSeverity severity, string message);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about the Error event.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/event-tracking#error-events"
                        }
                    };
                case TourSteps.CustomDimensions:
                    return new StringWithType[] {
                        new StringWithType { Text = "Custom Dimensions can be used to filter your data in the GameAnalytics webtool. To add custom dimensions to your events you will first have to create a list of all the allowed values. You can do this in Settings under the Setup tab. Any value which is not defined will be ignored." },
                        new StringWithType { Text = "For example, to set Custom Dimension 01, call the following function:" },
                        new StringWithType {
                            Text = "GameAnalytics.SetCustomDimension01(string customDimension);",
                            Type = StringType.TextBox
                        },
                        new StringWithType { Text = "" },
                        new StringWithType {
                            Text = "Click here to learn more about Custom Dimensions.",
                            Type = StringType.Link,
                            Link = "https://docs.gameanalytics.com/integrations/sdk/unity/advanced-setup#custom-dimensions"
                        }
                    };
                case TourSteps.FinishGuide:
                    return new StringWithType[] {
                        new StringWithType { Text = "You're almost there! To complete the integration and start sending data to GameAnalytics, all you need to do is build and run your game." },
                    #if UNITY_IOS || UNITY_TVOS || UNITY_ANDROID
                        new StringWithType {Text = "The link below describes the important last steps you need to complete to build for the build platform you selected in the editor."},
                    #endif
                        new StringWithType { Text = "" },

                    #if UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE || UNITY_TIZEN || UNITY_WEBGL || UNITY_WINRT

                    new StringWithType {
                    Text = "Click here to check online documentation!",
                    Type = StringType.Link,
                    Link = "https://docs.gameanalytics.com/integrations/sdk/unity/"
                    }

                    #else

                    new StringWithType { Text = "Your selected build platform is not currently supported by GameAnalytics." },
                    new StringWithType { Text = "The Unity SDK includes support for Windows, Mac, Linux, WebGL, iOS, tvOS, UWP, Tizen, Universal Windows 8 and Android.", Type = StringType.Link, Link = "https://docs.gameanalytics.com/integrations/sdk/unity" },

                    #endif
                    };
                default:
                    return new StringWithType[] {
                        new StringWithType { Text = "-" }
                    };
            }
        }

        private string GetGuideStepNext(TourSteps step)
        {
            TourSteps next = step + 1;
            switch (next)
            {
                case TourSteps.AddObject:
                    return "In the next step we look at how to add the GameAnalytics object.";
                case TourSteps.StartTracking:
                    return "In the next step we look at how to start tracking events.";
                case TourSteps.TrackBusiness:
                    return "In the next step we look at how to track real money transactions.";
                case TourSteps.TrackResources:
                    return "In the next step we look at how to balance your virtual economy.";
                case TourSteps.TrackProgression:
                    return "In the next step we look at how to track player progression.";
                case TourSteps.TrackDesign:
                    return "In the next step we look at how to use custom design events.";
                case TourSteps.LogErrors:
                    return "In the next step we look at how to log error events.";
                case TourSteps.CustomDimensions:
                    return "In the next step we look at how to use custom dimensions.";
                case TourSteps.FinishGuide:
                    return "In the last step we look at completing the integration.";
                case TourSteps.AllSteps:
                    return "Thank you for choosing GameAnalytics!";
                default:
                    return "-";
            }
        }

        private void PaintAppStoreIcon(string storeName)
        {
            switch (storeName)
            {
                case "amazon_appstore":
                    if (GameAnalytics.SettingsGA.AmazonIcon != null)
                    {
                        //GUILayout.Label("", GUILayout.Height(-20));
                        GUILayout.Label(GameAnalytics.SettingsGA.AmazonIcon, new GUILayoutOption[] {
                            GUILayout.Width(20),
                            GUILayout.MaxHeight(20)
                        });
                    }

                    GUILayout.Label("Amazon", GUILayout.Width(80));
                    break;
                case "google_play":
                    if (GameAnalytics.SettingsGA.GooglePlayIcon != null)
                    {
                        //GUILayout.Label("", GUILayout.Height(-20));
                        GUILayout.Label(GameAnalytics.SettingsGA.GooglePlayIcon, new GUILayoutOption[] {
                            GUILayout.Width(20),
                            GUILayout.MaxHeight(20)
                        });
                    }

                    GUILayout.Label("Google Play", GUILayout.Width(80));
                    break;
                case "apple":
                    if (GameAnalytics.SettingsGA.iosIcon != null)
                    {
                        //GUILayout.Label("", GUILayout.Height(-20));
                        GUILayout.Label(GameAnalytics.SettingsGA.iosIcon, new GUILayoutOption[] {
                            GUILayout.Width(20),
                            GUILayout.MaxHeight(20)
                        });
                    }

                    GUILayout.Label("iOS", GUILayout.Width(80));
                    break;
                case "apple:mac":
                    if (GameAnalytics.SettingsGA.macIcon != null)
                    {
                        //GUILayout.Label("", GUILayout.Height(-20));
                        GUILayout.Label(GameAnalytics.SettingsGA.macIcon, new GUILayoutOption[] {
                            GUILayout.Width(20),
                            GUILayout.MaxHeight(20)
                        });
                    }

                    GUILayout.Label("Mac", GUILayout.Width(80));
                    break;
                case "windows_phone":
                    if (GameAnalytics.SettingsGA.windowsPhoneIcon != null)
                    {
                        //GUILayout.Label("", GUILayout.Height(-20));
                        GUILayout.Label(GameAnalytics.SettingsGA.windowsPhoneIcon, new GUILayoutOption[] {
                            GUILayout.Width(20),
                            GUILayout.MaxHeight(20)
                        });
                    }

                    GUILayout.Label("Win. Phone", GUILayout.Width(80));
                    break;
                default:
                    GUILayout.Label(storeName, GUILayout.Width(100));
                    break;
            }
        }

        public IEnumerator GetAppStoreIconTexture(UnityWebRequest www, string storeName, GA_SignUp signup)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
#if UNITY_2020_1_OR_NEWER
                if (!(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError))
#elif UNITY_2017_1_OR_NEWER
                if (!(www.isNetworkError || www.isHttpError))
#else
                if (string.IsNullOrEmpty(www.error))
#endif
                {
                    switch (storeName)
                    {
                        case "amazon_appstore":
#if UNITY_2017_1_OR_NEWER
                            GameAnalytics.SettingsGA.AmazonIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
#else
                            GameAnalytics.SettingsGA.AmazonIcon = www.texture;
#endif
                            break;
                        case "google_play":
#if UNITY_2017_1_OR_NEWER
                            GameAnalytics.SettingsGA.GooglePlayIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
#else
                            GameAnalytics.SettingsGA.GooglePlayIcon = www.texture;
#endif
                            break;
                        case "apple:ios":
#if UNITY_2017_1_OR_NEWER
                            GameAnalytics.SettingsGA.iosIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
#else
                            GameAnalytics.SettingsGA.iosIcon = www.texture;
#endif
                            break;
                        case "apple:mac":
#if UNITY_2017_1_OR_NEWER
                            GameAnalytics.SettingsGA.macIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
#else
                            GameAnalytics.SettingsGA.macIcon = www.texture;
#endif
                            break;
                        case "windows_phone":
#if UNITY_2017_1_OR_NEWER
                            GameAnalytics.SettingsGA.windowsPhoneIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
#else
                            GameAnalytics.SettingsGA.windowsPhoneIcon = www.texture;
#endif
                            break;
                    }
                    signup.Repaint();
                }
            }
            catch
            {
            }
        }
    }
}
