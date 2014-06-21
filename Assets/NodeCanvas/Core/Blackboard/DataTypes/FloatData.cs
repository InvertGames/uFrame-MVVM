using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class FloatData : Data{

		public float value;

		public override object objectValue{
			get {return value;}
			set {this.value = (float)value;}
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			GUI.backgroundColor = new Color(0.7f,0.7f,1);
			value = UnityEditor.EditorGUILayout.FloatField(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}