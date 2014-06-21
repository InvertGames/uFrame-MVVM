using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(ActionList))]
	public class ActionListInspector : Editor{

		private ActionList actionList{
			get {return target as ActionList;}
		}

		void OnEnable(){
			foreach (ActionTask action in actionList.actions){
				if (action != null)
					action.hideFlags = HideFlags.HideInInspector;
			}
		}

		public override void OnInspectorGUI(){

			EditorGUILayout.LabelField("Elapsed Time", actionList.elapsedTime.ToString());
			actionList.ShowInspectorGUI();
			if (GUI.changed)
				EditorUtility.SetDirty(actionList);
		}
	}
}