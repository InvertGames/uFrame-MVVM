using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class Vector2Data : Data{

		public Vector2 value;

		public override object objectValue{
			get {return value;}
			set {this.value = (Vector2)value;}
		}

		public override object GetSerialized(){
			return new float[] {value.x, value.y};
		}

		public override void SetSerialized(object obj){
			var floatArr = obj as float[];
			value = new Vector3(floatArr[0], floatArr[1]);
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.Vector2Field("", value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(18));
		}

		#endif
	}
}