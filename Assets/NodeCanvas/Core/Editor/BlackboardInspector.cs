using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;
using NodeCanvas.Variables;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(Blackboard))]
	public class BlackboardInspector : Editor {

		void OnEnable(){
			foreach (Data data in (target as Blackboard).GetAllData()){
				if (data != null)
					data.hideFlags = HideFlags.HideInInspector;
			}
		}

		override public void OnInspectorGUI(){
			
			(target as Blackboard).ShowBlackboardGUI();
			EditorUtils.EndOfInspector();
			if (Application.isPlaying)
				Repaint();
		}
	}
}