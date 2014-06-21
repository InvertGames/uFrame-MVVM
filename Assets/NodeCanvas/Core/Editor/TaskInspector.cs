using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(Task))]
	public class TaskInspector : Editor {

		override public void OnInspectorGUI(){
			
			(target as Task).ShowInspectorGUI();
			EditorUtils.EndOfInspector();

			Repaint();
			
			if (GUI.changed)
				EditorUtility.SetDirty(target);
		}
	}
}