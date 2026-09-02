using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;
using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Setup;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace GameAnalyticsSDK.Editor
{
    /// <summary>
    /// Custom inspector for the GameAnalytics Settings asset (UI Toolkit).
    /// UI state (tab, foldouts, intro dismissal) lives in EditorPrefs so browsing
    /// the inspector never dirties Settings.asset.
    /// </summary>
    [CustomEditor(typeof(Settings))]
    public class GA_SettingsInspector : UnityEditor.Editor
    {
        private const string GaUrl = "https://platform.gameanalytics.com/ext/v1/";
        private const string GaToolUrl = "https://tool.gameanalytics.com";
        private const string GaForgotPasswordUrl = GaToolUrl + "/forgot-password";
        private const string GaSettingsUrl = GaToolUrl + "/game/{0}/settings/general";
        private const string GaOverviewUrl = GaToolUrl + "/game/{0}/overview";
        private const string GaLoginUrl = GaToolUrl + "/login?";
        private const string GaSignUpUrl = GaToolUrl + "/signup";
        private const string GaSupportUrl = "https://www.gameanalytics.com/contact";
        private const string GaIssuesUrl = "https://github.com/GameAnalytics/GA-SDK-UNITY/issues";
        private const string GaDocsUrl = "https://docs.gameanalytics.com/event-tracking-and-integrations/sdks-and-collection-api/game-engine-sdks/unity";
        private const string GaConfigurationDocsUrl = GaDocsUrl + "/configuration";
        private const string GaLoggingDocsUrl = GaDocsUrl + "/debug";
        private const string GaHealthDocsUrl = GaDocsUrl + "/health";

        private const string NotLoggedInStatus = "Not logged in.";

        private const int MaxNumberOfDimensions = 20;

        private enum Tab
        {
            Account = 0,
            Setup = 1,
            Events = 2,
            Advanced = 3
        }

        private Settings settings;

        private VisualElement _root;
        private VisualElement _header;
        private VisualElement _body;
        private Texture2D _logoTexture;
        private int _addPlatformIndex;

        // Snapshot used by the scheduled poll to notice async state changes
        // (login coroutines, update check) and refresh the UI.
        private string _lastLoginStatus;
        private bool _lastLoggedIn;
        private string _lastNewVersion;

        private static bool _checkedProjectNames;

        #region EditorPrefs-backed UI state

        private static string PrefKey(string name)
        {
            return "GA.Inspector." + name + "-" + Application.dataPath;
        }

        private Tab CurrentTab
        {
            get
            {
                Tab tab = (Tab)EditorPrefs.GetInt(PrefKey("Tab"), (int)Tab.Setup);
                if (tab == Tab.Account && IsLoggedIn)
                {
                    tab = Tab.Setup;
                }
                if (tab < Tab.Account || tab > Tab.Advanced)
                {
                    tab = Tab.Setup;
                }
                return tab;
            }
            set { EditorPrefs.SetInt(PrefKey("Tab"), (int)value); }
        }

        private bool IntroDismissed
        {
            get { return EditorPrefs.GetBool(PrefKey("IntroDismissed"), false); }
            set { EditorPrefs.SetBool(PrefKey("IntroDismissed"), value); }
        }

        private bool GetPlatformFold(RuntimePlatform platform)
        {
            return EditorPrefs.GetBool(PrefKey("PlatformFold." + platform), platform == ActiveBuildRuntimePlatform());
        }

        private void SetPlatformFold(RuntimePlatform platform, bool open)
        {
            EditorPrefs.SetBool(PrefKey("PlatformFold." + platform), open);
        }

        private bool GetListFold(string listName)
        {
            return EditorPrefs.GetBool(PrefKey("Fold." + listName), false);
        }

        private void SetListFold(string listName, bool open)
        {
            EditorPrefs.SetBool(PrefKey("Fold." + listName), open);
        }

        #endregion

        private bool IsLoggedIn
        {
            get { return settings.Organizations != null; }
        }

        void OnEnable()
        {
            settings = (Settings)target;

            // GA_SignUp still renders this icon from the shared Settings object.
            if (settings.InstrumentIcon == null)
            {
                settings.InstrumentIcon = GA_EditorPaths.LoadTexture("Images/instrument.png");
            }
            if (settings.Logo == null)
            {
                settings.Logo = GA_EditorPaths.LoadTexture("gaLogo.png");
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            settings = (Settings)target;
            settings.EnsureBuildNumberAutoDetectList();

            _logoTexture = GA_EditorPaths.LoadTexture("gaLogoIcon.png");
            if (_logoTexture == null)
            {
                _logoTexture = GA_EditorPaths.LoadTexture("gaLogo.png");
            }

            _root = new VisualElement();
            _root.AddToClassList("ga-root");
            StyleSheet styleSheet = GA_EditorPaths.LoadStyleSheet("GA_SettingsInspector.uss");
            if (styleSheet != null)
            {
                _root.styleSheets.Add(styleSheet);
            }

            _header = new VisualElement();
            _body = new VisualElement();
            _root.Add(_header);
            _root.Add(_body);

            Rebuild();
            _root.schedule.Execute(RefreshIfStateChanged).Every(300);

            GA_UpdateChecker.CheckForUpdates();

            return _root;
        }

        private void Rebuild()
        {
            TakeStateSnapshot();

            _header.Clear();
            _body.Clear();

            BuildHeader(_header);

            if (ShowIntro())
            {
                PrefillStudioAndGameName();
                BuildIntroScreen(_body);
            }
            else
            {
                BuildTabs(_body);
                switch (CurrentTab)
                {
                    case Tab.Account: BuildAccountTab(_body); break;
                    case Tab.Setup: BuildSetupTab(_body); break;
                    case Tab.Events: BuildEventsTab(_body); break;
                    case Tab.Advanced: BuildAdvancedTab(_body); break;
                }
            }

            BuildFooter(_body);
        }

        private void TakeStateSnapshot()
        {
            _lastLoginStatus = settings.LoginStatus;
            _lastLoggedIn = IsLoggedIn;
            _lastNewVersion = settings.NewVersion;
        }

        private void RefreshIfStateChanged()
        {
            if (settings == null)
            {
                return;
            }

            bool loggedInChanged = _lastLoggedIn != IsLoggedIn;
            bool changed = loggedInChanged
                || _lastLoginStatus != settings.LoginStatus
                || _lastNewVersion != settings.NewVersion;

            if (!changed)
            {
                return;
            }

            if (loggedInChanged && IsLoggedIn)
            {
                CurrentTab = Tab.Setup;
            }

            Rebuild();
        }

        private bool ShowIntro()
        {
            if (IntroDismissed || IsLoggedIn)
            {
                return false;
            }
            for (int i = 0; i < settings.Platforms.Count; ++i)
            {
                if (settings.GetGameKey(i).Length > 0 || settings.GetSecretKey(i).Length > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Records an undoable change to the settings asset and refreshes the UI.</summary>
        private void Apply(string undoName, Action mutate, bool rebuild = true)
        {
            Undo.RecordObject(settings, undoName);
            mutate();
            EditorUtility.SetDirty(settings);
            if (rebuild)
            {
                Rebuild();
            }
        }

        #region Header

        private void BuildHeader(VisualElement parent)
        {
            VisualElement titleRow = new VisualElement();
            titleRow.AddToClassList("ga-header-row");

            if (_logoTexture != null)
            {
                Image logo = new Image { image = _logoTexture, scaleMode = ScaleMode.ScaleToFit };
                logo.AddToClassList("ga-logo");
                titleRow.Add(logo);
            }

            Label title = new Label("GameAnalytics SDK");
            title.AddToClassList("ga-title");
            titleRow.Add(title);

            if (GA_UpdateChecker.IsUpdateAvailable(settings))
            {
                Button badge = new Button(GA_UpdateWindow.Open)
                {
                    text = "v " + Settings.VERSION + " → " + settings.NewVersion,
                    tooltip = settings.NewVersion + " is available - open the update window."
                };
                badge.AddToClassList("ga-badge");
                badge.AddToClassList("ga-badge--update");
                titleRow.Add(badge);
            }
            else
            {
                Label badge = new Label("v " + Settings.VERSION);
                badge.AddToClassList("ga-badge");
                badge.tooltip = "Installed SDK version.";
                titleRow.Add(badge);
            }

            parent.Add(titleRow);

            VisualElement chipsRow = new VisualElement();
            chipsRow.AddToClassList("ga-chips-row");

            if (ShowIntro())
            {
                chipsRow.Add(MakeChip("Not configured", ok: false, neutral: true));
            }
            else if (settings.Platforms.Count == 0)
            {
                chipsRow.Add(MakeChip("No platforms configured", ok: false));
            }
            else
            {
                for (int i = 0; i < settings.Platforms.Count; ++i)
                {
                    bool configured = settings.GetGameKey(i).Length > 0 && settings.GetSecretKey(i).Length > 0;
                    string name = PlatformToString(settings.Platforms[i]);
                    chipsRow.Add(MakeChip(configured ? name : name + " · keys missing", configured));
                }
            }

            if (IsLoggedIn)
            {
                VisualElement account = new VisualElement();
                account.AddToClassList("ga-account-row");
                Label email = new Label(settings.EmailGA);
                email.AddToClassList("ga-dim");
                email.AddToClassList("ga-account-email");
                email.style.marginRight = 6;
                account.Add(email);
                Button logout = new Button(() =>
                {
                    settings.Organizations = null;
                    SetLoginStatus(NotLoggedInStatus, settings);
                    Rebuild();
                })
                { text = "Log out" };
                account.Add(logout);
                chipsRow.Add(account);
            }

            parent.Add(chipsRow);

            VisualElement separator = new VisualElement();
            separator.AddToClassList("ga-header-separator");
            parent.Add(separator);
        }

        private static VisualElement MakeChip(string text, bool ok, bool neutral = false)
        {
            Label chip = new Label(text);
            chip.AddToClassList("ga-chip");
            if (!neutral)
            {
                chip.AddToClassList(ok ? "ga-chip--ok" : "ga-chip--warn");
            }
            return chip;
        }

        #endregion // Header

        #region Intro screen

        private void BuildIntroScreen(VisualElement parent)
        {
            VisualElement intro = new VisualElement();
            intro.AddToClassList("ga-intro");

            Label title = new Label("Track your game for free");
            title.AddToClassList("ga-intro-title");
            intro.Add(title);

            Label sub = new Label("Sessions, retention, monetization and health metrics for 100k+ games.");
            sub.AddToClassList("ga-intro-sub");
            intro.Add(sub);

            Button signUp = new Button(OpenSignUp)
            {
                text = "Create free account",
                tooltip = "Opens the GameAnalytics signup page."
            };
            signUp.AddToClassList("ga-button-primary");
            signUp.style.marginTop = 12;
            signUp.style.paddingLeft = 24;
            signUp.style.paddingRight = 24;
            signUp.style.height = 30;
            intro.Add(signUp);

            intro.Add(MakeDivider("Already have an account?"));

            intro.Add(BuildLoginForm());

            intro.Add(MakeDivider(null));

            Button manual = MakeLink("Skip - enter game keys manually", () =>
            {
                IntroDismissed = true;
                CurrentTab = Tab.Setup;
                Rebuild();
            }, "Fill in your game and secret keys yourself under Setup.");
            intro.Add(manual);

            parent.Add(intro);
        }

        private static VisualElement MakeDivider(string text)
        {
            VisualElement divider = new VisualElement();
            divider.AddToClassList("ga-divider");
            VisualElement left = new VisualElement();
            left.AddToClassList("ga-divider-line");
            divider.Add(left);
            if (!string.IsNullOrEmpty(text))
            {
                Label label = new Label(text.ToUpperInvariant());
                label.AddToClassList("ga-divider-text");
                divider.Add(label);
                VisualElement right = new VisualElement();
                right.AddToClassList("ga-divider-line");
                divider.Add(right);
            }
            return divider;
        }

        private void PrefillStudioAndGameName()
        {
            if (_checkedProjectNames || EditorPrefs.GetBool("GA_Installed" + "-" + Application.dataPath, false))
            {
                return;
            }
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
        }

        #endregion // Intro screen

        #region Tabs

        private void BuildTabs(VisualElement parent)
        {
            VisualElement tabs = new VisualElement();
            tabs.AddToClassList("ga-tabs");

            if (!IsLoggedIn)
            {
                tabs.Add(MakeTabButton("Account", Tab.Account, "Log in to fetch your game keys automatically."));
            }
            tabs.Add(MakeTabButton("Setup", Tab.Setup, "Per-platform game keys and build numbers."));
            tabs.Add(MakeTabButton("Events", Tab.Events, "Custom dimensions and resource types."));
            tabs.Add(MakeTabButton("Advanced", Tab.Advanced, "Session handling, logging, health tracking and diagnostics."));

            parent.Add(tabs);
        }

        private Button MakeTabButton(string label, Tab tab, string tooltip)
        {
            Button button = new Button(() =>
            {
                CurrentTab = tab;
                Rebuild();
            })
            { text = label, tooltip = tooltip };
            button.AddToClassList("ga-tab");
            if (CurrentTab == tab)
            {
                button.AddToClassList("ga-tab--active");
            }
            return button;
        }

        #endregion // Tabs

        #region Account tab

        private void BuildAccountTab(VisualElement parent)
        {
            VisualElement body;
            parent.Add(MakeCard("Sign in to GameAnalytics", null, null, out body));

            Label note = new Label("Signing in fetches your organizations, studios and games so keys fill themselves in.");
            note.AddToClassList("ga-note");
            body.Add(note);

            BuildLoginStatusRows(body);

            if (IsLoggedIn)
            {
                return;
            }

            body.Add(BuildLoginForm());

            body.Add(MakeDivider(null));

            VisualElement links = new VisualElement();
            links.AddToClassList("ga-links-row");
            links.AddToClassList("ga-links-row--split");
            links.Add(MakeLink("New here? Create a free account ↗", OpenSignUp, "Opens the GameAnalytics signup page."));
            links.Add(MakeLink("Enter game keys manually →", () =>
            {
                CurrentTab = Tab.Setup;
                Rebuild();
            }, "Fill in your game and secret keys yourself under Setup."));
            body.Add(links);
        }

        private VisualElement BuildLoginForm()
        {
            VisualElement form = new VisualElement();
            form.AddToClassList("ga-login-form");

            TextField email = new TextField("Email")
            {
                value = settings.EmailGA,
                tooltip = "Your GameAnalytics user account email."
            };
            email.RegisterValueChangedCallback(evt => { settings.EmailGA = evt.newValue; EditorUtility.SetDirty(settings); });
            form.Add(email);

            TextField password = new TextField("Password")
            {
                value = settings.PasswordGA,
                isPasswordField = true,
                tooltip = "Your GameAnalytics user account password. Press Enter to sign in."
            };
            password.RegisterValueChangedCallback(evt => settings.PasswordGA = evt.newValue);
            form.Add(password);

            EventCallback<KeyDownEvent> submitOnEnter = evt =>
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    evt.StopPropagation();
                    DoLogin();
                }
            };
            email.RegisterCallback(submitOnEnter);
            password.RegisterCallback(submitOnEnter);

            VisualElement buttonRow = new VisualElement();
            buttonRow.style.flexDirection = FlexDirection.Row;
            buttonRow.style.flexWrap = Wrap.Wrap;
            buttonRow.style.justifyContent = Justify.FlexEnd;
            buttonRow.style.alignItems = Align.Center;
            buttonRow.style.marginTop = 6;

            Button forgot = MakeLink("Forgot password?", () => Application.OpenURL(GaForgotPasswordUrl), "Opens the password reset page.");
            forgot.style.marginRight = 8;
            buttonRow.Add(forgot);

            Button login = new Button(DoLogin) { text = "Sign in" };
            login.AddToClassList("ga-button-primary");
            login.style.paddingLeft = 18;
            login.style.paddingRight = 18;
            buttonRow.Add(login);

            form.Add(buttonRow);
            return form;
        }

        private void DoLogin()
        {
            IntroDismissed = true;
            CurrentTab = Tab.Account;
            StartLogin();
            Rebuild();
        }

        private void StartLogin()
        {
            settings.Organizations = null;
            SetLoginStatus("Contacting Server..", settings);
            LoginUser(settings);
        }

        private void BuildLoginStatusRows(VisualElement parent)
        {
            if (string.IsNullOrEmpty(settings.LoginStatus) || settings.LoginStatus.Equals(NotLoggedInStatus))
            {
                return;
            }

            if (settings.JustSignedUp && !settings.HideSignupWarning)
            {
                VisualElement row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.alignItems = Align.FlexStart;
                HelpBox notice = new HelpBox(
                    "Please be aware that our service might take a few minutes to get ready to receive events. Open Integration Status to follow the progress as you start sending events.",
                    HelpBoxMessageType.Warning);
                notice.style.flexGrow = 1;
                row.Add(notice);
                Button dismiss = new Button(() => { settings.HideSignupWarning = true; Rebuild(); }) { text = "X" };
                row.Add(dismiss);
                parent.Add(row);
            }

            VisualElement statusRow = new VisualElement();
            statusRow.AddToClassList("ga-row");
            Label label = new Label("Status");
            label.AddToClassList("ga-field-label");
            statusRow.Add(label);
            statusRow.Add(new Label(settings.LoginStatus));
            parent.Add(statusRow);
        }

        #endregion // Account tab

        #region Setup tab

        private void BuildSetupTab(VisualElement parent)
        {
            SyncPlatformOrganizationList();
            settings.EnsureBuildNumberAutoDetectList();

            VisualElement statusHost = new VisualElement();
            statusHost.style.marginTop = 4;
            BuildLoginStatusRows(statusHost);
            if (statusHost.childCount > 0)
            {
                parent.Add(statusHost);
            }

            for (int i = 0; i < settings.Platforms.Count; ++i)
            {
                parent.Add(BuildPlatformCard(i));
            }

            BuildAddPlatformRow(parent);

#if !(UNITY_IOS || UNITY_TVOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL)
            HelpBox unsupported = new HelpBox(
                "PLEASE NOTICE: Currently the GameAnalytics Unity SDK does not support your selected build Platform. Please refer to the GameAnalytics documentation for additional information.",
                HelpBoxMessageType.Warning);
            unsupported.style.marginTop = 8;
            unsupported.RegisterCallback<ClickEvent>(_ => Application.OpenURL(GaDocsUrl));
            parent.Add(unsupported);
#endif
        }

        private VisualElement BuildPlatformCard(int i)
        {
            RuntimePlatform platform = settings.Platforms[i];
            bool open = GetPlatformFold(platform);
            bool configured = settings.GetGameKey(i).Length > 0 && settings.GetSecretKey(i).Length > 0;

            VisualElement card = new VisualElement();
            card.AddToClassList("ga-card");

            VisualElement header = new VisualElement();
            header.AddToClassList("ga-card-header");

            Button arrow = new Button(() =>
            {
                SetPlatformFold(platform, !open);
                Rebuild();
            })
            { text = open ? "▼" : "▶", tooltip = open ? "Collapse" : "Expand" };
            arrow.AddToClassList("ga-fold-arrow");
            header.Add(arrow);

            Label title = new Label(PlatformToString(platform));
            title.AddToClassList("ga-card-title");
            header.Add(title);

            // The chip is anchored to the right edge of the card header. No per-platform
            // help button: it would duplicate the Documentation link in the footer.
            VisualElement chip = MakeChip(configured ? "configured" : "keys missing", configured);
            chip.style.marginLeft = StyleKeyword.Auto;
            chip.style.marginRight = 0;
            header.Add(chip);

            // Clicking the header (not its buttons) also toggles the foldout.
            header.RegisterCallback<ClickEvent>(evt =>
            {
                if (evt.target == header || evt.target == title)
                {
                    SetPlatformFold(platform, !open);
                    Rebuild();
                }
            });

            card.Add(header);

            if (!open)
            {
                return card;
            }

            VisualElement body = new VisualElement();
            body.AddToClassList("ga-card-body");

            bool loggedIn = settings.Organizations != null && settings.Organizations.Count > 0 && i < settings.SelectedOrganization.Count;
            if (loggedIn)
            {
                BuildGameSelectionPopups(body, i);
            }
            else
            {
                BuildSelectedGameSummary(body, i);
            }

            body.Add(BuildKeyRow(i, "Game key", "Your GameAnalytics Game Key - copy/paste from the GA website.", isSecret: false));
            body.Add(BuildKeyRow(i, "Secret key", "Your GameAnalytics Secret Key - copy/paste from the GA website.", isSecret: true));

            BuildBuildNumberRows(body, i);

            VisualElement footer = new VisualElement();
            footer.style.flexDirection = FlexDirection.Row;
            footer.style.flexWrap = Wrap.Wrap;
            footer.style.justifyContent = Justify.SpaceBetween;
            footer.style.alignItems = Align.Center;
            footer.style.marginTop = 6;

            VisualElement links = new VisualElement();
            links.AddToClassList("ga-links-row");
            links.style.flexShrink = 1;
            if (settings.SelectedPlatformGameID[i] >= 0)
            {
                int gameId = settings.SelectedPlatformGameID[i];
                links.Add(MakeLink("Integration status ↗", () => OpenGamePage(GaOverviewUrl, gameId),
                    "Opens this game's realtime integration status on the dashboard."));
                links.Add(MakeLink("Game settings ↗", () => OpenGamePage(GaSettingsUrl, gameId),
                    "Opens this game's settings on the dashboard."));
            }
            footer.Add(links);

            int index = i;
            Button remove = new Button(() =>
                Apply("Remove platform", () => settings.RemovePlatformAtIndex(index)))
            { text = "Remove", tooltip = "Remove the " + PlatformToString(platform) + " configuration." };
            footer.Add(remove);

            body.Add(footer);
            card.Add(body);
            return card;
        }

        private VisualElement BuildKeyRow(int i, string label, string tooltip, bool isSecret)
        {
            VisualElement row = new VisualElement();
            row.AddToClassList("ga-row");

            Label fieldLabel = new Label(label);
            fieldLabel.AddToClassList("ga-field-label");
            fieldLabel.tooltip = tooltip;
            row.Add(fieldLabel);

            VisualElement wrap = new VisualElement();
            wrap.AddToClassList("ga-keywrap");

            TextField field = new TextField
            {
                value = isSecret ? settings.GetSecretKey(i) : settings.GetGameKey(i),
                isPasswordField = isSecret,
                tooltip = tooltip
            };

            int index = i;
            field.RegisterValueChangedCallback(evt =>
            {
                string current = isSecret ? settings.GetSecretKey(index) : settings.GetGameKey(index);
                if (!evt.newValue.Equals(current))
                {
                    // A hand-edited key unlinks the game that was selected via login.
                    settings.SelectedPlatformOrganization[index] = "";
                    settings.SelectedPlatformStudio[index] = "";
                    settings.SelectedPlatformGame[index] = "";
                }
                if (isSecret)
                {
                    settings.UpdateSecretKey(index, evt.newValue);
                }
                else
                {
                    settings.UpdateGameKey(index, evt.newValue);
                }
                EditorUtility.SetDirty(settings);
            });
            // Rebuild (to refresh status chips) only when the key actually changed
            // during this edit - an unconditional rebuild on blur would swallow the
            // click that moved focus away (e.g. onto the reveal button).
            string valueAtFocusIn = null;
            field.RegisterCallback<FocusInEvent>(_ => valueAtFocusIn = field.value);
            field.RegisterCallback<FocusOutEvent>(_ =>
            {
                bool changed = valueAtFocusIn != null && valueAtFocusIn != field.value;
                valueAtFocusIn = null;
                if (changed)
                {
                    Rebuild();
                }
            });
            wrap.Add(field);

            if (isSecret)
            {
                // Eye button overlaid on the right edge of the field, as in the mockup.
                Image eye = new Image
                {
                    image = EditorGUIUtility.IconContent("animationvisibilitytoggleon").image,
                    scaleMode = ScaleMode.ScaleToFit,
                    pickingMode = PickingMode.Ignore
                };
                Button reveal = null;
                reveal = new Button(() =>
                {
                    field.isPasswordField = !field.isPasswordField;
                    bool revealed = !field.isPasswordField;
                    reveal.tooltip = revealed ? "Hide secret key" : "Show secret key";
                    reveal.EnableInClassList("ga-reveal--on", revealed);
                })
                { tooltip = "Show secret key" };
                reveal.AddToClassList("ga-reveal");
                reveal.Add(eye);
                wrap.Add(reveal);
            }

            row.Add(wrap);
            return row;
        }

        private void BuildBuildNumberRows(VisualElement parent, int i)
        {
            RuntimePlatform platform = settings.Platforms[i];
            int index = i;

            if (!Settings.SupportsBuildNumberAutoDetect(platform))
            {
                VisualElement row = new VisualElement();
                row.AddToClassList("ga-row");
                Label label = new Label("Build number");
                label.AddToClassList("ga-field-label");
                label.tooltip = "The build number reported with events. Auto-detection is available on iOS, tvOS and Android only.";
                row.Add(label);
                TextField field = new TextField { value = settings.Build[i], tooltip = label.tooltip };
                field.RegisterValueChangedCallback(evt => { settings.Build[index] = evt.newValue; EditorUtility.SetDirty(settings); });
                row.Add(field);
                parent.Add(row);
                return;
            }

            bool auto = settings.BuildNumberAutoDetect[i];

            VisualElement sourceRow = new VisualElement();
            sourceRow.AddToClassList("ga-row");
            Label sourceLabel = new Label("Build number");
            sourceLabel.AddToClassList("ga-field-label");
            sourceLabel.tooltip = "Where the build number reported with events comes from.";
            sourceRow.Add(sourceLabel);

            List<string> choices = new List<string>
            {
                "Auto - Player Settings › Version (" + PlayerSettings.bundleVersion + ")",
                "Custom value..."
            };
            DropdownField source = new DropdownField(choices, auto ? 0 : 1) { tooltip = sourceLabel.tooltip };
            source.RegisterValueChangedCallback(_ =>
                Apply("Change build number source", () => settings.BuildNumberAutoDetect[index] = source.index == 0));
            sourceRow.Add(source);
            parent.Add(sourceRow);

            if (!auto)
            {
                VisualElement valueRow = new VisualElement();
                valueRow.AddToClassList("ga-row");
                Label spacer = new Label("");
                spacer.AddToClassList("ga-field-label");
                valueRow.Add(spacer);
                TextField field = new TextField
                {
                    value = settings.Build[i],
                    tooltip = "The build number reported with events for this platform."
                };
                field.RegisterValueChangedCallback(evt => { settings.Build[index] = evt.newValue; EditorUtility.SetDirty(settings); });
                valueRow.Add(field);
                parent.Add(valueRow);
            }
        }

        private void BuildGameSelectionPopups(VisualElement parent, int i)
        {
            int index = i;

            string[] organizationNames = Organization.GetOrganizationNames(settings.Organizations);
            if (settings.SelectedOrganization[i] >= organizationNames.Length)
            {
                settings.SelectedOrganization[i] = 0;
            }
            parent.Add(MakePopupRow("Organization", "Organizations tied to your GameAnalytics user account.",
                organizationNames, settings.SelectedOrganization[i], newIndex =>
                {
                    Apply("Select organization", () =>
                    {
                        settings.SelectedOrganization[index] = newIndex;
                        settings.SelectedStudio[index] = 0;
                        settings.SelectedGame[index] = 0;
                        if (newIndex <= 0)
                        {
                            SetLoginStatus("Please select organization..", settings);
                        }
                        else
                        {
                            SelectOrganization(newIndex, settings, index);
                        }
                    });
                }));

            if (settings.SelectedOrganization[i] <= 0)
            {
                return;
            }

            string[] studioNames = Studio.GetStudioNames(settings.Organizations[settings.SelectedOrganization[i] - 1].Studios);
            if (settings.SelectedStudio[i] >= studioNames.Length)
            {
                settings.SelectedStudio[i] = 0;
            }
            parent.Add(MakePopupRow("Studio", "Studios tied to your GameAnalytics user account.",
                studioNames, settings.SelectedStudio[i], newIndex =>
                {
                    Apply("Select studio", () =>
                    {
                        if (newIndex <= 0)
                        {
                            settings.SelectedStudio[index] = 0;
                            settings.SelectedGame[index] = 0;
                            SetLoginStatus("Please select studio..", settings);
                        }
                        else
                        {
                            SelectStudio(newIndex, settings, index);
                        }
                    });
                }));

            if (settings.SelectedStudio[i] <= 0)
            {
                return;
            }

            string[] gameNames = Studio.GetGameNames(settings.SelectedStudio[i] - 1, settings.Organizations[settings.SelectedOrganization[i] - 1].Studios);
            if (settings.SelectedGame[i] >= gameNames.Length)
            {
                settings.SelectedGame[i] = 0;
            }
            parent.Add(MakePopupRow("Game", "Games tied to the selected GameAnalytics studio.",
                gameNames, settings.SelectedGame[i], newIndex =>
                {
                    Apply("Select game", () => SelectGame(newIndex, settings, index));
                }));
        }

        private static VisualElement MakePopupRow(string label, string tooltip, string[] choices, int selectedIndex, Action<int> onChanged)
        {
            VisualElement row = new VisualElement();
            row.AddToClassList("ga-row");
            Label fieldLabel = new Label(label);
            fieldLabel.AddToClassList("ga-field-label");
            fieldLabel.tooltip = tooltip;
            row.Add(fieldLabel);

            DropdownField popup = new DropdownField(new List<string>(choices), selectedIndex) { tooltip = tooltip };
            popup.RegisterValueChangedCallback(_ => onChanged(popup.index));
            row.Add(popup);
            return row;
        }

        private void BuildSelectedGameSummary(VisualElement parent, int i)
        {
            parent.Add(MakeSummaryRow("Organization", settings.SelectedPlatformOrganization[i]));
            parent.Add(MakeSummaryRow("Studio", settings.SelectedPlatformStudio[i]));
            parent.Add(MakeSummaryRow("Game", settings.SelectedPlatformGame[i]));
        }

        private static VisualElement MakeSummaryRow(string label, string value)
        {
            VisualElement row = new VisualElement();
            row.AddToClassList("ga-row");
            Label fieldLabel = new Label(label);
            fieldLabel.AddToClassList("ga-field-label");
            row.Add(fieldLabel);
            Label valueLabel = new Label(string.IsNullOrEmpty(value) ? "N/A" : value);
            valueLabel.AddToClassList("ga-dim");
            row.Add(valueLabel);
            return row;
        }

        private void BuildAddPlatformRow(VisualElement parent)
        {
            string[] availablePlatforms = settings.GetAvailablePlatforms();
            if (availablePlatforms.Length == 0)
            {
                Label done = new Label("All supported platforms are configured.");
                done.AddToClassList("ga-note");
                done.style.marginTop = 8;
                parent.Add(done);
                return;
            }

            VisualElement card = new VisualElement();
            card.AddToClassList("ga-card");
            VisualElement body = new VisualElement();
            body.AddToClassList("ga-card-body");
            body.style.flexDirection = FlexDirection.Row;
            body.style.alignItems = Align.Center;

            if (_addPlatformIndex >= availablePlatforms.Length)
            {
                _addPlatformIndex = 0;
            }
            DropdownField picker = new DropdownField(new List<string>(availablePlatforms), _addPlatformIndex)
            {
                tooltip = "Platform to add."
            };
            picker.AddToClassList("ga-grow");
            picker.style.marginRight = 6;
            picker.RegisterValueChangedCallback(_ => _addPlatformIndex = picker.index);
            body.Add(picker);

            Button add = new Button(() =>
            {
                string chosen = availablePlatforms[Mathf.Clamp(_addPlatformIndex, 0, availablePlatforms.Length - 1)];
                Apply("Add platform", () => settings.AddPlatform((RuntimePlatform)Enum.Parse(typeof(RuntimePlatform), chosen)));
                _addPlatformIndex = 0;
            })
            { text = "Add platform" };
            body.Add(add);

            card.Add(body);
            parent.Add(card);
        }

        private void SyncPlatformOrganizationList()
        {
            List<string> list = settings.SelectedPlatformOrganization;

            while (list.Count < settings.Platforms.Count)
            {
                list.Add("");
            }
            while (list.Count > settings.Platforms.Count)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        private void OpenGamePage(string urlTemplate, int gameId)
        {
            if (string.IsNullOrEmpty(settings.TokenGA))
            {
                Application.OpenURL(string.Format(urlTemplate, gameId));
            }
            else
            {
                Application.OpenURL(GaLoginUrl);
            }
        }

        private static RuntimePlatform ActiveBuildRuntimePlatform()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android: return RuntimePlatform.Android;
                case BuildTarget.iOS: return RuntimePlatform.IPhonePlayer;
                case BuildTarget.tvOS: return RuntimePlatform.tvOS;
                case BuildTarget.WebGL: return RuntimePlatform.WebGLPlayer;
                case BuildTarget.StandaloneOSX: return RuntimePlatform.OSXPlayer;
                case BuildTarget.StandaloneLinux64: return RuntimePlatform.LinuxPlayer;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64: return RuntimePlatform.WindowsPlayer;
                default: return (RuntimePlatform)(-1);
            }
        }

        #endregion // Setup tab

        #region Events tab

        private void BuildEventsTab(VisualElement parent)
        {
            VisualElement dimensionsBody;
            parent.Add(MakeCard("Custom dimensions", "Custom dimensions documentation.",
                GaConfigurationDocsUrl + "#set-custom-dimensions", out dimensionsBody));

            Label dimensionsNote = new Label("Define your custom dimension values below. Values that are not defined will be ignored.");
            dimensionsNote.AddToClassList("ga-note");
            dimensionsBody.Add(dimensionsNote);

            BuildStringListGroup(dimensionsBody, "CustomDimensions01", "Dimension 01", settings.CustomDimensions01, ValidateCustomDimensionEditor, NumberedNewValue);
            BuildStringListGroup(dimensionsBody, "CustomDimensions02", "Dimension 02", settings.CustomDimensions02, ValidateCustomDimensionEditor, NumberedNewValue);
            BuildStringListGroup(dimensionsBody, "CustomDimensions03", "Dimension 03", settings.CustomDimensions03, ValidateCustomDimensionEditor, NumberedNewValue);

            VisualElement resourcesBody;
            parent.Add(MakeCard("Resource types", "Resource events documentation.",
                GaConfigurationDocsUrl + "#resources", out resourcesBody));

            Label resourcesNote = new Label("Define all your resource currencies and resource item types. Values that are not defined will be ignored.");
            resourcesNote.AddToClassList("ga-note");
            resourcesBody.Add(resourcesNote);

            BuildStringListGroup(resourcesBody, "ResourceCurrencies", "Currencies", settings.ResourceCurrencies, ValidateResourceCurrencyEditor, _ => "NewCurrency");
            BuildStringListGroup(resourcesBody, "ResourceItemTypes", "Item types", settings.ResourceItemTypes, ValidateResourceItemTypeEditor, NumberedNewValue);
        }

        private void BuildStringListGroup(VisualElement parent, string foldKey, string title, List<string> values,
            Func<string, string> validate, Func<int, string> makeNewValue)
        {
            bool open = GetListFold(foldKey);

            VisualElement header = new VisualElement();
            header.AddToClassList("ga-row");
            header.style.marginTop = 6;

            Button arrow = new Button(() =>
            {
                SetListFold(foldKey, !open);
                Rebuild();
            })
            { text = open ? "▼" : "▶", tooltip = open ? "Collapse" : "Expand" };
            arrow.AddToClassList("ga-fold-arrow");
            header.Add(arrow);

            Label titleLabel = new Label(title + "  (" + values.Count + " / " + MaxNumberOfDimensions + ")");
            titleLabel.AddToClassList("ga-subhead");
            titleLabel.style.marginTop = 0;
            titleLabel.style.marginBottom = 0;
            header.Add(titleLabel);

            parent.Add(header);

            if (!open)
            {
                return;
            }

            for (int i = 0; i < values.Count; i++)
            {
                int index = i;
                VisualElement row = new VisualElement();
                row.AddToClassList("ga-row");
                row.style.marginLeft = 16;

                TextField field = new TextField { value = values[i] };
                field.RegisterCallback<FocusOutEvent>(_ =>
                {
                    if (index >= values.Count)
                    {
                        return; // the row was deleted while this field had focus
                    }
                    string validated = validate(field.value);
                    if (validated != values[index])
                    {
                        Apply("Edit " + title, () => values[index] = validated, rebuild: false);
                        field.SetValueWithoutNotify(validated);
                    }
                });
                row.Add(field);

                Button delete = new Button(() =>
                    Apply("Remove " + title + " value", () => values.RemoveAt(index)))
                { text = "✕", tooltip = "Remove value." };
                delete.AddToClassList("ga-fold-arrow");
                row.Add(delete);

                parent.Add(row);
            }

            Button addButton = new Button(() =>
            {
                if (values.Count < MaxNumberOfDimensions)
                {
                    Apply("Add " + title + " value", () => values.Add(makeNewValue(values.Count)));
                }
            })
            { text = "+ Add" };
            addButton.style.alignSelf = Align.FlexStart;
            addButton.style.marginLeft = 16;
            addButton.style.marginTop = 2;
            parent.Add(addButton);
        }

        private static string NumberedNewValue(int currentCount)
        {
            return "New (" + (currentCount + 1) + ")";
        }

        #endregion // Events tab

        #region Advanced tab

        private void BuildAdvancedTab(VisualElement parent)
        {
            VisualElement advancedBody;
            parent.Add(MakeCard("Advanced settings", "Advanced settings documentation.",
                GaConfigurationDocsUrl + "#manual-session-handling", out advancedBody));

            advancedBody.Add(MakeToggle("Manual session handling",
                "Manually choose when to end and start a new session. Note initializing of the SDK will automatically start the first session.",
                () => settings.UseManualSessionHandling, v => settings.UseManualSessionHandling = v));
            advancedBody.Add(MakeToggle("Submit average FPS (legacy)",
                "Submit the average frames per second. Warning: This FPS tracking approach will be replaced in a future update.",
                () => settings.SubmitFpsAverage, v => settings.SubmitFpsAverage = v));

            VisualElement criticalRow = new VisualElement();
            criticalRow.style.flexDirection = FlexDirection.Row;
            criticalRow.style.flexWrap = Wrap.Wrap;
            criticalRow.style.alignItems = Align.Center;

            IntegerField threshold = new IntegerField
            {
                value = settings.FpsCriticalThreshold,
                tooltip = "Frames per second threshold."
            };
            threshold.style.width = 44;
            threshold.SetEnabled(settings.SubmitFpsCritical);
            threshold.RegisterValueChangedCallback(evt =>
            {
                int clamped = Mathf.Clamp(evt.newValue, 5, 99);
                settings.FpsCriticalThreshold = clamped;
                threshold.SetValueWithoutNotify(clamped);
                EditorUtility.SetDirty(settings);
            });

            Toggle critical = MakeToggle("Submit critical FPS (legacy)",
                "Submit a message whenever the frames per second falls below a certain threshold. Warning: This FPS tracking approach will be replaced in a future update.",
                () => settings.SubmitFpsCritical, v => settings.SubmitFpsCritical = v);
            critical.RegisterValueChangedCallback(evt => threshold.SetEnabled(evt.newValue));
            criticalRow.Add(critical);

            Label below = new Label("below");
            below.AddToClassList("ga-dim");
            below.style.marginLeft = 6;
            below.style.marginRight = 4;
            criticalRow.Add(below);
            criticalRow.Add(threshold);
            advancedBody.Add(criticalRow);

            VisualElement loggingBody;
            parent.Add(MakeCard("Logging", "Debug logging documentation.",
                GaLoggingDocsUrl, out loggingBody));

            loggingBody.Add(MakeToggle("Info log in editor",
                "Show info messages from GA in the unity editor console when submitting data.",
                () => settings.InfoLogEditor, v => settings.InfoLogEditor = v));
            loggingBody.Add(MakeToggle("Info log in build",
                "Show info messages from GA in builds (f.x. Xcode for iOS).",
                () => settings.InfoLogBuild, v => settings.InfoLogBuild = v));
            loggingBody.Add(MakeToggle("Verbose log in build",
                "Show full info messages from GA in builds. Note that this option includes long JSON messages sent to the server.",
                () => settings.VerboseLogBuild, v => settings.VerboseLogBuild = v));

            VisualElement healthBody;
            parent.Add(MakeCard("Health tracking", "Health feature documentation.", GaHealthDocsUrl, out healthBody));

            Label general = new Label("GENERAL");
            general.AddToClassList("ga-subhead");
            healthBody.Add(general);

            healthBody.Add(MakeToggle("Submit Unity errors automatically",
                "Submit error and exception messages to the GameAnalytics server. Useful for getting relevant data when the game crashes, etc.",
                () => settings.SubmitErrors, v => settings.SubmitErrors = v));
            healthBody.Add(MakeToggle("Submit native errors automatically (Android, iOS)",
                "Submit error and exception messages from native errors and exceptions to the GameAnalytics server.",
                () => settings.NativeErrorReporting, v => settings.NativeErrorReporting = v));
            healthBody.Add(MakeToggle("SDK init event - boot time (Android, iOS)",
                "Track the boot time: from application launch to the GameAnalytics SDK initialization.",
                () => settings.EnableSDKInitEvent, v => settings.EnableSDKInitEvent = v));
            healthBody.Add(MakeToggle("Hardware info (Android, iOS)",
                "Memory information collected (if available) and added as properties to health events: total device memory, system memory usage and app memory usage.",
                () => settings.EnableHardwareTracking, v => settings.EnableHardwareTracking = v));

            Label sessionPerformance = new Label("SESSION PERFORMANCE");
            sessionPerformance.AddToClassList("ga-subhead");
            healthBody.Add(sessionPerformance);

            healthBody.Add(MakeToggle("FPS histogram (Android, iOS)",
                "Sample FPS across the entire session and send an FPS histogram at session end. Review FPS insights in the GameAnalytics Health feature.",
                () => settings.EnableFPSHistogram, v => settings.EnableFPSHistogram = v));
            healthBody.Add(MakeToggle("Memory usage histogram (Android, iOS)",
                "Sample memory usage across the entire session and send a memory histogram at session end.",
                () => settings.EnableMemoryHistogram, v => settings.EnableMemoryHistogram = v));
            healthBody.Add(MakeToggle("Memory snapshots (Android, iOS)",
                "Performance & error events will take memory usage snapshots.",
                () => settings.EnableMemoryTracking, v => settings.EnableMemoryTracking = v));

            BuildDiagnosticsCard(parent);
        }

        private void BuildDiagnosticsCard(VisualElement parent)
        {
            VisualElement body;
            parent.Add(MakeCard("Diagnostics", "Copy these versions into a support ticket.", GaSupportUrl, out body));

            string diagnostics =
                "Unity SDK: " + Settings.VERSION + "\n" +
                "Unity Editor: " + Application.unityVersion + "\n" +
                "Native Android: " + GA_NativeSdkVersions.Android + "\n" +
                "Native iOS/tvOS: " + GA_NativeSdkVersions.AppleIosTvos + "\n" +
                "Native desktop: " + GA_NativeSdkVersions.Desktop + "\n" +
                "WebGL library: " + GA_NativeSdkVersions.WebGL + "\n" +
                "Install channel: " + GA_UpdateChecker.InstallChannel;

            foreach (string line in diagnostics.Split('\n'))
            {
                int split = line.IndexOf(':');
                VisualElement row = new VisualElement();
                row.AddToClassList("ga-row");
                Label key = new Label(line.Substring(0, split));
                key.AddToClassList("ga-field-label");
                row.Add(key);
                row.Add(new Label(line.Substring(split + 1).Trim()));
                body.Add(row);
            }

            VisualElement footer = new VisualElement();
            footer.style.flexDirection = FlexDirection.Row;
            footer.style.justifyContent = Justify.SpaceBetween;
            footer.style.alignItems = Align.Center;
            footer.style.marginTop = 6;

            footer.Add(MakeLink("Contact support ↗", () => Application.OpenURL(GaSupportUrl),
                "Opens gameanalytics.com/contact - paste the copied block into your ticket."));

            Button copy = new Button(() =>
            {
                EditorGUIUtility.systemCopyBuffer = diagnostics;
                Debug.Log("GameAnalytics: diagnostics copied to clipboard.");
            })
            { text = "Copy", tooltip = "Copy all versions for a support ticket." };
            footer.Add(copy);

            body.Add(footer);
        }

        #endregion // Advanced tab

        #region Footer

        private void BuildFooter(VisualElement parent)
        {
            VisualElement footer = new VisualElement();
            footer.AddToClassList("ga-footer");

            footer.Add(MakeLink("Documentation ↗", () => Application.OpenURL(GaDocsUrl),
                "Opens the GameAnalytics Unity SDK documentation."));
            footer.Add(MakeLink("Contact support ↗", () => Application.OpenURL(GaSupportUrl),
                "The preferred channel for bugs, account and integration help."));
            footer.Add(MakeLink("Report an issue ↗", () => Application.OpenURL(GaIssuesUrl),
                "GitHub issue tracker. Contact support is the preferred channel."));

            parent.Add(footer);
        }

        #endregion // Footer

        #region Shared UI helpers

        private static VisualElement MakeCard(string title, string helpTooltip, string docUrl, out VisualElement body)
        {
            VisualElement card = new VisualElement();
            card.AddToClassList("ga-card");

            VisualElement header = new VisualElement();
            header.AddToClassList("ga-card-header");
            Label titleLabel = new Label(title);
            titleLabel.AddToClassList("ga-card-title");
            header.Add(titleLabel);
            if (docUrl != null)
            {
                header.Add(MakeHelpButton(helpTooltip, docUrl));
            }
            card.Add(header);

            body = new VisualElement();
            body.AddToClassList("ga-card-body");
            card.Add(body);
            return card;
        }

        private static Button MakeHelpButton(string tooltip, string docUrl)
        {
            Button help = new Button(() => Application.OpenURL(docUrl))
            {
                text = "?",
                tooltip = tooltip + " Opens " + docUrl
            };
            help.AddToClassList("ga-help-button");
            return help;
        }

        private static Button MakeLink(string text, Action onClick, string tooltip)
        {
            Button link = new Button(onClick) { text = text, tooltip = tooltip };
            link.AddToClassList("ga-link");
            return link;
        }

        private Toggle MakeToggle(string label, string tooltip, Func<bool> get, Action<bool> set)
        {
            Toggle toggle = new Toggle { text = label, value = get(), tooltip = tooltip };
            toggle.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(settings, label);
                set(evt.newValue);
                EditorUtility.SetDirty(settings);
            });
            return toggle;
        }

        private static void OpenSignUp()
        {
            Application.OpenURL(GaSignUpUrl);
        }

        private static string PlatformToString(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.tvOS:
                    return "tvOS";
                default:
                    return platform.ToString();
            }
        }

        /// <summary>
        /// IMGUI separator kept for GA_SignUp, which still draws with OnGUI.
        /// </summary>
        public static void Splitter(Color rgb, float thickness = 1, int margin = 0)
        {
            GUIStyle splitter = new GUIStyle();
            splitter.normal.background = EditorGUIUtility.whiteTexture;
            splitter.stretchWidth = true;
            splitter.margin = new RectOffset(margin, margin, 7, 7);

            Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

            if (Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = rgb;
                splitter.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        #endregion // Shared UI helpers

        #region Account web requests

        private static void LoginUser(Settings settings)
        {
            Hashtable jsonTable = new Hashtable();
            jsonTable["email"] = settings.EmailGA;
            jsonTable["password"] = settings.PasswordGA;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(GA_MiniJSON.Serialize(jsonTable));

            UnityWebRequest www = new UnityWebRequest(GaUrl + "token", UnityWebRequest.kHttpVerbPOST);
            www.uploadHandler = new UploadHandlerRaw(data)
            {
                contentType = "application/json"
            };
            www.downloadHandler = new DownloadHandlerBuffer();

            foreach (KeyValuePair<string, string> entry in GA_EditorUtilities.WWWHeaders())
            {
                www.SetRequestHeader(entry.Key, entry.Value);
            }

            GA_ContinuationManager.StartCoroutine(LoginUserCoroutine(www, settings), () => www.isDone);
        }

        private static IEnumerator LoginUserCoroutine(UnityWebRequest www, Settings settings)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
                IDictionary<string, object> response = DeserializeResponse(www);
                string error = GetFirstErrorMessage(response);

                if (!RequestFailed(www))
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        SetLoginStatus("Failed to login.", settings);
                    }
                    else if (response != null)
                    {
                        IList<object> resultList = response["results"] as IList<object>;
                        IDictionary<string, object> results = resultList[0] as IDictionary<string, object>;
                        settings.TokenGA = results["token"].ToString();

                        SetLoginStatus("Logged in. Getting data.", settings);

                        GetUserData(settings);
                    }
                }
                else if (IsApiOutdated(www.responseCode))
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
            catch (Exception e)
            {
                Debug.LogError("Failed to login:" + e);
                Debug.LogError(e.StackTrace);
                SetLoginStatus("Failed to login.", settings);
            }
        }

        private static void GetUserData(Settings settings)
        {
            UnityWebRequest www = UnityWebRequest.Get(GaUrl + "user");
            foreach (KeyValuePair<string, string> entry in GA_EditorUtilities.WWWHeadersWithAuthorization(settings.TokenGA))
            {
                www.SetRequestHeader(entry.Key, entry.Value);
            }

            GA_ContinuationManager.StartCoroutine(GetUserDataCoroutine(www, settings), () => www.isDone);
        }

        private static IEnumerator GetUserDataCoroutine(UnityWebRequest www, Settings settings)
        {
            yield return www.SendWebRequest();

            while (!www.isDone)
                yield return null;

            try
            {
                IDictionary<string, object> response = DeserializeResponse(www);
                string error = GetFirstErrorMessage(response);

                if (!RequestFailed(www))
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError(error);
                        SetLoginStatus("Failed to get data.", settings);
                    }
                    else if (response != null)
                    {
                        IList<object> resultList = response["results"] as IList<object>;
                        IDictionary<string, object> results = resultList[0] as IDictionary<string, object>;

                        settings.Organizations = ParseOrganizations(results);

                        if (settings.Organizations.Count == 1 && settings.Organizations[0].Studios.Count == 1)
                        {
                            bool autoSelectedPlatform = false;
                            for (int i = 0; i < settings.Platforms.Count; ++i)
                            {
                                if (settings.Platforms[i] == settings.LastCreatedGamePlatform)
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
                    }
                }
                else if (IsApiOutdated(www.responseCode))
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
                Debug.LogError("Failed to get user data: " + e + ", " + e.StackTrace);
                SetLoginStatus("Failed to get data.", settings);
            }
        }

        private static List<Organization> ParseOrganizations(IDictionary<string, object> results)
        {
            IDictionary<string, object> orgs = results["organizations"] as IDictionary<string, object>;
            IList<object> studioList = results["studios"] as IList<object>;

            Dictionary<string, Organization> organizationMap = new Dictionary<string, Organization>();
            List<Organization> organizations = new List<Organization>();
            foreach (KeyValuePair<string, object> pair in orgs)
            {
                IDictionary<string, object> organization = pair.Value as IDictionary<string, object>;
                Organization o = new Organization(organization["name"].ToString(), organization["id"].ToString());
                organizations.Add(o);
                organizationMap.Add(o.ID, o);
            }

            for (int s = 0; s < studioList.Count; s++)
            {
                IDictionary<string, object> studio = studioList[s] as IDictionary<string, object>;

                if (GetFlag(studio, "demo") || GetFlag(studio, "archived"))
                {
                    continue;
                }

                List<Game> games = new List<Game>();
                List<object> gamesList = (List<object>)studio["games"];
                for (int g = 0; g < gamesList.Count; g++)
                {
                    IDictionary<string, object> game = gamesList[g] as IDictionary<string, object>;

                    if (!GetFlag(game, "archived") && !GetFlag(game, "disabled"))
                    {
                        games.Add(new Game(game["name"].ToString(), int.Parse(game["id"].ToString()), game["key"].ToString(), game["secret"].ToString()));
                    }
                }

                Studio st = new Studio(studio["name"].ToString(), studio["id"].ToString(), studio["org_id"].ToString(), games);
                organizationMap[st.OrganizationID].Studios.Add(st);
            }

            return organizations;
        }

        private static bool GetFlag(IDictionary<string, object> data, string key)
        {
            return data.ContainsKey(key) && (bool)data[key];
        }

        private static IDictionary<string, object> DeserializeResponse(UnityWebRequest www)
        {
            string text = www.downloadHandler.text;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return GA_MiniJSON.Deserialize(text) as IDictionary<string, object>;
        }

        private static string GetFirstErrorMessage(IDictionary<string, object> response)
        {
            if (response == null || !response.ContainsKey("errors"))
            {
                return "";
            }

            IList<object> errorList = response["errors"] as IList<object>;
            if (errorList != null && errorList.Count > 0)
            {
                IDictionary<string, object> errors = errorList[0] as IDictionary<string, object>;
                if (errors.ContainsKey("msg"))
                {
                    return errors["msg"].ToString();
                }
            }
            return "";
        }

        private static bool RequestFailed(UnityWebRequest www)
        {
            return www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError;
        }

        private static bool IsApiOutdated(long responseCode)
        {
            return responseCode == 301 || responseCode == 404 || responseCode == 410;
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

            Studio studio = settings.Organizations[settings.SelectedOrganization[platform] - 1].Studios[index - 1];
            if (studio.Games.Count == 1)
            {
                if (settings.IsGameKeyValid(platform, studio.Games[0].GameKey) &&
                    settings.IsSecretKeyValid(platform, studio.Games[0].SecretKey))
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
                return;
            }

            Organization organization = settings.Organizations[settings.SelectedOrganization[platform] - 1];
            Studio studio = organization.Studios[settings.SelectedStudio[platform] - 1];
            Game game = studio.Games[index - 1];

            if (settings.IsGameKeyValid(platform, game.GameKey) && settings.IsSecretKeyValid(platform, game.SecretKey))
            {
                settings.SelectedPlatformOrganization[platform] = organization.Name;
                settings.SelectedPlatformStudio[platform] = studio.Name;
                settings.SelectedPlatformGame[platform] = game.Name;
                settings.SelectedPlatformGameID[platform] = game.ID;
                settings.UpdateGameKey(platform, game.GameKey);
                settings.UpdateSecretKey(platform, game.SecretKey);
                SetLoginStatus("Received keys. Ready to go!", settings);
            }
            else if (!settings.IsGameKeyValid(platform, game.GameKey))
            {
                Debug.LogError("[GameAnalytics] Game key already exists for another platform. Platforms can't use the same key.");
                settings.SelectedGame[platform] = 0;
            }
            else
            {
                Debug.LogError("[GameAnalytics] Secret key already exists for another platform. Platforms can't use the same key.");
                settings.SelectedGame[platform] = 0;
            }
        }

        private static void SetLoginStatus(string status, Settings settings)
        {
            settings.LoginStatus = status;
            EditorUtility.SetDirty(settings);
        }

        #endregion // Account web requests

        #region UI validation

        /// <summary>
        /// Check if a string matches a defined pattern
        /// </summary>
        /// <returns><c>true</c>, if match <c>false</c> otherwise.</returns>
        /// <param name="s">Given string</param>
        /// <param name="pattern">Pattern.</param>
        public static bool StringMatch(string s, string pattern)
        {
            if (s == null || pattern == null)
            {
                return false;
            }

            return Regex.IsMatch(s, pattern);
        }

        private static string ValidateResourceCurrencyEditor(string currency)
        {
            if (!StringMatch(currency, "^[A-Za-z]+$"))
            {
                if (currency != null)
                {
                    Debug.LogError("Validation fail - resource currency: Cannot contain other characters than 'A-Za-z'. String:'" + currency + "'");
                }
                return "Empty";
            }
            if (ConsistsOfWhiteSpace(currency))
            {
                return "Empty";
            }
            return currency;
        }

        private static string ValidateResourceItemTypeEditor(string itemType)
        {
            if (itemType.Length > 64)
            {
                Debug.LogError("Validation fail - resource itemType cannot be longer than 64 chars.");
                return "Empty";
            }
            if (!StringMatch(itemType, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,64}$"))
            {
                if (itemType != null)
                {
                    Debug.LogError("Validation fail - resource itemType: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: '" + itemType + "'");
                }
                return "Empty";
            }
            if (ConsistsOfWhiteSpace(itemType))
            {
                return "Empty";
            }
            return itemType;
        }

        private static string ValidateCustomDimensionEditor(string customDimension)
        {
            if (customDimension.Length > 32)
            {
                Debug.LogError("Validation fail - custom dimension cannot be longer than 32 chars.");
                return "Empty";
            }
            if (!StringMatch(customDimension, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,32}$"))
            {
                if (customDimension != null)
                {
                    Debug.LogError("Validation fail - custom dimension: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: '" + customDimension + "'");
                }
                return "Empty";
            }
            if (ConsistsOfWhiteSpace(customDimension))
            {
                return "Empty";
            }
            return customDimension;
        }

        private static bool ConsistsOfWhiteSpace(string s)
        {
            foreach (char c in s)
            {
                if (c != ' ')
                    return false;
            }
            return true;
        }

        #endregion // UI validation
    }
}
