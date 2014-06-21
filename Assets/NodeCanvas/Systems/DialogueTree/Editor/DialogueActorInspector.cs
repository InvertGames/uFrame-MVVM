using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeCanvas;
using NodeCanvas.DialogueTrees;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(DialogueActor))]
	public class DialogueActorInspector : Editor {

		private DialogueActor actor{
			get {return target as DialogueActor;}
		}

		public override void OnInspectorGUI(){

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			actor.portrait = (Texture2D)EditorGUILayout.ObjectField(actor.portrait, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

			GUILayout.BeginVertical();
			actor.actorName = EditorGUILayout.TextField("Actor Name", actor.actorName);
			actor.dialogueColor = EditorGUILayout.ColorField("Dialogue Color", actor.dialogueColor);
			actor.dialogueOffset = EditorGUILayout.Vector3Field("Dialogue Offset", actor.dialogueOffset);
			actor.blackboard = (Blackboard)EditorGUILayout.ObjectField("Blackboard", actor.blackboard, typeof(Blackboard), true);
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();

			if (GUI.changed)
				EditorUtility.SetDirty(actor);
		}
	}
}