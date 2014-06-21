#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	[Name("✫Say")]
	[Description("Will make the Dialogue Actor selected to talk.")]
	public class DLGStatementNode : DLGNodeBase{

		public Statement statement = new Statement("This is a dialogue text");

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		protected override NodeStates OnExecute(){

			if (!finalActor){
				DLGTree.StopGraph();
				return Error("Actor not found");
			}

			DLGTree.currentNode = this;
			var finalStatement = statement.BlackboardReplace(finalBlackboard);
			finalActor.Say(finalStatement, Continue);
			return NodeStates.Running;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
			labelStyle.wordWrap = true;

			var displayText = statement.text.Length > 60? statement.text.Substring(0, 60) + "..." : statement.text;
			GUILayout.Label("\"<i> " + displayText + "</i> \"", labelStyle);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			GUIStyle areaStyle = new GUIStyle(GUI.skin.GetStyle("TextArea"));
			areaStyle.wordWrap = true;
			
			EditorUtils.CoolLabel("Dialogue Text");
			statement.text = EditorGUILayout.TextArea(statement.text, areaStyle, GUILayout.Height(100));

			EditorUtils.CoolLabel("Audio File");
			statement.audio = EditorGUILayout.ObjectField(statement.audio, typeof(AudioClip), false)  as AudioClip;
			
			EditorUtils.CoolLabel("Meta Data");
			statement.meta = EditorGUILayout.TextField(statement.meta);
		}

		#endif
	}
}