using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class GameObjectListData : Data{

		public List<GameObject> value = new List<GameObject>();

		public override object objectValue{
			get {return value;}
			set {this.value = (List<GameObject>)value;}
		}

		public override object GetSerialized(){

			var goPaths = new List<string>();
			foreach (GameObject go in value){

				GameObject obj= go;
				if (obj == null){
					goPaths.Add(null);
					continue;
				}

				string path= "/" + obj.name;

				while (obj.transform.parent != null){
					obj = obj.transform.parent.gameObject;
					path = "/" + obj.name + path;
				}
				
				goPaths.Add(path);
			}

			return goPaths;
		}

		public override void SetSerialized(object obj){

			List<string> goPaths = new List<string>(obj as List<string>);
			foreach (string goPath in goPaths){
				GameObject go= GameObject.Find(goPath);
				value.Add(go);
				if (!go)
					Debug.LogWarning("GameObjectListData Failed to load a GameObject in the list. GameObject was not found in scene. Path '" + goPath + "'");
			}
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){

			if (GUILayout.Button(value.Count + " GameObjects", GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)))
				NodeCanvasEditor.GameObjectListEditor.Show(value);
		}

		#endif
	}
}