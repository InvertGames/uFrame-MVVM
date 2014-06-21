using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class BoolData : Data{

		public bool value;

		public override object objectValue{
			get {return value;}
			set {this.value = (bool)value;}
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		override public void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.Toggle(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}