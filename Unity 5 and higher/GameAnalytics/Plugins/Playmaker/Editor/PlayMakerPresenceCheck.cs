using UnityEngine;
using UnityEditor;
using System;

public class PlayMakerPresenceCheck : AssetPostprocessor{

	static string PlayMakerTypeCheck = "HutongGames.PlayMaker.Actions.ActivateGameObject, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
	static string PlayMakerBridgeTypeCheck = "HutongGames.PlayMaker.Actions.SendDesignEvent, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

	static string IgnorePlayMakerBridgeKey = "IgnorePlayMakerBridge";
	static string PlayMakerBridgeEnabledKey = "PlayMakerBridgeEnabled";

	static bool _debug = false;


	public static void ResetPrefs()
	{
		EditorPrefs.DeleteKey(IgnorePlayMakerBridgeKey+"-"+Application.dataPath);
		EditorPrefs.DeleteKey(PlayMakerBridgeEnabledKey+"-"+Application.dataPath);
	}

	static void OnPostprocessAllAssets ( string[] importedAssets,string[] deletedAssets,string[] movedAssets,string[] movedFromAssetPaths)
	{

	 
		//check here if we have access to a PlayMaker class, if we do, then we can alert the user.
		bool _playmakerDetected = System.Type.GetType(PlayMakerTypeCheck) != null;

		// check here if we have access to the PlayMaker Bridge Class.
		bool _bridgeEnabled =  System.Type.GetType(PlayMakerBridgeTypeCheck) !=null;

		if (_debug)
		{
			Debug.Log("PlayMaker detected : "+_playmakerDetected+ " , Bridge enabled="+_bridgeEnabled);
		}

		if (_playmakerDetected)
		{

			if (! _bridgeEnabled)
			{

				if (EditorPrefs.GetBool(IgnorePlayMakerBridgeKey+"-"+Application.dataPath))
				{
					if (_debug)
					{
						Debug.Log("Ignore detection alert");
					}
					return;
				}

				if (EditorPrefs.GetBool(PlayMakerBridgeEnabledKey+"-"+Application.dataPath) )
				{
					if (_debug)
					{
						Debug.Log("PlayMaker found but bridge not enabled, tho we actually enabled it, so we bail");
					}
					return;
				}

				if (_debug)
				{
					Debug.Log("PlayMaker found but bridge not enabled");
				}

				if (EditorUtility.DisplayDialog("GameAnalytics : PlayMaker Detected","Do you want to enable PlayMaker Actions for GameAnalytics?","Yes","No"))
				{
					EditorPrefs.SetBool(PlayMakerBridgeEnabledKey+"-"+Application.dataPath,true);

					EditorApplication.ExecuteMenuItem("Window/GameAnalytics/PlayMaker/Toggle Scripts");


				}else{
					EditorPrefs.SetBool(IgnorePlayMakerBridgeKey+"-"+Application.dataPath,true);
					Debug.Log("To enable PlayMaker support for GameAnalytics manualy, simply go to the menu: 'Window/GameAnalytics/PlayMaker/Toggle Scripts'");
				}
			}
		}

	}
}
