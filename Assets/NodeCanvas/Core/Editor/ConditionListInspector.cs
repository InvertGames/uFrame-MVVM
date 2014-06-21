using UnityEngine;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(ConditionList))]
	public class ConditionListInspector : TaskInspector{

		public override void OnInspectorGUI(){

			(target as ConditionList).ShowInspectorGUI();
			if (GUI.changed)
				EditorUtility.SetDirty(target as ConditionList);
		}
	}
}