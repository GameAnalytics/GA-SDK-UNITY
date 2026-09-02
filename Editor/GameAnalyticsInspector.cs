/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;

namespace GameAnalyticsSDK.Editor
{
	[CustomEditor(typeof(GameAnalytics))]
	public class GameAnalyticsInspector : UnityEditor.Editor
	{
		private GUIContent _documentationLink		= new GUIContent("Help", "Opens the GameAnalytics Unity SDK documentation page in your browser.");
		//private GUIContent _guiAllowScreenshot		= new GUIContent("Take Screenshot", "If enabled the player will be able to include a screenshot when submitting feedback and bug reports (This feature is not yet fully implemented).");
		
		override public void OnInspectorGUI ()
		{
			GameAnalytics ga = target as GameAnalytics;
			
			EditorGUI.indentLevel = 1;
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
			
			GUILayout.Label("GameAnalytics Object",EditorStyles.largeLabel);
			
			if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(60)))
			{
				Application.OpenURL("https://docs.gameanalytics.com/event-tracking-and-integrations/sdks-and-collection-api/game-engine-sdks/unity");
			}
			
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Basic component for initializing GameAnalytics.",EditorStyles.miniLabel);
			GUILayout.EndHorizontal();

			EditorGUILayout.Space();

			if (GUI.changed)
			{
	            EditorUtility.SetDirty(ga);
	        }
		}
	}
}