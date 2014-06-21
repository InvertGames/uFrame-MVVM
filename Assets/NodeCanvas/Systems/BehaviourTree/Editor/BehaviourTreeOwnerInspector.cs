using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(BehaviourTreeOwner))]
	public class BehaviourTreeOwnerInspector : GraphOwnerInspector {

		BehaviourTreeOwner owner{
			get {return target as BehaviourTreeOwner; }
		}

		protected override void OnExtraOptions(){
			
			GUILayout.BeginHorizontal();
			owner.runForever = EditorGUILayout.Toggle("Run Forever", owner.runForever);
			if (owner.runForever)
				owner.updateInterval = EditorGUILayout.FloatField("Update Interval", owner.updateInterval );
			GUILayout.EndHorizontal();
		}
	}
}