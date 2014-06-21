using UnityEngine;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class ColorData : Data {

		public Color value;

		public override object objectValue{
			get {return value;}
			set {this.value = (Color)value;}
		}

		public override System.Object GetSerialized(){
			return new float[] {value.r, value.g, value.b, value.a};
		}

		public override void SetSerialized(System.Object obj){
			var floatArr = obj as float[];
			value = new Color(floatArr[0], floatArr[1], floatArr[2], floatArr[3]);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		public override void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.ColorField(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}