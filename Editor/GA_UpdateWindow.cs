using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GameAnalyticsSDK.Setup;

namespace GameAnalyticsSDK.Editor
{
    /// <summary>
    /// "Update available" window. The primary action adapts to how the SDK was
    /// installed (see GA_UpdateChecker.InstallChannel).
    /// </summary>
    public class GA_UpdateWindow : EditorWindow
    {
        public static void Open()
        {
            if (Application.isBatchMode)
            {
                return;
            }

            GA_UpdateWindow window = GetWindow<GA_UpdateWindow>(utility: true, title: "GameAnalytics - Update available");
            window.position = new Rect(150, 150, 420, 360);
            window.minSize = new Vector2(360, 280);
            window.Show();
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            StyleSheet styleSheet = GA_EditorPaths.LoadStyleSheet("GA_SettingsInspector.uss");
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }
            root.style.paddingLeft = 12;
            root.style.paddingRight = 12;
            root.style.paddingTop = 10;
            root.style.paddingBottom = 10;

            root.Add(MakeVersionRow("Installed version", Settings.VERSION));
            root.Add(MakeVersionRow("Latest version", GetNewVersion()));

            Label changesTitle = new Label("Changes");
            changesTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
            changesTitle.style.marginTop = 10;
            root.Add(changesTitle);

            // flex-shrink defaults to 0 in UI Toolkit; without it a long changelog
            // pushes the hint and buttons out of the window instead of scrolling.
            ScrollView changes = new ScrollView();
            changes.style.flexGrow = 1;
            changes.style.flexShrink = 1;
            changes.style.minHeight = 60;
            changes.style.marginTop = 4;
            changes.AddToClassList("ga-changelog");
            Label changesLabel = new Label(string.IsNullOrEmpty(GameAnalytics.SettingsGA.Changes)
                ? "See the full changelog on GitHub."
                : GameAnalytics.SettingsGA.Changes);
            changesLabel.style.whiteSpace = WhiteSpace.Normal;
            changes.Add(changesLabel);
            root.Add(changes);

            Label hint = new Label(GA_UpdateChecker.UpdateActionHint);
            hint.style.whiteSpace = WhiteSpace.Normal;
            hint.style.marginTop = 8;
            hint.style.flexShrink = 0;
            hint.AddToClassList("ga-dim");
            root.Add(hint);

            VisualElement buttons = new VisualElement();
            buttons.style.flexDirection = FlexDirection.Row;
            buttons.style.justifyContent = Justify.FlexEnd;
            buttons.style.marginTop = 10;
            buttons.style.flexShrink = 0;

            Button skip = new Button(() =>
            {
                GA_UpdateChecker.SkippedVersion = GetNewVersion();
                Close();
            })
            { text = "Skip this version", tooltip = "Do not prompt again for this version." };
            buttons.Add(skip);

            Button update = new Button(() => GA_UpdateChecker.ApplyUpdate(GetNewVersion()))
            {
                text = GA_UpdateChecker.UpdateActionLabel,
                tooltip = GA_UpdateChecker.UpdateActionHint
            };
            update.AddToClassList("ga-button-primary");
            buttons.Add(update);

            root.Add(buttons);
        }

        private static VisualElement MakeVersionRow(string label, string value)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            Label key = new Label(label);
            key.style.width = 120;
            key.AddToClassList("ga-dim");
            row.Add(key);
            row.Add(new Label(value));
            return row;
        }

        public static void SetNewVersion(string newVersion)
        {
            if (!string.IsNullOrEmpty(newVersion))
            {
                GameAnalytics.SettingsGA.NewVersion = newVersion;
            }
        }

        public static string GetNewVersion()
        {
            return GameAnalytics.SettingsGA.NewVersion;
        }

        // Always overwrites, so notes from an earlier check never outlive it.
        public static void SetChanges(string changes)
        {
            GameAnalytics.SettingsGA.Changes = changes ?? "";
        }

        public static string UpdateStatus(string currentVersion)
        {
            return GA_UpdateChecker.IsNewVersion(GameAnalytics.SettingsGA.NewVersion, currentVersion) ? "New update" : "";
        }
    }
}
