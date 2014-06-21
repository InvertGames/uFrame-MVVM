using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	///Automatically added when needed, collectively calls methods that needs updating
	public class MonoManager : MonoBehaviour {

		//This is actually faster than adding/removing to delegate
		private List<System.Action> updateMethods = new List<System.Action>();
		private static bool isQuiting;

		private static MonoManager _current;
		public static MonoManager current{
			get
			{
				if (_current == null && !isQuiting)
					_current = new GameObject("_MonoManager").AddComponent<MonoManager>();

				return _current;
			}

			private set {_current = value;}
		}

		public static void Create(){
			_current = current;
		}

		//This is actually faster than adding/removing to delegate
		public void AddMethod(System.Action method){
			updateMethods.Add(method);
		}

		//This is actually faster than adding/removing to delegate
		public void RemoveMethod(System.Action method){
			updateMethods.Remove(method);
		}

		void OnApplicationQuit(){
			isQuiting = true;
		}

		void Awake(){

			if (_current != null && _current != this){
				DestroyImmediate(this.gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			_current = this;
		}

		void Update(){

			for (int i = 0; i < updateMethods.Count; i++)
				updateMethods[i]();
		}
	}
}