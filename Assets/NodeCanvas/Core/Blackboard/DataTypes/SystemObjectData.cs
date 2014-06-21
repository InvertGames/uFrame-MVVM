using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class SystemObjectData : Data{

		public object value;

		public override System.Type dataType{
			get {return value != null? value.GetType() : typeof(object);}
		}

		public override object objectValue{
			get {return value;}
			set {this.value = value;}
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		override public void ShowDataGUI(){
			GUILayout.Label("(" + dataType + ")", GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}