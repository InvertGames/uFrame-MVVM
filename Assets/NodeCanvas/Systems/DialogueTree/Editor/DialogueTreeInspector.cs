using UnityEditor;
using UnityEngine;
using System.Collections;
using NodeCanvas;
using NodeCanvas.DialogueTrees;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(DialogueTree))]
	public class DialogueTreeInspector : GraphInspector{

		public DialogueTree DLGTree{
			get {return target as DialogueTree;}
		}

		public override void OnInspectorGUI(){

			base.OnInspectorGUI();

	        GUI.color = Color.yellow;
	        GUILayout.Label("Dialogue Actors");
	        GUI.color = Color.white;
			EditorGUILayout.HelpBox("Add the Names of the Dialogue Actors for this Dialogue Tree here. When Dialogue starts, all Actors must be present in the scene for the Dialogue to start correctly", MessageType.Info);

			GUILayout.BeginVertical("box");
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("0. Owner");
			GUILayout.FlexibleSpace();
			GUILayout.Label("(<< Optional Use. The Actor Starting the Dialogue Tree)");
			GUILayout.EndHorizontal();

			for (int i = 0; i < DLGTree.dialogueActorNames.Count; i++){

				GUILayout.BeginHorizontal();

				DLGTree.dialogueActorNames[i] = EditorGUILayout.TextField(DLGTree.dialogueActorNames[i]);

				GUI.backgroundColor = EditorUtils.lightRed;
				if (GUILayout.Button("X", GUILayout.Width(18)))
					DLGTree.dialogueActorNames.RemoveAt(i);
				GUI.backgroundColor = Color.white;
				GUILayout.EndHorizontal();
			}

			if (GUILayout.Button("Add New Actor")){
				DLGTree.dialogueActorNames.Add("actor name");
			}

			GUILayout.EndVertical();

			GUILayout.Space(10);

			if (Application.isPlaying){

				if (!DLGTree.isRunning && GUILayout.Button("Start Dialogue"))
					DLGTree.StartGraph();

				if (DLGTree.isRunning && GUILayout.Button("Stop Dialogue"))
					DLGTree.StopGraph();

				if (DLGTree.isRunning && GUILayout.Button("Pause Dialogue"))
					DLGTree.PauseGraph();
			} 

			if (GUI.changed)
				EditorUtility.SetDirty(DLGTree);
		}
	}
}