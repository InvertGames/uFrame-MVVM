#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace NodeCanvas.Conditions{

	[Category("Input")]
	public class CheckKeyboardInput : ConditionTask{

		public enum PressTypes {KeyDown, KeyUp, KeyPressed}
		public PressTypes PressType= PressTypes.KeyDown;
		public KeyCode Key= KeyCode.Space;

		protected override string info{
			get {return PressType.ToString() + " " + Key.ToString();}
		}

		protected override bool OnCheck(){

			if (PressType == PressTypes.KeyDown)
				return Input.GetKeyDown(Key);

			if (PressType == PressTypes.KeyUp)
				return Input.GetKeyUp(Key);

			if (PressType == PressTypes.KeyPressed)
				return Input.GetKey(Key);

			return false;
		}
		

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnTaskInspectorGUI(){

			EditorGUILayout.BeginHorizontal();
			PressType = (PressTypes)EditorGUILayout.EnumPopup(PressType);
			Key = (KeyCode)EditorGUILayout.EnumPopup(Key);
			EditorGUILayout.EndHorizontal();
		}

		#endif
	}
}