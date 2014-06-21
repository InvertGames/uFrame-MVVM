#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class StringData : Data{

		public string value = string.Empty;

		public override object objectValue{
			get {return value;}
			set {this.value = (string)value;}
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR	

		public override void ShowDataGUI(){
			GUI.backgroundColor = new Color(0.5f,0.5f,0.5f);
			value = EditorGUILayout.TextField(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}