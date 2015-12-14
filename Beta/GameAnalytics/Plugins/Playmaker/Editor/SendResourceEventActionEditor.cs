#if false

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;
using GameAnalyticsSDK;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof (SendResourceEvent))]
	public class SendResourceEventActionEditor : CustomActionEditor
    {
		
        public override bool OnGUI()
        {
			bool edited = false;
			SendResourceEvent _target = (SendResourceEvent)target;
			
			if (_target.ResourceFlowTypeAsString == null)
			{
				_target.ResourceFlowTypeAsString = new HutongGames.PlayMaker.FsmString(){ UseVariable=false };
			}
			
			if (_target.ResourceFlowTypeAsString.UseVariable)
			{
				EditField("ResourceFlowTypeAsString");
					
			}
			else
			{
				GUILayout.BeginHorizontal();
				_target.ResourceFlowType = (GA_Resource.GAResourceFlowType)EditorGUILayout.EnumPopup("Resource Flow Type", _target.ResourceFlowType);
				
				if (PlayMakerEditor.FsmEditorGUILayout.MiniButtonPadded(PlayMakerEditor.FsmEditorContent.VariableButton))
				{
					_target.ResourceFlowTypeAsString.UseVariable = true;
				}
				GUILayout.EndHorizontal();
			}

			EditField("ResourceCurrency");
			EditField("Amount");
			EditField("ItemType");
			EditField("ItemID");

			return GUI.changed || edited;
        }

    }
}

#endif