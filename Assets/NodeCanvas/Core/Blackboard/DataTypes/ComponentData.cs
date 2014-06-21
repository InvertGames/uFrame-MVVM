#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class ComponentData : Data{

		public Component value;
		
		[SerializeField]
		private string _typeName;

		private System.Type type{
			get {return value != null? value.GetType() : System.Type.GetType(_typeName);}
			set{
				_typeName = value.AssemblyQualifiedName;
				if (this.value != null && !value.IsAssignableFrom(this.value.GetType()) )
					this.value = null;
			}
		}

		public override System.Type dataType{
			get {return type;}
		}

		public override object objectValue{
			get {return value;}
			set
			{
				this.value = (Component)value;
				if (value != null && !type.IsAssignableFrom(value.GetType()))
					type = value.GetType();			
			}
		}

		public override object GetSerialized(){

			if (value == null)
				return null;

			GameObject obj = value.gameObject;
			if (obj == null)
				return null;

			string path= "/" + obj.name;
			while (obj.transform.parent != null){
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}

			return new SerializedComponent(path, value.GetType());
		}

		public override void SetSerialized(object obj){

			SerializedComponent serComponent = obj as SerializedComponent;
			if (obj == null){
				value = null;
				return;
			}

			type = serComponent.trueType;
			GameObject go = GameObject.Find(serComponent.path);
			if (!go){
				Debug.LogWarning("ComponentData Failed to load. The component's gameobject was not found in the scene. Path '" + serComponent.path + "'");
				return;
			}

			value = go.GetComponent(serComponent.trueType);
			if (value == null)
				Debug.LogWarning("ComponentData Failed to load. GameObject was found but the component of type '" + serComponent.trueType.ToString() + "' itself was not. Path '" + serComponent.path + "'");
		}


		[System.Serializable]
		private class SerializedComponent{

			public string path;
			public System.Type trueType;

			public SerializedComponent(string path, System.Type type){
				this.path = path;
				this.trueType = type;
			}
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		void OnValidate(){
			if (type == null)
				type = typeof(Component);
		}

		void Reset(){
			type = typeof(Component);
		}

		public override void ShowDataGUI(){

			value = EditorGUILayout.ObjectField(value, dataType, true, GUILayout.MaxWidth(90), GUILayout.ExpandWidth(true)) as Component;

			if (GUILayout.Button("", GUILayout.Width(10), GUILayout.Height(14))){
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Component"), false, Selected, typeof(Component));
				menu.AddItem(new GUIContent("Transform"), false, Selected, typeof(Transform));
				menu.AddItem(new GUIContent("Rigidbody"), false, Selected, typeof(Rigidbody));
				menu.AddItem(new GUIContent("Collider"), false, Selected, typeof(Collider));

				foreach(System.Type t in EditorUtils.GetAssemblyTypes(typeof(Component))){
					var friendlyName = t.Assembly.GetName().Name + "/" + (string.IsNullOrEmpty(t.Namespace)? "" : t.Namespace + "/") + t.Name;
					menu.AddItem(new GUIContent(friendlyName), false, Selected, t);
				}

				menu.ShowAsContext();
			}
		}

		void Selected(object t){
			type = (System.Type)t;
		}

		#endif
	}
}