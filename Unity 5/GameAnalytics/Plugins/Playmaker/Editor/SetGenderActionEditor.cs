#if false

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;
using GameAnalyticsSDK;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof (SetGender))]
    public class SetGenderActionEditor : CustomActionEditor
    {
		
        public override bool OnGUI()
        {
			bool edited = false;
			SetGender _target = (SetGender)target;
			
			if (_target.GenderAsString==null)
			{
				_target.GenderAsString = new HutongGames.PlayMaker.FsmString(){UseVariable=false};
			}
			
			if (_target.GenderAsString.UseVariable)
			{
				EditField("GenderAsString");
					
			}else{
				GUILayout.BeginHorizontal();
				 	_target.Gender = (GA_Setup.GAGender)EditorGUILayout.EnumPopup("Gender", _target.Gender);
				
					if (PlayMakerEditor.FsmEditorGUILayout.MiniButtonPadded(PlayMakerEditor.FsmEditorContent.VariableButton))
					{
						_target.GenderAsString.UseVariable = true;
					}
				GUILayout.EndHorizontal();
			}
			
			return GUI.changed || edited;
        }

    }
}

#endif